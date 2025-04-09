using UnityEngine;
using System.IO;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private string path => Path.Combine(Application.persistentDataPath, "save.json");

    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
        StartCoroutine(DelayedLoad());
    }

    public void SaveGame()
    {
        var data = new GameSaveData
        {
            money = MoneyManager.Instance.Money,
            inventory = InventoryManager.Instance.SaveInventory(),
            deliveryCooldowns = DeliveryTimerManager.Instance.SaveCooldowns()
        };

        File.WriteAllText(path, JsonUtility.ToJson(data));
        Debug.Log("Saved");
    }

    public void LoadGame()
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<GameSaveData>(json);

        MoneyManager.Instance.LoadMoney(data.money);
        InventoryManager.Instance.LoadInventory(data.inventory);
        DeliveryTimerManager.Instance.LoadCooldowns(data.deliveryCooldowns);

        Debug.Log("Loaded");
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitUntil(() => IngredientDatabase.Instance != null);
        LoadGame();
    }
}
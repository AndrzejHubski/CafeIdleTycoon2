using UnityEngine;
using System.IO;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private string path => Path.Combine(Application.persistentDataPath, "save.json");

    private void Start()
    {
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitUntil(() =>
            DeliveryTimerManager.Instance != null &&
            InventoryManager.Instance != null &&
            MoneyManager.Instance != null &&
            IngredientDatabase.Instance != null
        );

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
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
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file found.");
            return;
        }

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<GameSaveData>(json);

        MoneyManager.Instance.LoadMoney(data.money);
        InventoryManager.Instance.LoadInventory(data.inventory);
        DeliveryTimerManager.Instance.LoadCooldowns(data.deliveryCooldowns);

        Debug.Log("Game loaded.");
    }
}
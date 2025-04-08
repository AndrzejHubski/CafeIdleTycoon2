using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [Header("Links")]
    public GameObject clientPrefab;
    public Transform enterPoint;
    public Transform targetPoint;
    public Cook cook;
    public InventoryManager inventory;
    public List<DishData> allDishes;

    [Header("Settings")]
    public float delayBeforeNextClient = 1.5f;

    private Client currentClient;

    private void Start()
    {
        SpawnClient(); 
    }

    private void SpawnClient()
    {
        if (currentClient != null) return;

        DishData dish = GetRandomAvailableDish();
        if (dish == null)
        {
            Debug.LogWarning("No available dishes");
            return;
        }

        GameObject clientObj = Instantiate(clientPrefab, enterPoint.position, Quaternion.identity);
        currentClient = clientObj.GetComponent<Client>();

        currentClient.enterPoint = enterPoint;
        currentClient.targetPoint = targetPoint;
        currentClient.exitPoint = enterPoint;
        currentClient.SetOrder(dish);

        currentClient.OnArrivedAtRegister += (client, dishData) =>
        {
            cook.StartOrder(dishData, client);
        };

        currentClient.OnServed += HandleClientServed;
    }

    private void HandleClientServed()
    {
        currentClient = null;
        Invoke(nameof(SpawnClient), delayBeforeNextClient);
    }

    private DishData GetRandomAvailableDish()
    {
        var available = allDishes.FindAll(d => inventory.HasIngredients(d.requiredIngredients));
        Debug.Log($"Dishes available {available.Count}");

        if (available.Count == 0) return null;

        return available[Random.Range(0, available.Count)];
    }
}
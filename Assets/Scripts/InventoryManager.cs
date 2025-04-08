using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public class InventoryEntry
    {
        public IngredientData ingredient;
        public int count;
    }

    [Header("Ingredients")]
    public List<InventoryEntry> initialStock;

    private Dictionary<IngredientData, int> inventory = new();

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var entry in initialStock)
        {
            inventory[entry.ingredient] = entry.count;
            Debug.Log(entry.count);
        }
    }

    public int GetCount(IngredientData ingredient)
    {
        foreach (var kvp in inventory)
        {
            Debug.Log($"[{ingredient.name}] == [{kvp.Key.name}]? sameRef: {ingredient == kvp.Key}");
        }

        inventory.TryGetValue(ingredient, out int count);
        return count;
    }

    public void Add(IngredientData ingredient, int amount)
    {
        if (!inventory.ContainsKey(ingredient))
            inventory[ingredient] = 0;

        inventory[ingredient] += amount;
    }

    public bool TryUse(IngredientData ingredient, int amount)
    {
        if (GetCount(ingredient) < amount)
            return false;

        inventory[ingredient] -= amount;
        return true;
    }

    public bool TryBuy(IngredientData ingredient, int amount, int cost)
    {
        if (MoneyManager.Instance.Money < cost)
            return false;

        MoneyManager.Instance.AddMoney(-cost);
        Add(ingredient, amount);
        return true;
    }

    public Dictionary<IngredientData, int> GetAllInventory()
    {
        return new Dictionary<IngredientData, int>(inventory);
    }

    public bool HasIngredients(List<IngredientData> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            if (!ingredient.isUnlimited && GetCount(ingredient) <= 0)
                return false;
        }
        return true;
    }

    public void LoadInventory(List<IngredientEntry> list)
    {
        inventory.Clear();
        foreach (var entry in list)
        {
            var ingredient = IngredientDatabase.Instance.GetIngredientById(entry.id);
            if (ingredient != null)
                inventory[ingredient] = entry.count;
        }
    }

    public List<IngredientEntry> SaveInventory()
    {
        var list = new List<IngredientEntry>();
        foreach (var pair in inventory)
        {
            list.Add(new IngredientEntry
            {
                id = pair.Key.id,
                count = pair.Value
            });
        }
        return list;
    }
}
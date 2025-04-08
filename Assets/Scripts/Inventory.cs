using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<IngredientData, int> ingredients = new();

    public bool HasIngredients(List<IngredientData> required)
    {
        foreach (var ing in required)
        {
            if (ing.isUnlimited) continue;
            if (!ingredients.ContainsKey(ing) || ingredients[ing] <= 0)
                return false;
        }
        return true;
    }

    public void ConsumeIngredients(List<IngredientData> required)
    {
        foreach (var ing in required)
        {
            if (ing.isUnlimited) continue;
            ingredients[ing]--;
        }
    }

    public void AddIngredient(IngredientData ing, int amount)
    {
        if (!ingredients.ContainsKey(ing))
            ingredients[ing] = 0;
        ingredients[ing] += amount;
    }
}
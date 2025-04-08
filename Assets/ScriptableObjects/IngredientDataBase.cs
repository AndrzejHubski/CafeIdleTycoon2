using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDatabase", menuName = "Inventory/IngredientDatabase")]
public class IngredientDatabase : ScriptableObject
{
    public List<IngredientData> allIngredients;

    public static IngredientDatabase Instance;

    public IngredientData GetIngredientById(string id)
    {
        return allIngredients.Find(i => i.id == id);
    }

    private void OnEnable()
    {
        Instance = this;
    }
}
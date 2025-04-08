using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cafe/Dish")]
public class DishData : ScriptableObject
{
    public string dishName;
    public List<IngredientData> requiredIngredients;

    [Header("Visualisation")]
    public GameObject rawModelPrefab;     
    public GameObject modelPrefab;        

    [Header("Gameplay")]
    public int price = 10;
    public float baseCookTime = 5f;
}
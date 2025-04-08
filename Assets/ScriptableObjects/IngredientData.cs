using UnityEngine;

[CreateAssetMenu(menuName = "Cafe/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public string id;
    public Sprite icon; 
    public GameObject modelPrefab;
    public bool isUnlimited = false;

    [Header("Delivery")]
    public int deliveryAmount = 10;
    public int deliveryCost = 20;
    public float deliveryCooldown = 30f;
}
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int money;
    public List<IngredientEntry> inventory = new();
    public List<DeliveryCooldownEntry> deliveryCooldowns = new();
}

[System.Serializable]
public class IngredientEntry
{
    public string id;
    public int count;
}

[System.Serializable]
public class DeliveryCooldownEntry
{
    public string id;
    public float remainingTime;
}

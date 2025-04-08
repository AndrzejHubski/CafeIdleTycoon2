using System.Collections.Generic;
using UnityEngine;

public class DeliveryTimerManager : MonoBehaviour
{
    private Dictionary<IngredientData, float> cooldowns = new Dictionary<IngredientData, float>();
    private Dictionary<IngredientData, bool> cooldownStatus = new Dictionary<IngredientData, bool>();

    public static DeliveryTimerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartCooldown(IngredientData ingredient, float duration)
    {
        if (IsOnCooldown(ingredient)) return; 

        cooldowns[ingredient] = duration;
        cooldownStatus[ingredient] = true;
    }

    public bool IsOnCooldown(IngredientData ingredient)
    {
        return cooldownStatus.ContainsKey(ingredient) && cooldownStatus[ingredient];
    }

    public float GetCooldownRemaining(IngredientData ingredient)
    {
        if (cooldowns.ContainsKey(ingredient))
            return cooldowns[ingredient];
        return 0f;
    }

    public void UpdateTimers()
    {
        List<IngredientData> ingredients = new List<IngredientData>(cooldowns.Keys);
        foreach (var ingredient in ingredients)
        {
            if (cooldowns[ingredient] > 0)
            {
                cooldowns[ingredient] -= Time.deltaTime;
                if (cooldowns[ingredient] <= 0)
                {
                    cooldowns[ingredient] = 0;
                    cooldownStatus[ingredient] = false;
                }
            }
        }
    }

    public List<DeliveryCooldownEntry> SaveCooldowns()
    {
        var list = new List<DeliveryCooldownEntry>();
        foreach (var pair in cooldowns)
        {
            if (pair.Value > 0)
            {
                list.Add(new DeliveryCooldownEntry
                {
                    id = pair.Key.id,
                    remainingTime = pair.Value
                });
            }
        }
        return list;
    }


    public void LoadCooldowns(List<DeliveryCooldownEntry> list)
    {
        cooldowns.Clear();
        cooldownStatus.Clear();

        foreach (var entry in list)
        {
            var ingredient = IngredientDatabase.Instance.GetIngredientById(entry.id);
            if (ingredient != null)
            {
                cooldowns[ingredient] = entry.remainingTime;
                cooldownStatus[ingredient] = true;
            }
        }
    }

}
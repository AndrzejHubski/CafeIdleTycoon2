using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeData> upgrades;

    public List<GameObject> hireableCooks; 
    private int hiredCookCount = 0;

    private void Start()
    {
        Load(); 

        var cookUpgrade = upgrades.Find(u => u.type == UpgradeType.HireCook);
        if (cookUpgrade != null)
        {
            for (int i = 0; i < cookUpgrade.currentLevel && i < hireableCooks.Count; i++)
                hireableCooks[i].SetActive(true);

            hiredCookCount = Mathf.Clamp(cookUpgrade.currentLevel, 0, hireableCooks.Count);
        }
    }

    public void TryHireCook()
    {
        var upgrade = upgrades.Find(u => u.type == UpgradeType.HireCook);
        if (upgrade == null || hiredCookCount >= hireableCooks.Count) return;

        int cost = Mathf.CeilToInt(GetUpgradeCost(upgrade));
        if (MoneyManager.Instance.TrySpend(cost))
        {
            upgrade.currentLevel++;
            if (hiredCookCount < hireableCooks.Count)
            {
                hireableCooks[hiredCookCount].SetActive(true);
                hiredCookCount++;
            }
        }
    }


    public bool CanHireMoreCooks()
    {
        return hiredCookCount < hireableCooks.Count;
    }

    private void Awake()
    {
        Instance = this;
    }

    public bool TryUpgrade(UpgradeData upgrade)
    {
        int cost = Mathf.CeilToInt(GetUpgradeCost(upgrade));

        if (MoneyManager.Instance.TrySpend(cost))
        {
            upgrade.currentLevel++;
            return true;
        }

        return false;
    }

    public float GetUpgradeCost(UpgradeData upgrade)
    {
        return upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, upgrade.currentLevel);
    }

    public int GetUpgradeLevel(UpgradeType type)
    {
        var u = upgrades.Find(u => u.type == type);
        return u != null ? u.currentLevel : 0;
    }

    public float GetCookSpeedMultiplier()
    {
        int level = GetUpgradeLevel(UpgradeType.CookSpeed);
        return Mathf.Pow(0.95f, level); 
    }

    public float GetDeliveryCooldownMultiplier()
    {
        int level = GetUpgradeLevel(UpgradeType.DeliverySpeed);
        return Mathf.Pow(0.95f, level);
    }


    private string SavePath => Path.Combine(Application.persistentDataPath, "upgrades.json");

    public void Save()
    {
        var data = new UpgradeSaveData();
        foreach (var u in upgrades)
            data.levels[u.upgradeName] = u.currentLevel;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        var data = JsonUtility.FromJson<UpgradeSaveData>(json);

        foreach (var u in upgrades)
        {
            if (data.levels.TryGetValue(u.upgradeName, out int savedLevel))
            {
                u.currentLevel = savedLevel;

                if (u.type == UpgradeType.HireCook)
                {
                    for (int i = 0; i < savedLevel && i < hireableCooks.Count; i++)
                        hireableCooks[i].SetActive(true);

                    hiredCookCount = Mathf.Clamp(savedLevel, 0, hireableCooks.Count);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void ResetUpgrades()
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.currentLevel = 0;
        }

        for (int i = 0; i < hireableCooks.Count; i++)
            hireableCooks[i].SetActive(false);

        hiredCookCount = 0;
    }
}

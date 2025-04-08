using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;

    public UpgradeType type;

    public float baseCost = 100;
    public int currentLevel = 0;

    public float costMultiplier = 2.0f; 
}
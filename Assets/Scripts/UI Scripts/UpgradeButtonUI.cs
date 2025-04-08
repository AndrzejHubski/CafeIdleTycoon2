using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;

    private UpgradeData upgradeData;
    private UpgradeUI parentUI;

    public void Setup(UpgradeData data, UpgradeUI ui)
    {
        upgradeData = data;
        parentUI = ui;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(Upgrade);

        Refresh();
    }

    private void Upgrade()
    {
        if (upgradeData.type == UpgradeType.HireCook)
        {
            UpgradeManager.Instance.TryHireCook();
            Refresh();
        }
        else
        {
            if (UpgradeManager.Instance.TryUpgrade(upgradeData))
            {
                Refresh();
            }
        }
    }

    public void Refresh()
    {
        int level = upgradeData.currentLevel;
        int cost = Mathf.CeilToInt(UpgradeManager.Instance.GetUpgradeCost(upgradeData));

        if (upgradeData.type == UpgradeType.HireCook && !UpgradeManager.Instance.CanHireMoreCooks())
        {
            titleText.text = $"{upgradeData.upgradeName} (Max)";
            buttonText.text = "Max cooks";
            upgradeButton.interactable = false;
        }
        else
        {
            titleText.text = $"{upgradeData.upgradeName} (Lv.{level})";
            buttonText.text = $"Upgrade ({cost}$)";
            upgradeButton.interactable = true;
        }
    }
}
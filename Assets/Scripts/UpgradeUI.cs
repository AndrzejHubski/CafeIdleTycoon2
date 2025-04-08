using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public Transform contentParent;
    public GameObject upgradeButtonPrefab;

    private void Start()
    {
        foreach (var upgrade in UpgradeManager.Instance.upgrades)
        {
            var obj = Instantiate(upgradeButtonPrefab, contentParent);
            var ui = obj.GetComponent<UpgradeButtonUI>();
            ui.Setup(upgrade, this);
        }
    }

    public void TogglePanel()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }
}
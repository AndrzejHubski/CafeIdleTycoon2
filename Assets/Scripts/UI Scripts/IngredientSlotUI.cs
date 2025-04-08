using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public Button orderButton;
    public TextMeshProUGUI orderButtonText;

    private IngredientData ingredient;
    private InventoryUI inventoryUI;

    public IngredientData Ingredient => ingredient;




    public void Setup(IngredientData data, int count, InventoryUI ui)
    {
        ingredient = data;
        inventoryUI = ui;

        icon.sprite = data.icon;
        icon.enabled = data.icon != null;

        countText.text = data.isUnlimited ? "-" : count.ToString();

        if (data.isUnlimited)
        {
            orderButton.gameObject.SetActive(false);
        }
        else
        {
            orderButton.gameObject.SetActive(true);
            orderButton.onClick.RemoveAllListeners();
            orderButton.onClick.AddListener(() => TryOrder());

            UpdateButtonText();
        }
    }


    private void TryOrder()
    {
        if (DeliveryTimerManager.Instance.IsOnCooldown(ingredient) || ingredient.isUnlimited) return;

        bool success = InventoryManager.Instance.TryBuy(ingredient, ingredient.deliveryAmount, ingredient.deliveryCost);
        if (success)
        {
            float adjustedCooldown = ingredient.deliveryCooldown * UpgradeManager.Instance.GetDeliveryCooldownMultiplier();
            DeliveryTimerManager.Instance.StartCooldown(ingredient, adjustedCooldown);
            inventoryUI.RefreshInventory();  
        }
    }

    private void UpdateButtonText()
    {
        if (DeliveryTimerManager.Instance.IsOnCooldown(ingredient))
        {
            float remainingTime = DeliveryTimerManager.Instance.GetCooldownRemaining(ingredient);
            orderButtonText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
        else
        {
            orderButtonText.text = $"BUY ({ingredient.deliveryCost}$)";
        }
    }

    public void UpdateSlot(int newCount)
    {
        if (!ingredient.isUnlimited)
        {
            countText.text = newCount.ToString();
        }
    }

    private void Update()
    {
        if (DeliveryTimerManager.Instance.IsOnCooldown(ingredient))
        {
            float remainingTime = DeliveryTimerManager.Instance.GetCooldownRemaining(ingredient);
            orderButtonText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
        else
        {
            orderButtonText.text = $"BUY ({ingredient.deliveryCost}$)";
        }
    }
}
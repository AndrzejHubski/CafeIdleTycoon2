using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Links")]
    public GameObject inventoryPanel;
    public Transform contentParent;
    public GameObject slotPrefab;

    private List<IngredientSlotUI> activeSlots = new();

    public void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
            RefreshInventory();
    }



    public void RefreshInventory()
    {
        if (activeSlots.Count == 0)
        {
            foreach (var pair in InventoryManager.Instance.GetAllInventory())
            {
                IngredientData ingredient = pair.Key;
                int count = pair.Value;

                GameObject slotObj = Instantiate(slotPrefab, contentParent);
                var slot = slotObj.GetComponent<IngredientSlotUI>();
                slot.Setup(ingredient, count, this);

                activeSlots.Add(slot);
            }
        }
        else
        {
            foreach (var slot in activeSlots)
            {
                int currentCount = InventoryManager.Instance.GetCount(slot.Ingredient);
                slot.UpdateSlot(currentCount);
            }
        }
    }

    private void Update()
    {
        if (!inventoryPanel.activeSelf) return;

        DeliveryTimerManager.Instance.UpdateTimers();

        foreach (var slot in activeSlots)
        {
            int currentCount = InventoryManager.Instance.GetCount(slot.Ingredient);
            slot.UpdateSlot(currentCount);
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
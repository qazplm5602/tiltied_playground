using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Image[] itemImages;

    private void OnEnable()
    {
        inventory.OnSlotChangeEvent += UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            if (!inventory.slots[i].IsEmpty())
            {
                itemImages[i].sprite = inventory.slots[i].item.itemIcon;
            }
            else
            {
                itemImages[i].sprite = null;
            }
        }
    }
}

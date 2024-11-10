using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 게임 매니저가 P1과 P2의 Inventory를 가지고 있고,
    // 아이템을 먹으면 p1(또는 2)Inventory.AddItem,
    // 사용하면 p1(또는 2)Inventory.UseItem을 호출하면 됨

    public List<Slot> slots = new(2);
    public event Action OnSlotChangeEvent;

    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void AddItem(Item newItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].item = newItem;
                OnSlotChangeEvent?.Invoke();

                return;
            }
        }

        Debug.Log("슬롯이 가득 차서 아이템이 소멸됨");
    }

    public void UseItem()
    {
        if (!slots[0].IsEmpty())
        {
            slots[0].item.Use();
            slots[0].ClearSlot();
            ChangeSlot();

            OnSlotChangeEvent.Invoke();
        }

        Debug.Log("아이템이 없음");
    }

    public void ChangeSlot()
    {
        (slots[0], slots[1]) = (slots[1], slots[0]); // 수왑
    }
}

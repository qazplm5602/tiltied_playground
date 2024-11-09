using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // ���� �Ŵ����� P1�� P2�� Inventory�� ������ �ְ�,
    // �������� ������ p1(�Ǵ� 2)Inventory.AddItem,
    // ����ϸ� p1(�Ǵ� 2)Inventory.UseItem�� ȣ���ϸ� ��

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

        Debug.Log("������ ���� ���� �������� �Ҹ��");
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

        Debug.Log("�������� ����");
    }

    public void ChangeSlot()
    {
        (slots[0], slots[1]) = (slots[1], slots[0]); // ����
    }
}

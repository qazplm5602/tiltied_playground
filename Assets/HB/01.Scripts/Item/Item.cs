using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    [SerializeField] private ItemSO[] _itemSO;
    public int buffTime;

    public event Action OnDestroyEvent;

    public virtual void Use()
    {
        Debug.Log("������ ���");
    }

    private IEnumerator GetItem(Player player)
    {

        for (int i = 0; i < _itemSO.Length; i++)
        {
            player.PlayerStatSO.AddModifier(_itemSO[i].statType, _itemSO[i].value);
            
        }

        yield return new WaitForSeconds(buffTime);

        for (int i = 0; i < _itemSO.Length; i++)
        {
            player.PlayerStatSO.RemoveModifier(_itemSO[i].statType, _itemSO[i].value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.attachedRigidbody)
        {
            Debug.Log(other.name);
            if (other.TryGetComponent<Player>(out Player player))
            {
                StartCoroutine(GetItem(player));
                OnDestroyEvent?.Invoke();
            }
        }
    }
}
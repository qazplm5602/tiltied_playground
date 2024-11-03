using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO[] _itemSO;
    public int buffTime;

    private IEnumerator GetItem(HB_Player player)
    {

        for (int i = 0; i < _itemSO.Length; i++)
        {
            player.Stat.AddModifier(_itemSO[i].statType, _itemSO[i].value);
        }

        yield return new WaitForSeconds(buffTime);

        for (int i = 0; i < _itemSO.Length; i++)
        {
            player.Stat.RemoveModifier(_itemSO[i].statType, _itemSO[i].value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.attachedRigidbody)
        {
            if (other.TryGetComponent<HB_Player>(out HB_Player player))
            {
                StartCoroutine(GetItem(player));
            }
        }
    }
}
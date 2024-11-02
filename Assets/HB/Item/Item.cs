using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Player _player;
    [SerializeField] private ItemSO[] _itemSO;
    public int buffTime;


    public IEnumerator GetItem(Player player)
    {
        for (int i = 0; i < _itemSO.Length; i++)
        {
            player.Stat.AddModifier(_itemSO[i].statType, _itemSO[i].value);
            yield return new WaitForSeconds(buffTime);
            player.Stat.RemoveModifier(_itemSO[i].statType, _itemSO[i].value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.attachedRigidbody)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                GetItem(player);
            }
        }
    }
}
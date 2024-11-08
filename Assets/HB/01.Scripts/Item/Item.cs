using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    [SerializeField] private ItemSO[] _itemSO;
    public int buffTime;

    public virtual void Use()
    {
        Debug.Log("아이템 사용");

        // 닿은 플레이어 스크립트 가져와서 GetItem 부르면 됨
    }

    // 획득 판정 (다른 스크립트로 이동해야 할 듯) 아이템 먹으면 무조건 사라져야됨

    /*private IEnumerator GetItem(Player player)
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
            if (other.TryGetComponent<Player>(out Player player))
            {
                StartCoroutine(GetItem(player));
            }
        }
    }*/
}
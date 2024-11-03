using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LightTransport;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO[] _itemSO;
    public int buffTime;

    private string objName = "";
    [SerializeField] private List<string> nameList = new();

    private void OnEnable()
    {
        objName = gameObject.name;

        nameList = objName.Split("/").ToList();

        buffTime = int.Parse(nameList[nameList.Count - 1]);
    }

    private IEnumerator GetItem(Player player)
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
            if (other.TryGetComponent<Player>(out Player player))
            {
                StartCoroutine(GetItem(player));
            }
        }
    }
}
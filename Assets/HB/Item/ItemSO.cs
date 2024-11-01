using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private Player _player; // ���߿� PlayerManager.Instance.Player.Stat~~�� �ٲܰ���

    public StatType statType;
    public int value;
    public int buffTime;

    public IEnumerator GetItem()
    {
        _player.Stat.AddModifier(statType, value);
        yield return new WaitForSeconds(buffTime);
        _player.Stat.RemoveModifier(statType, value);
    }
}

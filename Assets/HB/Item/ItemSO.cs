using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public StatType statType;
    public int value;
}

using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public StatType statType;
    
    public Texture texture;
    public Sprite sprite;
    public int value;
    public Vector3 appendingScale;
    [Tooltip("���� �ð�")] public float lastTime = 2f;
}

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "SO/Stat/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("���� ���� �� ������ ����")]
    [Tooltip("�̸�")]           public string playerName;
    [Tooltip("����")]           public string nationlity;
    [Tooltip("�̹���")]         public Sprite icon;
    [Tooltip("Ű")]             public float height;
    [Tooltip("������")]         public float weight;

    [Header("�ɷ�ġ")]
    [Tooltip("�⺻ �ӵ� ")]     public Stat defaultSpeed;
    [Tooltip("�޸��� �ӵ�")]    public Stat runSpeed;
    [Tooltip("�� ������")]      public Stat goalDecision;
    [Tooltip("�� �Ŀ�")]        public Stat shootPower;
}
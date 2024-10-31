using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "SO/Stat/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("선수 선택 시 보여질 정보")]
    [Tooltip("이름")]           public string playerName;
    [Tooltip("국적")]           public string nationlity;
    [Tooltip("이미지")]         public Sprite icon;
    [Tooltip("키")]             public float height;
    [Tooltip("몸무게")]         public float weight;

    [Header("능력치")]
    [Tooltip("기본 속도 ")]     public Stat defaultSpeed;
    [Tooltip("달리기 속도")]    public Stat runSpeed;
    [Tooltip("골 결정력")]      public Stat goalDecision;
    [Tooltip("슛 파워")]        public Stat shootPower;
}
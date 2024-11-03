using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "SO/Stat/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("선수 프로필")]
    [Tooltip("이름")] public string playerName;
    [Tooltip("국적")] public string nationlity;
    [Tooltip("이미지")] public Sprite icon;
    [Tooltip("키")] public float height;
    [Tooltip("몸무게")] public float weight;

    [Header("능력치")]
    [Tooltip("기본 속도 ")] public Stat defaultSpeed;
    [Tooltip("달리기 속도")] public Stat runSpeed;
    [Tooltip("골 결정력")] public Stat goalDecision;
    [Tooltip("슛 파워")] public Stat shootPower;

    private Player _player;
    private Dictionary<StatType, Stat> _statDictionary;

    public void SetOwner(Player player)
    {
        _player = player;
    }

    protected virtual void OnEnable()
    {
        _statDictionary = new Dictionary<StatType, Stat>();

        Type agentStatType = typeof(PlayerStatsSO);

        foreach (StatType enumType in Enum.GetValues(typeof(StatType)))
        {
            try
            {
                string fieldName = LowerFirstChar(enumType.ToString());

                FieldInfo statField = agentStatType.GetField(fieldName);
                _statDictionary.Add(enumType, statField.GetValue(this) as Stat);

            }
            catch (Exception ex)
            {
                Debug.LogError($"There are no stat - {enumType.ToString()}, {ex.Message}");
            }
        }
    }

    private string LowerFirstChar(string input) => $"{char.ToLower(input[0])}{input.Substring(1)}";

    public void AddModifier(StatType type, int value)
    {
        _statDictionary[type].AddModifier(value);
    }

    public void RemoveModifier(StatType type, int value)
    {
        _statDictionary[type].RemoveModifier(value);
    }

    public void ShowStats()
    {
        Debug.Log($"{_player.gameObject.name}:");
        Debug.Log($"{_player.Stat._statDictionary[StatType.GoalDecision].GetValue()}");
        Debug.Log($"{_player.Stat._statDictionary[StatType.DefaultSpeed].GetValue()}");
        Debug.Log($"{_player.Stat._statDictionary[StatType.RunSpeed].GetValue()}");
        Debug.Log($"{_player.Stat._statDictionary[StatType.ShootPower].GetValue()}");
    }
}
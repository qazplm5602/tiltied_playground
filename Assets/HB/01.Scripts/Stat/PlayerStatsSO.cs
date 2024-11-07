using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "SO/Stat/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("���� ������")]
    [Tooltip("�̸�")] public string playerName;
    [Tooltip("����")] public string nationlity;
    [Tooltip("�̹���")] public Sprite icon;
    [Tooltip("Ű (cm)")] public float height;
    [Tooltip("������ (kg)")] public float weight;

    [Header("�ɷ�ġ")]
    [Tooltip("�⺻ �ӵ�")] public Stat defaultSpeed;
    [Tooltip("�޸��� �ӵ�")] public Stat runSpeed;
    [Tooltip("�� ������ (%)")] public Stat goalDecision;
    [Tooltip("�� �Ŀ�")] public Stat shootPower;

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
}
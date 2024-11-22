using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Control/TeamControlsSO")]
public class TeamControlsSO : ScriptableObject
{
    [System.Serializable]
    struct Value {
        public BallAreaType team;
        public PlayerControlSO control;
    }

    [SerializeField] private Value[] values;
    private Dictionary<BallAreaType, PlayerControlSO> controls;

    private void OnEnable() {
        foreach (var item in values)
            controls[item.team] = item.control;
    }

    public PlayerControlSO GetControlByTeam(BallAreaType team) {
        controls.TryGetValue(team, out var control);
        return control;
    }
}

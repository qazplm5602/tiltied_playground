using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Dictionary<BallAreaType, Player> players;
    private Dictionary<BallAreaType, Transform> spawnPos;
    [SerializeField] private Player basePlayer;
    [SerializeField] private string spawnParentName;

    [SerializeField] private PlayerControlSO control1;
    [SerializeField] private PlayerControlSO control2;

    private Transform spawnParent;

    private void Awake() {
        players = new();
        spawnPos = new();

        spawnParent = GameObject.Find(spawnParentName)?.transform;
        if (spawnParent == null)
            Debug.LogWarning("Not Found Player Spawn Parent.");

        FindSpawnPos();
        CreatePlayer();
    }

    private void FindSpawnPos() {
        foreach (BallAreaType item in Enum.GetValues(typeof(BallAreaType)))
        {
            Transform pointTrm = GameObject.Find($"PlayerSpawnPoints/{item}").transform;
            if (pointTrm == null) {
                Debug.LogWarning($"{item} spawn point not found.");
                continue;
            }

            spawnPos.Add(item, pointTrm);
        }
    }

    private void CreatePlayer() {
        GameDataManager data = GameDataManager.Instance;
        if (data == null) {
            Debug.LogWarning("Game Data Missing! Can Not Create Player.");
            // return;
        }
        
        // (이거 data에 있는 prefab 으로 생성할 예정...)
        // 데이터에 있는거 꺼내와서 적용할꺼
        Player player_1 = Instantiate(basePlayer, spawnParent);
        player_1.SetControl(control1);

        Player player_2 = Instantiate(basePlayer, spawnParent);
        player_2.SetControl(control2);

        // 임시 스탯 적용
        if (data) {
            player_1.SetStat(data.player1_StatData);
            player_2.SetStat(data.player2_StatData);
        }

        player_1.gameObject.SetActive(false);
        player_2.gameObject.SetActive(false);

        players.Add(BallAreaType.Blue, player_1);
        players.Add(BallAreaType.Red, player_2);
    }   

    public void ResetPos() {
        foreach (var item in players)
        {
            if (spawnPos.TryGetValue(item.Key, out Transform point))
                item.Value.transform.position = point.position;
        }
    }

    public Player GetPlayer(BallAreaType team) {
        if (players.TryGetValue(team, out var player))
            return player;

        return null;
    }
}
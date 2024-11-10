using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheerleaderManager : MonoBehaviour
{
    [SerializeField] private CheerleaderListSO characters;

    private List<CheerleaderNPC> redCheers;
    private List<CheerleaderNPC> blueCheers;

    private SoccerBall ball;

    private void Awake() {
        ball = FindAnyObjectByType<SoccerBall>();
        ball.OnGoal += HandleBallGoal;

        CreatePeople();
    }

    private void CreatePeople() {
        redCheers = new();
        blueCheers = new();

        // 스폰 포인트 찾기
        CheerleaderPoint[] points = FindObjectsByType<CheerleaderPoint>(FindObjectsSortMode.None);

        foreach (var item in points)
        {
            Transform peopleTrm = item.transform.parent.parent.Find("People");
            Transform prefab = characters.GetRandom().transform;
            CheerleaderNPC npc = Instantiate(characters.GetRandom(), peopleTrm);

            npc.allowSeat = item.AllowSeat;
            npc.transform.position = item.transform.position;
            npc.transform.rotation = item.transform.rotation;

            GetNpcs(item.Team).Add(npc);
        }
    }

    List<CheerleaderNPC> GetNpcs(BallAreaType team) => team == BallAreaType.Blue ? blueCheers : redCheers;

    private void HandleBallGoal(BallAreaType team) {
        List<CheerleaderNPC> npcs = GetNpcs(team)
            .Where(v => Random.Range(0, 20) != Random.Range(0, 20)) // 골 넣어도 응원 안하는 사람도 있지 않나?? (좀 적음)
            .ToList();

        foreach (var item in npcs)
        {
            if (Random.Range(0, 5) == Random.Range(0, 5)) // 소수의 사람은 그냥 박수만 침
                item.CheerPlay();
            else
                item.Dance(); // 춤춤춤
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class CheerleaderManager : MonoBehaviour
{
    [SerializeField] private CheerleaderListSO characters;

    private List<CheerleaderNPC> redCheers;
    private List<CheerleaderNPC> blueCheers;

    private void Awake() {
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

            (item.Team == BallAreaType.Blue ? blueCheers : redCheers).Add(npc);
        }


    }
}

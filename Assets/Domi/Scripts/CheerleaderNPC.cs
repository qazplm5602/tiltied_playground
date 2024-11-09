using System.Collections;
using UnityEngine;

public class CheerleaderNPC : MonoBehaviour
{
    // 애님
    private static readonly int ANIM_SEAT = Animator.StringToHash("seat");
    private static readonly int ANIM_BLEND = Animator.StringToHash("Blend");
    private static readonly int ANIM_CHEER = Animator.StringToHash("cheer");
    private static readonly int ANIM_DANCE = Animator.StringToHash("dance");

    public bool allowSeat = true; // 앉을 수 있는 자리임? (앉을 수 있으면 랜덤으로 앉기도 함)
    [SerializeField] private Animator anim;
    [SerializeField] private Vector2 cheerRange = new Vector2(10, 20);

    private float cheerTime = 0;
    private bool seatting = false;

    private void Awake() {
        cheerTime = Random.Range(cheerRange.x, cheerRange.y); // 응원 랜덤
    }
    
    private void Start() {
        SetSeat();
    }

    private void Update() {
        cheerTime -= Time.deltaTime;

        if (cheerTime <= 0) {
            cheerTime = Random.Range(cheerRange.x, cheerRange.y);

            anim.SetFloat(ANIM_BLEND, Random.Range(0, seatting ? 3 : 2));
            anim.SetTrigger(ANIM_CHEER); // 춤 ㄱㄱㄱ
        }
    }

    // 앉을래 말래??
    private void SetSeat() {
        if (!allowSeat) return;

        if (Random.Range(0, 3) > 0) {
            anim.SetBool(ANIM_SEAT, true);
            seatting = true;
        }
    }

    public void CheerPlay(bool force = false) {
        cheerTime = force ? 0 : Random.Range(0, 1f);
    }

    public void Dance(bool force = false) {
        StartCoroutine(WaitDance(force ? 0 : Random.Range(0, 1f)));
    }

    IEnumerator WaitDance(float wait) {
        yield return new WaitForSeconds(wait);
        anim.SetTrigger(ANIM_DANCE);
    }
}
using System;
using UnityEngine;

public class PlayerBallAttack : MonoBehaviour
{
    [SerializeField] Player testPlayer;
    [SerializeField] float takeDistance = 1;
    [SerializeField] LayerMask whatIsBall;
    [SerializeField] float allowRange = -0.5f;
    
    private Player player;
    private Collider[] detectBall;

    private void Awake() {
        player = GetComponent<Player>();
        
        player.AttackEvent += HandleAttack;
        detectBall = new Collider[1];
    }

    
    private void HandleAttack()
    {
        Player ballOwner = BallControlBundle.GetBallOwner();
        if (ballOwner == null) return; // 공 주인 없는뎅

        int detectCount = Physics.OverlapSphereNonAlloc(transform.position, takeDistance, detectBall, whatIsBall);
        if (detectCount == 0) return;

        Vector3 dirToTarget = transform.position - ballOwner.transform.position;
        Vector3 corss = Vector3.Cross(ballOwner.transform.right, dirToTarget);

        if (corss.y > allowRange) return; // 플레이어 뒤에 있는데??

        player.ForceTakeBall(); // 강제적으로 공 뺏음
    }

    private void Update() {
        



        // Debug.DrawLine(transform.position + Vector3.up * 2, testPlayer.transform.position + Vector3.up * 2, corss.y <= -0.5f ? Color.red : Color.blue);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, takeDistance);

        Gizmos.color = Color.white;
    }


}

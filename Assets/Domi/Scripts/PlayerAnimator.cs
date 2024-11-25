using System;
using System.Collections.Generic;
using UnityEngine;

enum PlayerAnimState : byte {
    Idle,
    Walk,
    Kick
}

public class PlayerAnimator : MonoBehaviour
{
    private readonly int ANIM_WALK = Animator.StringToHash("Walk");
    private readonly int ANIM_KICK = Animator.StringToHash("Kick");
    private readonly int ANIM_SPEED = Animator.StringToHash("Speed");

    [SerializeField] float kickDuration = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private Dictionary<PlayerAnimState, int> animHashes;

    private Player player;
    private PlayerAnimState currentState;

    private float kickTime = 0f;
    
    private void Awake() {
        animHashes = new();
        player = GetComponent<Player>();
        player.ShootingRunEvent += Kick;

        foreach (PlayerAnimState item in Enum.GetValues(typeof(PlayerAnimState)))
        {
            if ((byte)item != 0)
                animHashes.Add(item, Animator.StringToHash(item.ToString()));
        }
    }

    private void OnDestroy() {
        player.ShootingRunEvent -= Kick;
    }

    private void ChangeState(PlayerAnimState state) {
        if (currentState != 0)
            animator.SetBool(animHashes[currentState], false);
        
        if (state != 0)
            animator.SetBool(animHashes[state], true);
    
        currentState = state;
    }

    private void Update() {
        if (kickTime > 0) {
            kickTime -= Time.deltaTime;
        } else if (currentState == PlayerAnimState.Kick)
            ChangeState(PlayerAnimState.Idle);

        Move();
    }
    
    private void Move() {
        if (currentState != PlayerAnimState.Idle && currentState != PlayerAnimState.Walk) return;

        Vector2 inputDir = player.PlayerControlSO.GetMoveDirection();
        bool running = !Mathf.Approximately(inputDir.sqrMagnitude, 0.0f);
        
        if (running && currentState == PlayerAnimState.Idle) {
            ChangeState(PlayerAnimState.Walk);
        } else if (!running && currentState == PlayerAnimState.Walk) {
            ChangeState(PlayerAnimState.Idle);
        }

        if (currentState == PlayerAnimState.Walk) {
            animator.SetFloat(ANIM_SPEED, player.GetNowSpeed() * 0.1f);
        }
    }
    
    private void Kick() {
        kickTime = kickDuration;
        ChangeState(PlayerAnimState.Kick);
    }
}

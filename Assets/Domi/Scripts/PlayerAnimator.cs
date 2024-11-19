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

    [SerializeField] float kickDuration = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private Dictionary<PlayerAnimState, int> animHashes;

    private Player player;
    private PlayerAnimState currentState;

    private float kickTime = 0f;
    
    private void Awake() {
        animHashes = new();
        player = GetComponent<Player>();
        player.ShootingEndEvent += Kick;

        foreach (PlayerAnimState item in Enum.GetValues(typeof(PlayerAnimState)))
        {
            if ((byte)item != 0)
                animHashes.Add(item, Animator.StringToHash(item.ToString()));
        }
    }

    private void OnDestroy() {
        player.ShootingEndEvent -= Kick;
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
        bool running = inputDir.sqrMagnitude > 0.1f;
        
        if (running && currentState == PlayerAnimState.Idle) {
            ChangeState(PlayerAnimState.Walk);
        } else if (!running && currentState == PlayerAnimState.Walk) {
            ChangeState(PlayerAnimState.Idle);
        }
    }
    
    private void Kick() {
        kickTime = kickDuration;
        ChangeState(PlayerAnimState.Kick);
    }
}

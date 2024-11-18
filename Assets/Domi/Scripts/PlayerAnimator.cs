using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int ANIM_WALK = Animator.StringToHash("Walk");
    private readonly int ANIM_KICK = Animator.StringToHash("Kick");

    [SerializeField] float kickDuration = 1f;
    [SerializeField] private Animator animator;

    private Player player;
    private float kickTime = 0f;
    
    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        
    }
    
    private void Move() {
        if (kickTime > 0) return;

        Vector2 inputDir = player.PlayerControlSO.GetMoveDirection();
        // animator.SetBool()
    }

    private void AllDisable() {
        animator.SetBool(ANIM_WALK, false);
        animator.SetBool(ANIM_KICK, false);
    }
}

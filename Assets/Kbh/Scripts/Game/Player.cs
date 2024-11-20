using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Requires")]
    [field: SerializeField] public PlayerStatsSO PlayerStatSO { get; private set; }
    [field: SerializeField] public PlayerControlSO PlayerControlSO { get; private set; }
    [field: SerializeField] public Rigidbody RigidbodyComponent { get; private set; }

    [Header("Bundles")]
    [SerializeField] private BallControlBundle _ballController;


    private const int MAX_ITEM_COUNT = 2;
    [SerializeField] private int[] _itemIDs;

    [Tooltip("아이템 미리보기를 위한 이미지들")]
    [SerializeField] private Sprite[] _itemImages;

    [SerializeField] private bool isKnockback = false;
    [SerializeField] private float shootTakeDelay = 0.1f; // 슈팅 한 후 공을 가져갈 수 있는 쿨탐

    TagHandle _ballTag;
    private bool HasBall() => BallControlBundle.GetBallOwner() == this;

    private bool _prevShootKeyDown = false;
    private float _prevShootKeyDownTime = 0.0f;
    private float shootTime;

    private SkillBase _currentSkill;
    public SkillBase CurrentSkill => _currentSkill;
    public event Action ShootingStartEvent;
    public event Action ShootingEndEvent;
    public event Action AttackEvent; // 공 없이 슈팅 누른 경우

    private void Awake()
    {
        PlayerStatSO = Instantiate(PlayerStatSO);
        SkillInit();

        RigidbodyComponent = GetComponent<Rigidbody>();
        _itemIDs = new int[MAX_ITEM_COUNT];
        _ballTag = TagHandle.GetExistingTag("Ball");
    }

   private void Start() {
      PlayerControlSO.ItemChangeEvent += HandleItemChange;
      PlayerControlSO.ItemUseEvent += HandleItemUse;
      PlayerControlSO.InteractEvent += HandleInterect;
      PlayerControlSO.SkillEvent += HandleSkillUse;
   }

   private void FixedUpdate()
   {
      if(isKnockback)
      {
         // 다른 플레이어로부터 타격을 받았을 경우
      }
      else
      {
         Movement();
      }
   }



    private void Movement()
    {
        Vector2 inputDir = -PlayerControlSO.GetMoveDirection(); // 카메라가 반대라서 인풋도 반대임 ㅋㅋ
        if (inputDir.x == 0 && inputDir.y == 0) return;

        inputDir.Normalize();
        Vector3 moveDir = new(inputDir.x, 0, inputDir.y);



        transform.localPosition
           += moveDir * PlayerStatSO.defaultSpeed.GetValue() * Time.fixedDeltaTime;

        transform.localRotation
           = Quaternion.Lerp(transform.localRotation,
           Quaternion.Euler(0, Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg, 0), 0.1f);
    }

    private void OnDestroy()
    {
        PlayerControlSO.ItemChangeEvent -= HandleItemChange;
        PlayerControlSO.ItemUseEvent -= HandleItemUse;
        PlayerControlSO.InteractEvent -= HandleInterect;
        PlayerControlSO.SkillEvent -= HandleSkillUse;
    }


    private void HandleInterect(bool isPerformed)
    {
        if (HasBall())
        {
            if (isPerformed)
            {
                TryInterect();

                StartCoroutine(ShootTimer());
            }
            else
                Shooting();
        } else if (isPerformed) { // 축구공은 안가지고 있는데 슈팅 누름
            AttackEvent?.Invoke();
        }
    }

    private void TryInterect()
    {
        _prevShootKeyDown = true;
        _prevShootKeyDownTime = Time.time;

        ShootingStartEvent?.Invoke();
    }

    private void Shooting()
    {
        if (!_prevShootKeyDown) return;

        float currentGauge = Time.time - _prevShootKeyDownTime;

        _ballController.PushBall((transform.forward + transform.up).normalized * currentGauge * PlayerStatSO.shootPower.GetValue() * 30);
        this.Release(_ballController);

        _prevShootKeyDown = false;
        shootTime = Time.time;

        ShootingEndEvent?.Invoke();
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(5);

        Shooting();
    }

    private void HandleItemUse()
    {
        // itemIDs[0]를 사용하고
        _itemIDs[0] = 0; // 비운다. 
    }

    private void HandleItemChange()
    {
        (_itemIDs[0], _itemIDs[1]) = (_itemIDs[1], _itemIDs[0]);
    }
    
    private void HandleSkillUse()
    {
        if (_currentSkill.SkillUseAbleCheck())
            _currentSkill.UseSkill();
    }
    
    // 강제적으로 공 가져옴
    public void ForceTakeBall() {
        this.Registe(_ballController);
    }
    // 강제적으로 공 뺏음 ㅅㄱ
    public void ForceReleseBall() {
        this.Release(_ballController);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(_ballTag) // 공에 닿았고
           && _ballController.BallIsFree() // 공이 자유롭다면
           && Time.time - shootTime > shootTakeDelay // 잡기 쿨탐
        )
        {
            this.Registe(_ballController);
        }

    }

    public void SetControl(PlayerControlSO control)
    {
        PlayerControlSO = control;
    }

    public void SetStat(PlayerStatsSO stat)
    {
        PlayerStatSO = stat;
        SkillInit();
    }

    private void SkillInit()
    {
        if(_currentSkill != null)
            return;
        
        _currentSkill = Instantiate(SkillManager.Instance.GetSkill(PlayerStatSO.skillData.skillType), transform);
        _currentSkill.Init(this);
    }
}
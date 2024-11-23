using System;
using System.Collections;
using UnityEditor;
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
    private Coroutine shootWait;

    private SkillBase _currentSkill;
    public SkillBase CurrentSkill => _currentSkill;
    public event Action ShootingStartEvent; // 슈팅 시작
    public event Action ShootingEndEvent; // 슈팅 끝 (찼거나 아니면 취소 되었거나)
    public event Action ShootingRunEvent; // 진짜 공 참
    public event Action AttackEvent; // 공 없이 슈팅 누른 경우
    public event Action<float> BlindEvent; // 블라인드 스킬을 맞은 경우

    private bool _isMeditation = false;
    public bool IsMeditation {
        get => _isMeditation;
        set { _isMeditation = value; }
    }

    [SerializeField] private SoundSO _shootingSound;

    private void Awake()
    {
        PlayerStatSO = Instantiate(PlayerStatSO);

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
      if (_isMeditation) return;

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



        int speedValue = HasBall() ? PlayerStatSO.dribbleSpeed.GetValue() : PlayerStatSO.defaultSpeed.GetValue();

        transform.localPosition
           += moveDir * speedValue * Time.fixedDeltaTime;

        transform.localRotation
           = Quaternion.Lerp(transform.localRotation,
           Quaternion.Euler(0, Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg, 0), 0.3f);
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

                shootWait = StartCoroutine(ShootTimer());
            }
            else {
                Shooting();
                ShootStop(false);
            }
        } 
        else
        {
            if (isPerformed)
            { // 축구공은 안가지고 있는데 슈팅 누름
                AttackEvent?.Invoke();
            }
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
        SoundManager.Instance.PlaySFX(Vector3.zero, _shootingSound);

        _prevShootKeyDown = false;
        shootTime = Time.time;

        ShootingRunEvent?.Invoke();
        ShootingEndEvent?.Invoke();
    }

    private void ShootStop(bool callEvent = true) {
        if (shootWait == null) return;
        StopCoroutine(shootWait);

        _prevShootKeyDown = false;

        if (callEvent)
            ShootingEndEvent?.Invoke();
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(4);

        shootWait = null;
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
        ShootStop();
        this.Release(_ballController);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (
            ManagerManager.GetManager<GameManager>().GetMode().IsPlay // 플레이 중이여야 함
           && collision.collider.CompareTag(_ballTag) // 공에 닿았고
           && _ballController.BallIsFree() // 공이 자유롭다면
           && Time.time - shootTime > shootTakeDelay // 잡기 쿨탐
           && IsLookObject(collision.transform) // 공 보고 있음??
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

    [SerializeField] private float allowBallAngle = 60f; // 앞쪽 허용 각도 (60도)
    public bool IsLookObject(Transform targetTrm) {
        // 플레이어 위치와 방향
        Vector3 playerForward = transform.forward;
        Vector3 directionToTarget = (targetTrm.position - transform.position).normalized;
        float dot = Vector3.Dot(playerForward, directionToTarget);
        
        return dot >= Mathf.Cos(allowBallAngle * Mathf.Deg2Rad);
    }

    public void BlindSkill(float skillTime)
    {
        BlindEvent?.Invoke(skillTime);
    }
}
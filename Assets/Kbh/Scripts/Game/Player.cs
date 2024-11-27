using DG.Tweening;
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
    [SerializeField] private ItemSO[] _items;
    private int _currentItemCnt = 0;

    [Tooltip("아이템 미리보기를 위한 이미지들")]
    [SerializeField] private Sprite[] _itemImages;

    [SerializeField] private bool isKnockback = false;
    [SerializeField] private float shootTakeDelay = 0.1f; // 슈팅 한 후 공을 가져갈 수 있는 쿨탐
    [SerializeField] private AnimationCurve ballRotateCurve; // 조금 움직였는데 너무 안움직이는 공 떄문에 함 ㅅㄱ

    TagHandle _ballTag, _itemTag;
    private bool HasBall() => _ballController.isRegisted;

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

    // 아이템 이벤트
    public event Action<int, ItemSO> ItemChangedEvent;

    private bool _isMeditation = false;
    public bool IsMeditation
    {
        get => _isMeditation;
        set { _isMeditation = value; }
    }

    [SerializeField] private SoundSO _shootingSound;

    private void Awake()
    {
        PlayerStatSO = Instantiate(PlayerStatSO);

        RigidbodyComponent = GetComponent<Rigidbody>();
        _items = new ItemSO[MAX_ITEM_COUNT];

        _ballTag = TagHandle.GetExistingTag("Ball");
        _itemTag = TagHandle.GetExistingTag("Item");
    }

    private void Start()
    {
        PlayerControlSO.ItemChangeEvent += HandleItemChange;
        PlayerControlSO.ItemUseEvent += HandleItemUse;
        PlayerControlSO.InteractEvent += HandleInterect;
        PlayerControlSO.SkillEvent += HandleSkillUse;
    }

    private void FixedUpdate()
    {
        if (_isMeditation) return;

        if (isKnockback)
        {
            // 다른 플레이어로부터 타격을 받았을 경우
        }
        else
        {
            Movement();
        }
    }

    public float GetNowSpeed()
    {
        Vector2 inputDir = PlayerControlSO.GetMoveDirection();
        int speedValue = HasBall() ? PlayerStatSO.dribbleSpeed.GetValue() : PlayerStatSO.defaultSpeed.GetValue();
        return speedValue * ballRotateCurve.Evaluate(inputDir.magnitude);
    }

    private void Movement()
    {
        Vector2 inputDir = -PlayerControlSO.GetMoveDirection(); // 카메라가 반대라서 인풋도 반대임 ㅋㅋ
        if (inputDir.x == 0 && inputDir.y == 0)
        {   
            if (HasBall()) // 공 안굴러 감
                _ballController.SetBallRotate(Vector3.zero, 0);

            RigidbodyComponent.linearVelocity = Vector3.zero;
            return;
        }

        // inputDir.Normalize();
        Vector3 moveDir = new(inputDir.x, 0, inputDir.y);

        if (HasBall())
        {
            Vector3 rotateDir = new Vector3(moveDir.z, 0, -moveDir.x);
            _ballController.SetBallRotate(rotateDir.normalized, GetNowSpeed() * 20);
        }

        int speedValue = HasBall() ? PlayerStatSO.dribbleSpeed.GetValue() : PlayerStatSO.defaultSpeed.GetValue();
        RigidbodyComponent.linearVelocity = moveDir * (speedValue * 60f * Time.fixedDeltaTime);
        //transform.localPosition += moveDir * (speedValue * Time.fixedDeltaTime);

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
            if (isPerformed && !_isMeditation)
            {
                TryInterect();
                shootWait = StartCoroutine(ShootWaitRoutine());
            }
            else 
            {
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

    private void ShootStop(bool callEvent = true)
    {
        if (shootWait == null) return;
        StopCoroutine(shootWait);

        _prevShootKeyDown = false;

        if (callEvent)
            ShootingEndEvent?.Invoke();
    }

    private IEnumerator ShootWaitRoutine()
    {
        float elapseTime = 0f;
        
        while (true)
        {
            elapseTime += Time.deltaTime;

            if (_isMeditation)
            {
                ShootStop();
                break;
            }
            
            if (elapseTime >= 5)
            {
                Shooting();
                break;
            }
            yield return null;
        }
    }

    private void HandleItemUse()
    {
        if (_currentItemCnt == 0) return;

        var itemInfo = _items[0];
        _items[0] = null;
        ItemChangedEvent?.Invoke(0, null); // 사용함


        //float easeTime = 0.2f;
        //Vector3 currentScale = transform.localScale;
        //var tween = transform.DOScale(itemInfo.appendingScale, easeTime).SetRelative();

        PlayerStatSO.AddModifier(itemInfo.statType, itemInfo.value);

        DOVirtual.DelayedCall(itemInfo.lastTime, () =>
        {
            //if (tween is not null && tween.IsActive())
            //tween.Kill();

            PlayerStatSO.RemoveModifier(itemInfo.statType, itemInfo.value);        
            //transform.DOScale(currentScale, easeTime);
        });

        HandleItemChange(); // 아이템 위치를 밀어준다.
        --_currentItemCnt;
    }

    private void HandleItemChange()
    {
        if (_currentItemCnt == 2)
        {
            (_items[0], _items[1]) = (_items[1], _items[0]);
            ItemChangedEvent?.Invoke(0, _items[0]);
            ItemChangedEvent?.Invoke(1, _items[1]);
        }
    }

    private void HandleSkillUse()
    {
        if (_currentSkill.SkillUseAbleCheck())
            _currentSkill.UseSkill();
    }

    // 강제적으로 공 가져옴
    public void ForceTakeBall()
    {
        this.Registe(_ballController);
    }
    // 강제적으로 공 뺏음 ㅅㄱ
    public void ForceReleseBall()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_itemTag))
        {
            if (_currentItemCnt >= MAX_ITEM_COUNT)
                return;

            ++_currentItemCnt;

            var item = other.GetComponent<Kbh_Item>();
            var itemInfo = item.GetItemInfo();

            _items[_currentItemCnt - 1] = itemInfo;
            ItemChangedEvent?.Invoke(_currentItemCnt - 1, itemInfo);
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
        if (_currentSkill != null)
            return;

        _currentSkill = Instantiate(SkillManager.Instance.GetSkill(PlayerStatSO.skillData.skillType), transform);
        _currentSkill.Init(this);
    }

    [SerializeField] private float allowBallAngle = 60f; // 앞쪽 허용 각도 (60도)
    public bool IsLookObject(Transform targetTrm)
    {
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

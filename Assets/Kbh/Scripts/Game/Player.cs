using DG.Tweening;
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
    [SerializeField] private ItemSO[] _items;
    private int _currentItemCnt = 0;

    [Tooltip("아이템 미리보기를 위한 이미지들")]
    [SerializeField] private Sprite[] _itemImages;

    [SerializeField] private bool isKnockback = false;

    TagHandle _ballTag, _itemTag;
    private bool HasBall() => _ballController.isRegisted;

    private bool _prevShootKeyDown = false;
    private float _prevShootKeyDownTime = 0.0f;

    private SkillBase _currentSkill;
    public SkillBase CurrentSkill => _currentSkill;
    public event Action ShootingStartEvent;
    public event Action ShootingEndEvent;

    private void Awake()
    {
        PlayerStatSO = Instantiate(PlayerStatSO);
        SkillInit();

        RigidbodyComponent = GetComponent<Rigidbody>();
        _items = new ItemSO[MAX_ITEM_COUNT];

        _ballTag = TagHandle.GetExistingTag("Ball");
        _itemTag = TagHandle.GetExistingTag("Item");
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
                DOVirtual.DelayedCall(5f, Shooting);
            }
            else
                Shooting();
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

        ShootingEndEvent?.Invoke();
    }


    private void HandleItemUse()
    {
       if(_currentItemCnt == 0) return;

       var itemInfo = _items[0];
       _items[0] = null;


       float easeTime = 0.2f;
       Vector2 currentScale = transform.localScale;
       var tween = transform.DOScale(itemInfo.appendingScale, easeTime).SetRelative();

       PlayerStatSO.AddModifier(itemInfo.statType, itemInfo.value);
       
       DOVirtual.DelayedCall(itemInfo.lastTime, () =>
       {
           if (tween is not null && tween.IsActive())
               tween.Kill();
       
           PlayerStatSO.RemoveModifier(itemInfo.statType, itemInfo.value);
           transform.DOScale(currentScale, easeTime);
       });

       HandleItemChange(); // 아이템 위치를 밀어준다.
       --_currentItemCnt;
    }

    private void HandleItemChange()
    {
       if(_currentItemCnt == 2)
          (_items[0], _items[1]) = (_items[1], _items[0]);
    }
    
    private void HandleSkillUse()
    {
        if (_currentSkill.SkillUseAbleCheck())
            _currentSkill.UseSkill();
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(_ballTag) // 공에 닿았고
           && _ballController.BallIsFree()) // 공이 자유롭다면
        {
            this.Registe(_ballController);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_itemTag))
        {
           if(_currentItemCnt >= MAX_ITEM_COUNT)
               return;

            ++_currentItemCnt;

            var item = other.GetComponent<Kbh_Item>();
            var itemInfo = item.GetItemInfo();  

            _items[_currentItemCnt - 1] = itemInfo;
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


using System;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("Requires")]
   [field:SerializeField] public PlayerStatsSO PlayerStatSO { get; private set; }
   [field:SerializeField] public PlayerControlSO PlayerControlSO { get; private set; }
   [field:SerializeField] public Rigidbody RigidbodyComponent { get; private set; }

   [Header("Bundles")]
   [SerializeField] private BallControlBundle _ballController;


   private const int MAX_ITEM_COUNT = 2;
   [SerializeField] private int[] _itemIDs;

   [Tooltip("아이템 미리보기를 위한 이미지들")] 
   [SerializeField] private Sprite[] _itemImages;

   [SerializeField] private bool isKnockback = false;

   TagHandle _ballTag;
   private bool HasBall() => _ballController.isRegisted;


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
      Vector2 inputDir = PlayerControlSO.GetMoveDirection();
      if (inputDir.x == 0 && inputDir.y == 0) return;

      inputDir.Normalize();
      Vector3 moveDir = new ( inputDir.x, 0, inputDir.y);



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
   }


   private void HandleInterect(bool isPerformed)
   {
      if(isPerformed)
      {
         if (HasBall())
            Shooting();
         else
            TryInterect();
      }
   }

   private void TryInterect()
   {
      
   }

   private void Shooting()
   {
        Debug.Log(transform.forward);
      _ballController.PushBall(transform.forward * PlayerStatSO.shootPower.GetValue());
      // this.Release(_ballController);
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

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.collider.CompareTag(_ballTag) // 공에 닿았고
         && _ballController.BallIsFree()) // 공이 자유롭다면
      {
         this.Registe(_ballController);
      }
      
   }

   public void SetControl(PlayerControlSO control) {
      PlayerControlSO = control;
   }

   public void SetStat(PlayerStatsSO stat) {
      PlayerStatSO = stat;
   }
}
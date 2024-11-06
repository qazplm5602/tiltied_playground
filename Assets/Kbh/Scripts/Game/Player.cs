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

   [Tooltip("������ �̸����⸦ ���� �̹�����")] 
   [SerializeField] private Sprite[] _itemImages;

   [SerializeField] private bool isKnockback = false;

   TagHandle _ballTag;
   private bool HasBall() => _ballController.isRegisted;


   private void Awake()
   {
      PlayerStatSO = Instantiate(PlayerStatSO);

      PlayerControlSO.ItemChangeEvent += HandleItemChange;
      PlayerControlSO.ItemUseEvent += HandleItemUse;
      PlayerControlSO.InteractEvent += HandleInterect;

      RigidbodyComponent = GetComponent<Rigidbody>();
      _itemIDs = new int[MAX_ITEM_COUNT];
      _ballTag = TagHandle.GetExistingTag("Ball");
   }


   private void FixedUpdate()
   {
      if(isKnockback)
      {
         // �ٸ� �÷��̾�κ��� Ÿ���� �޾��� ���
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

      RigidbodyComponent.linearVelocity
         = moveDir * PlayerStatSO.defaultSpeed.GetValue();
      
      transform.localRotation
         = Quaternion.Lerp(transform.localRotation, 
         Quaternion.Euler(0, -Mathf.Atan2(moveDir.z, moveDir.x) * Mathf.Rad2Deg, 0), 0.1f);
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
      _ballController.PushBall(transform.right * PlayerStatSO.shootPower.GetValue());
      this.Release(_ballController);
   }

   private void HandleItemUse()
   {
      // itemIDs[0]�� ����ϰ�
      _itemIDs[0] = 0; // ����. 
   }

   private void HandleItemChange()
   {
      (_itemIDs[0], _itemIDs[1]) = (_itemIDs[1], _itemIDs[0]);
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.collider.CompareTag(_ballTag) // ���� ��Ұ�
         && _ballController.BallIsFree()) // ���� �����Ӵٸ�
      {
         this.Registe(_ballController);
      }
      
   }


}





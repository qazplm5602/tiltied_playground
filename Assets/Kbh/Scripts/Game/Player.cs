using System;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("Requires")]
   [field:SerializeField] public PlayerStats PlayerStatSO { get; private set; }
   [field:SerializeField] public PlayerControlSO PlayerControlSO { get; private set; }

   [Header("Bundles")]
   [SerializeField] private GetItemBundle _getItemBundle;

   private bool hasBall;
   private const int MAX_ITEM_COUNT = 2;
   [SerializeField] private int[] itemIDs;

   [Tooltip("������ �̸����⸦ ���� �̹�����")] 
   [SerializeField] private Sprite[] itemImages;

   private void Awake()
   {
      this.Registe(_getItemBundle);
      PlayerControlSO.ItemChangeEvent += HandleItemChange;
      PlayerControlSO.ItemUseEvent += HandleItemUse;
      PlayerControlSO.InteractEvent += HandleInterect;

      itemIDs = new int[MAX_ITEM_COUNT];
   }

   private void OnDestroy()
   {
      this.Release(_getItemBundle);
      PlayerControlSO.ItemChangeEvent -= HandleItemChange;
      PlayerControlSO.ItemUseEvent -= HandleItemUse;
      PlayerControlSO.InteractEvent -= HandleInterect;
   }


   private void HandleInterect(bool isPerformed)
   {
      this.Update(_getItemBundle);
   }

   private void HandleItemUse()
   {
      // itemIDs[0]�� ����ϰ�
      itemIDs[0] = 0; // ����. 
   }

   private void HandleItemChange()
   {
      (itemIDs[0], itemIDs[1]) = (itemIDs[1], itemIDs[0]);
   }
}

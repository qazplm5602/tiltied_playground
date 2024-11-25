using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private int itemIdx;
    [SerializeField] private BallAreaType team;

    private RawImage image;
    private Player myPlayer;

    private void Awake() {
        image = GetComponent<RawImage>();
    }

    private void Start() {
        myPlayer = ManagerManager.GetManager<PlayerManager>().GetPlayer(team);
        myPlayer.ItemChangedEvent += HandleItemChange;
    }
    private void OnDestroy() {
        myPlayer.ItemChangedEvent -= HandleItemChange;
    }

    private void HandleItemChange(int idx, ItemSO item)
    {
        if (itemIdx != idx) return;

        image.enabled = item != null;
        if (item)
            image.texture = item.texture;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadUI : MonoBehaviour
{
    [SerializeField] private GamePadSystem gamePadSys;
    [SerializeField] private bool requireOpen = true; // 패드 연결하거나 끊어지면 다시 재설정 해야하니까 자동으로 창 킴???
    [SerializeField] private Transform controlListTrm;
    [SerializeField] private GamePadBoxUI controlBoxPrefab;

    private List<GamePadBoxUI> controls;
    private bool isOpen = false;

    private void Awake() {
        controls = new();
        gamePadSys.OnAddDevice += HandleDeviceAdd;
        gamePadSys.OnRemoveDevice += HandleDeviceRemove;
    }

    private void Start() {
        ///////////////////// TEST
        Open(); // 이거 테스트
        ///////////////////// TEST END
    }

    private void OnDestroy() {
        gamePadSys.OnAddDevice -= HandleDeviceAdd;
        gamePadSys.OnRemoveDevice -= HandleDeviceRemove;
    }

    public void Open() {
        isOpen = true;

        foreach (var item in controls)
            Destroy(item.gameObject);

        foreach (var item in Gamepad.all)
            CreateDeviceBox(item);
    }

    public void Close() {
        isOpen = false;
    }

    private void CreateDeviceBox(InputDevice device) {
        GamePadBoxUI box = Instantiate(controlBoxPrefab, controlListTrm);
        box.Init(device);

        // PlayerControlSO control = 
        
        controls.Add(box);
    }

    private void HandleDeviceAdd(InputDevice device) {
        
    }

    private void HandleDeviceRemove(InputDevice device, PlayerControlSO control) {

    }
}

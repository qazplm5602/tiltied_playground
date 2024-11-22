using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Control/GamePadSystemSO")]
public class GamePadSystem : ScriptableObject
{
    private static Dictionary<PlayerControlSO, InputDevice> devices = new();
    public event System.Action<InputDevice> OnAddDevice;
    public event System.Action<InputDevice, PlayerControlSO> OnRemoveDevice;

    public static InputDevice GetDeviceByControl(PlayerControlSO control) {
        devices.TryGetValue(control, out var data);
        return data;
    }

    public PlayerControlSO GetControlByDevice(InputDevice device) {
        foreach (var item in devices)
            if (item.Value == device)
                return item.Key;

        return null;
    }

    public void SetDeviceControl(PlayerControlSO control, InputDevice device) {
        devices[control] = device;
        // 컨트롤러 한테 디바이스 적용 해야함
    }

    public void ClearAll() {
        foreach (var item in devices)
        {
            // 모든 contorller 한테 초기화 해야함
        }

        devices.Clear();
    }

    private void OnEnable() {
        InputSystem.onDeviceChange += HandleDeviceChange;
    }

    private void OnDisable() {
        InputSystem.onDeviceChange -= HandleDeviceChange;
    }

    private void HandleDeviceChange(InputDevice device, InputDeviceChange change) {
        if (device is not Gamepad) return; // 게임패드 아니면 안됨 ㅅㄱ
            
        switch (change)
        {
            case InputDeviceChange.Added:
                OnAddDevice?.Invoke(device);
                break;
            case InputDeviceChange.Removed:
                OnRemoveDevice?.Invoke(device, GetControlByDevice(device));
                break;
        }
    }
}

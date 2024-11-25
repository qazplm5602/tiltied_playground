using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Control/GamePadSystemSO")]
public class GamePadSystem : ScriptableObject
{
    private static Dictionary<PlayerControlSO, InputDevice> devices = new();
    public static bool UseGamepad => devices.Count > 0;
    public static event System.Action<bool> OnChangeGamepad;
    public event System.Action<InputDevice> OnAddDevice;
    public event System.Action<InputDevice, PlayerControlSO> OnRemoveDevice;

    public static InputDevice GetDeviceByControl(PlayerControlSO control) {
        devices.TryGetValue(control, out var data);
        return data;
    }

    public static void RequestSetDevice(PlayerControlSO control) {
        List<InputDevice> results = new() { Keyboard.current }; // 기본값 키보드
        InputDevice device = GetDeviceByControl(control);

        if (device != null)
            results.Add(device);

        control.SetDevices(results);    
    }

    public PlayerControlSO GetControlByDevice(InputDevice device) {
        foreach (var item in devices)
            if (item.Value == device)
                return item.Key;

        return null;
    }

    public void RemoveControlDevice(PlayerControlSO control) {
        devices.Remove(control);
        RequestSetDevice(control);
    }

    public void SetDeviceControl(PlayerControlSO control, InputDevice device) {
        bool lastUsed = UseGamepad; // 이 패드 설정 전에도 이미 사용중이엿나??
        devices[control] = device;

        if (!lastUsed) { // 이벤트
            OnChangeGamepad?.Invoke(true);
        }
        
        // 컨트롤러 한테 디바이스 적용 해야함
        // control.SetDevices(new () { Keyboard.current, device }); // 그냥 키보드도 되게 함 ㅁㄴㅇㄹ
        RequestSetDevice(control);
    }

    public void ClearAll() {
        bool lastUsed = UseGamepad;
        foreach (var item in devices)
        {
            // 모든 contorller 한테 초기화 해야함
            item.Key.SetDevices(new() { Keyboard.current }); // 키보드만 ㄱㄴ
        }

        devices.Clear();

        // if (lastUsed)
            OnChangeGamepad?.Invoke(false);
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

using UnityEngine;

public abstract class DeviceChangeListener : MonoBehaviour
{
    protected virtual void OnEnable() {
        GamePadSystem.OnChangeGamepad += OnChangeGamepad;
        
        // 바로 켜지자 마자 ㄱㄱ
        if (GamePadSystem.UseGamepad) {
            OnChangeGamepad(true);
        }
    }

    protected virtual void OnDisable() {
        GamePadSystem.OnChangeGamepad -= OnChangeGamepad;
    }

    protected abstract void OnChangeGamepad(bool active);
}

using UnityEngine;

public abstract class DeviceChangeListener : MonoBehaviour
{
    protected virtual void OnEnable() {
        GamePadSystem.OnChangeGamepad += OnChangeGamepad;
    }

    protected virtual void OnDisable() {
        GamePadSystem.OnChangeGamepad -= OnChangeGamepad;
    }

    protected abstract void OnChangeGamepad(bool active);
}

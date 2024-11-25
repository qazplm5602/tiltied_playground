using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFocusCancel : MonoBehaviour
{
    private Button[] registerBtns;
    
    private void Awake() {
        registerBtns = GetComponentsInChildren<Button>();
        
        foreach (var item in registerBtns)
            item.onClick.AddListener(HandleBtnClick);
    }

    private void OnDestroy() {
        foreach (var item in registerBtns)
            item.onClick.RemoveListener(HandleBtnClick);
    }

    private void HandleBtnClick() {
        if (EventSystem.current.currentSelectedGameObject != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}

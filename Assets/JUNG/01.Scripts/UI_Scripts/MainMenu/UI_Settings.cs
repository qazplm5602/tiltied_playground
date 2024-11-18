using UnityEngine;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private PlayerControlSO _inputSO1;

    private void Start()
    {
    }
    private void OnEnable()
    {
        _inputSO1.CloseUIEvent += HandleCloseUIEvent;
    }

    private void HandleCloseUIEvent()
    {
        UI_Manager.Instance.UIOpenOrClose(transform.parent.gameObject, false, transform.parent.gameObject);
    }

    private void OnDisable()
    {
        _inputSO1.CloseUIEvent -= HandleCloseUIEvent;
    }

}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

enum ControlType : byte {
    Player1 = 0,
    Player2
}

[CreateAssetMenu(fileName = "PlayerControlSO", menuName = "SO/Control/PlayerControlSO")]
public class PlayerControlSO : ScriptableObject, Controls.IPlayerActions
{
    [SerializeField] ControlType playerType;
    public event Action ItemUseEvent;
    public event Action SkillEvent;
    
    private Controls controls = null;

    private void OnEnable() {
        controls = new();
        controls.Player.SetCallbacks(this);
        LoadKeyBind();

        controls.Player.Enable(); // 켜ㅓㅓ
    }
    
    private void LoadKeyBind() {
        // 아직 저장기능이 구현 안되어있음
        // 저장된 바인딩 데이터가 있으면 그걸로 적용함
        // ... code
        
        // defualt 키 지정
        Debug.Log($"Load Key Bind... Control/base_{playerType.ToString()}");
        TextAsset data = Resources.Load<TextAsset>($"Control/base_{playerType.ToString()}");
        if (data == null) return; // 없는디??

        Debug.Log("Loaded Key Bind!");
        controls.LoadBindingOverridesFromJson(data.ToString());
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {

    }

    public void OnItemChange(InputAction.CallbackContext context)
    {

    }

    public void OnItemUse(InputAction.CallbackContext context)
    {
        if (context.performed)
            ItemUseEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
            SkillEvent?.Invoke();
    }
}

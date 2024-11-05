using System;
using UnityEngine;
using UnityEngine.InputSystem;

enum ControlType : byte
{
    Player1 = 0,
    Player2
}

[CreateAssetMenu(fileName = "PlayerControlSO", menuName = "SO/Control/PlayerControlSO")]
public class PlayerControlSO : ScriptableObject, Controls.IPlayerActions
{
    [SerializeField] ControlType playerType;
    public event Action ItemUseEvent;
    public event Action ItemChangeEvent;
    public event Action SkillEvent;
    public event Action MoveEvent;
    public event Action<bool> InteractEvent;

    private Controls controls = null;

    private void OnEnable()
    {
        controls = new();
        controls.Player.SetCallbacks(this);
        LoadKeyBind();

        controls.Player.Enable(); // 켜ㅓㅓ
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void LoadKeyBind()
    {
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

    public Vector2 GetMoveDirection()
    {
        return controls.Player.Move.ReadValue<Vector2>();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
            InteractEvent?.Invoke(context.performed);
    }

    public void OnItemChange(InputAction.CallbackContext context)
    {
        ItemChangeEvent?.Invoke();
    }

    public void OnItemUse(InputAction.CallbackContext context)
    {
        if (context.performed)
            ItemUseEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            ItemUseEvent?.Invoke();
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
            SkillEvent?.Invoke();
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamePadBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlName;
    [SerializeField] private Button findBtn;
    [SerializeField] private Image teamBorder;
    [SerializeField] private Image teamBox;
    [SerializeField] private TextMeshProUGUI teamText;

    public BallAreaType? SelectTeam { get; private set; }
    private Gamepad myDevice;
    public event System.Action OnChangeTeam;

    public void Init(InputDevice control) {
        myDevice = (Gamepad)control;
        controlName.text = control.displayName;
        findBtn.onClick.AddListener(HandleFind);
    }

    private void Update() {
        // xbox 컨트롤러로 X(트위터 아님) 임 
        if (myDevice.buttonWest.wasPressedThisFrame) {
            SetTeam(BallAreaType.Blue);
        }

        // B
        if (myDevice.buttonEast.wasPressedThisFrame) {
            SetTeam(BallAreaType.Red);
        }
    }

    public void SetTeam(BallAreaType? team) {
        if (SelectTeam != null && team != null && team.Value == SelectTeam.Value)
            team = null; // 똑같은 팀 고르면 해제

        // 컬러 정해야징
        string teamT = team.ToString();
        Color color = default;
        
        switch (team)
        {
            case BallAreaType.Blue:
                color = new Color32(76,201,254,255);
                break;
            case BallAreaType.Red:
                color = new Color32(249,84,84,255);
                break;
            case null:
                color = new Color32(132,132,132,255);
                teamT = "팀 없음";
                break;
        }

        teamBorder.color = teamBox.color = color;
        teamText.text = teamT;
        SelectTeam = team;

        OnChangeTeam?.Invoke();
    }

    public InputDevice GetDevice() => myDevice;

    private bool runMoter = false;
    private void HandleFind() {
        if (runMoter) return;

        runMoter = true;
        myDevice.SetMotorSpeeds(0.25f, 0.75f);

        DOVirtual.DelayedCall(2f, () => {
            runMoter = false;
            myDevice.ResetHaptics();
        });
    }
}

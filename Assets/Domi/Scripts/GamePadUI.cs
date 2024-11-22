using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadUI : MonoBehaviour, Controls.IGamePadSetUIActions
{
    [SerializeField] private GamePadSystem gamePadSys;
    [SerializeField] private bool requireOpen = true; // 패드 연결하거나 끊어지면 다시 재설정 해야하니까 자동으로 창 킴???
    [SerializeField] private Transform controlListTrm;
    [SerializeField] private GamePadBoxUI controlBoxPrefab;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TeamControlsSO teamControls;
    [SerializeField] private float animDuration = 0.3f;

    private List<GamePadBoxUI> controls;
    private Controls globalControls;
    private bool isOpen = false;
    private bool allowPlay = false;

    private Canvas mainScreen;
    private RectTransform panel;
    private CanvasGroup panelGroup;
    
    private Sequence animSequence;

    private void Awake() {
        controls = new();
        gamePadSys.OnAddDevice += HandleDeviceAdd;
        gamePadSys.OnRemoveDevice += HandleDeviceRemove;

        // 뉴인풋
        globalControls = new();
        globalControls.GamePadSetUI.SetCallbacks(this);

        mainScreen = GetComponentInParent<Canvas>();
        panel = transform.GetChild(0) as RectTransform;
        panelGroup = panel.GetComponent<CanvasGroup>();
    }

    private void Start() {
        ///////////////////// TEST
        // Open(); // 이거 테스트
        ///////////////////// TEST END
    }

    private void OnDestroy() {
        gamePadSys.OnAddDevice -= HandleDeviceAdd;
        gamePadSys.OnRemoveDevice -= HandleDeviceRemove;

        globalControls.GamePadSetUI.Disable();
    }

    public void Open() {
        isOpen = true;
        globalControls.GamePadSetUI.Enable();

        foreach (var item in Gamepad.all)
            CreateDeviceBox(item);

        CheckPlayAllow();

        // open 애니메이션
        animSequence?.Kill(true);
        mainScreen.sortingOrder = 1; // 우선순위

        panelGroup.alpha = 0;
        panelGroup.blocksRaycasts = true;
        panelGroup.interactable = true;

        panel.localScale = Vector3.one * 0.9f;

        animSequence = DOTween.Sequence();
        animSequence.Append(panelGroup.DOFade(1, animDuration));
        animSequence.Join(panel.DOScale(Vector3.one, animDuration));
    }

    public void Close() {
        isOpen = false;
        globalControls.GamePadSetUI.Disable();

        foreach (var item in controls)
            Destroy(item.gameObject);

        controls.Clear();

        // 닫는거 애니메이션
        animSequence?.Kill(true);

        panelGroup.blocksRaycasts = false;
        panelGroup.interactable = false;

        animSequence = DOTween.Sequence();
        animSequence.Append(panelGroup.DOFade(0, animDuration));
        animSequence.Join(panel.DOScale(Vector3.one *0.9f, animDuration));
        
        animSequence.OnComplete(() => {
            mainScreen.sortingOrder = -1;
        });
    }

    private void CreateDeviceBox(InputDevice device) {
        GamePadBoxUI box = Instantiate(controlBoxPrefab, controlListTrm);
        box.Init(device);

        box.OnChangeTeam += CheckPlayAllow; // 팀 바뀌면 체크 ㄱㄱ

        PlayerControlSO control = gamePadSys.GetControlByDevice(device);
        if (control) { // 어 머야 팀이 있네
            BallAreaType? team = teamControls.GetTeamByControl(control);
            if (team == null) return; // 뭐지 사기치는건가
            
            box.SetTeam(team.Value);
        }
        
        controls.Add(box);
    }

    private void HandleDeviceAdd(InputDevice device) {
        CheckPlayAllow();

        if (isOpen) { // 그냥 ui 업뎃
            CreateDeviceBox(device);
            return;
        }

        if (!requireOpen) return; // 자동으로 열 필요 없는뎅

        // 플레이 못하는 상태
        if (!allowPlay)
            Open(); // 설정해라.. ㅅㄱ
    }

    private void HandleDeviceRemove(InputDevice device, PlayerControlSO control) {
        if (control)
            gamePadSys.RemoveControlDevice(control);

        if (isOpen) {
            CheckPlayAllow();
            GamePadBoxUI box = controls.Find(v => v.GetDevice() == device);
            if (box == null) return; // 엥??
            
            controls.Remove(box);
            Destroy(box.gameObject);
            return;
        }

        if (!requireOpen) return; // 헤헤
        Open(); // 설정해라

        // 패드 다 없으면 키보드로 바꿈
    }

    private void CheckPlayAllow() {
        allowPlay = false;

        if (controls.Count < 2) {
            errorText.text = "컨트롤러가 2개 이상 이어야 합니다.";
            return;
        }

        bool red = false, blue = false;
        // 똥 코ㄷㅡ (나중에 바꿀꺼)
        foreach (var item in controls)
        {
            if (item.SelectTeam == BallAreaType.Red) {
                if (red) {
                    errorText.text = "Red팀 선택이 중복 되었습니다.";
                    return;
                }
                
                red = true;
            } else if (item.SelectTeam == BallAreaType.Blue) {
                if (blue) {
                    errorText.text = "Blue팀 선택이 중복 되었습니다.";
                    return;
                }
                
                blue = true;
            }
        }

        // 팀 선택 안되어있는거 체크
        if (!red || !blue) {
            StringBuilder builder = new();
            
            if (!red) {
                builder.Append("Red");
            }
            if (!blue) {
                if (!red)
                    builder.Append(", ");
                builder.Append("Blue");
            }

            builder.Append("팀이 선택되어 있지 않습니다.");

            errorText.text = builder.ToString();
            return;
        }

        errorText.text = string.Empty;
        allowPlay = true;
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (!context.performed || !allowPlay) return; // 플레이 못함 ㅅㄱ

        gamePadSys.ClearAll(); // 전부 초기화
        controls.ForEach(v => {
            if (v.SelectTeam == null) return;

            PlayerControlSO playerControl = teamControls.GetControlByTeam(v.SelectTeam.Value);
            if (playerControl == null) {
                Debug.LogError($"Not Found Player Control {v.SelectTeam.Value} team."); // 뭐해 ㅡㅡ;
                return;
            }

            gamePadSys.SetDeviceControl(playerControl, v.GetDevice());
        });

        Close();
        print("OnStart");
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        gamePadSys.ClearAll();
        Close();
        print("OnSkip");
    }
}

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestDomi : MonoBehaviour
{
    [SerializeField] PlayerControlSO controls;
    [SerializeField] TestSoccerBallKick ball;

    // private void Awake() {
    //     controls.ItemUseEvent += OnItemUse;
    //     controls.SkillEvent += OnSkill;
    //     controls.InteractEvent += OnInteract;
    // }

    // private void OnItemUse() {
    //     print("OnItemUse!");
    // }

    // private void OnSkill() {
    //     print("OnSkill!");
    // }

    // private void OnInteract(bool isDown) {
    //     print($"OnInteract {isDown}");
    // }

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     // controls.StartBindChange();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     Vector2 pos = controls.GetMoveDirection();
    //     // print(pos);
    // }

    CameraManager cameraManager;

    private void Awake() {
        BallGoalSimulateManager simulateSys = ManagerManager.GetManager<BallGoalSimulateManager>();
        simulateSys.onWillGoal += HandleWillGoal;

        cameraManager = ManagerManager.GetManager<CameraManager>();
    }

    private void Update() {
        if (Keyboard.current.gKey.wasPressedThisFrame)
            ball.KickBall();
    }

    private void OnDestroy() {
        BallGoalSimulateManager simulateSys = ManagerManager.GetManager<BallGoalSimulateManager>();
        simulateSys.onWillGoal -= HandleWillGoal;
    }

    private void HandleWillGoal(BallAreaType type) {
        if (type != BallAreaType.Blue && type != BallAreaType.Red) return;
        
        Time.timeScale = 0.1f; // 시간 느리게
        List<CameraType> cameras = cameraManager.GetNearCam(CameraManager.NearType.Near, new CameraType[] { type == BallAreaType.Blue ? CameraType.Blue_L : CameraType.Orange_L, type == BallAreaType.Blue ? CameraType.Blue_R : CameraType.Orange_R }, ball.transform.position);

        // 가까운거는
        CameraType nearCam = cameras[0];
        cameraManager.Transition.FadeChangeCam(nearCam);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}

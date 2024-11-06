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
     BallGoalSimulateManager simulateSys;

    private void Awake() {
        simulateSys = ManagerManager.GetManager<BallGoalSimulateManager>();
        simulateSys.onWillGoal += HandleWillGoal;

        cameraManager = ManagerManager.GetManager<CameraManager>();
    }

    private void Update() {
        if (Keyboard.current.gKey.wasPressedThisFrame)
            ball.KickBall();
    }

    private void OnDestroy() {
        simulateSys.onWillGoal -= HandleWillGoal;
    }

    private void HandleWillGoal(BallAreaType type) {

    }
}

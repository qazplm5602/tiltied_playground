using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class ResultCamera : MonoBehaviour
{
    [SerializeField] private Vector3 camMargin;
    [SerializeField] private float maxY;
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField] private float endDuration = 2f;
    [SerializeField] private AnimationCurve animEase;

    private void Start() {
        // TEST
        Player player = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);
        
        // StartCam(player.transform);
        // StartCoroutine(WaitRun(player.transform));
    }

    IEnumerator WaitRun(Transform hit) {
        yield return new WaitForSeconds(2);
        StartCam(hit);
    }

    public void StartCam(Transform hit) {
        Vector3 dir = hit.TransformDirection(camMargin);
        Vector3 startCamPos = hit.position + dir;

        Transform hitTrm = new GameObject("ResultHitPoint").transform;
        hitTrm.SetParent(hit);

        hitTrm.position = hit.position + hit.TransformDirection(new Vector3(camMargin.x, camMargin.y, 0));


        cinemachine.Follow = hitTrm;



        print($"{dir} / {hit.position} / {startCamPos}");

        cinemachine.transform.position = new Vector3(startCamPos.x, startCamPos.y + maxY, startCamPos.z);
        cinemachine.transform.DOMoveY(startCamPos.y, endDuration).SetEase(animEase);
    }

    public void GroundHit() {
        cinemachine.Follow = null;
        cinemachine.transform.position = new Vector3(0, 100f, 0);
        cinemachine.transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
    }

    public float GetDuration() => endDuration;
}

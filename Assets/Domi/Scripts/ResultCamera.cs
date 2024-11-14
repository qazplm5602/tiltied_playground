using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ResultCamera : MonoBehaviour
{
    [SerializeField] private Vector3 camMargin;
    [SerializeField] private CinemachineCamera cinemachine;

    private void Start() {
        // TEST
        Player player = ManagerManager.GetManager<PlayerManager>().GetPlayer(BallAreaType.Red);
        
        // StartCam(player.transform);
        StartCoroutine(WaitRun(player.transform));
    }

    IEnumerator WaitRun(Transform hit) {
        yield return new WaitForSeconds(2);
        StartCam(hit);
    }

    public void StartCam(Transform hit) {
        Vector3 dir = hit.TransformDirection(camMargin);
        Vector3 startCamPos = hit.position + dir;

        Transform hitTrm = new GameObject("ResultHitPoint").transform;
        hitTrm.SetParent(transform.parent);

        cinemachine.Follow = hitTrm;



        print($"{dir} / {hit.position} / {startCamPos}");

        cinemachine.transform.position = startCamPos;
    }
}

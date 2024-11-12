using System;
using UnityEngine;

public class SoccerBallRemoteTrigger : MonoBehaviour
{
    public event Action<Collider> TriggerEnter;

    private void OnTriggerEnter(Collider other) {
        print($"OnTriggerEnter {other}");
        TriggerEnter?.Invoke(other);
    }
}

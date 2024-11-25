using UnityEngine;

public class SoccerBallRotate : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 dir;

    private void Update() {
        if (speed == 0.0f) return;
        transform.Rotate(dir * speed * Time.deltaTime, Space.World);
    }

    public void Run() {
        enabled = true;
    }

    public void Stop() {
        enabled = false;
    }

    public void SetSpeed(float value) {
        speed = value;
    }

    public void SetDirection(Vector3 value) {
        dir = value;
    }
}


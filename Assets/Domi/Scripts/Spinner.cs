using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    private void Update() {
        transform.Rotate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
    }
}

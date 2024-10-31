using System.Collections.Generic;
using UnityEngine;

public class SimulationBallViewer : MonoBehaviour
{
    [SerializeField] private float time = 0;
    [SerializeField] private float maxTime = 20;
    [SerializeField] private float amount = 20;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private GameObject prefab;

    private float lastTime;
    private float lastMaxTime;
    private float lastAmount;
    private Vector3 lastPos;
    private Vector3 lastVelocity;

    private List<GameObject> debugObjects = new();

    private void Update() {
        if (IsDataFreeze()) return; // 데이터가 똑같앙

        DrawDomi();

        lastTime = time;
        lastMaxTime = maxTime;
        lastAmount = amount;
        lastPos = pos;
        lastVelocity = velocity;
    }

    private bool IsDataFreeze() => time == lastTime && lastPos == pos && lastVelocity == velocity && lastMaxTime == maxTime && lastAmount == amount;

    void DrawDomi() {
        Clear();
        
        for (int i = 0; i < amount; i++)
        {
            float nowTime = (maxTime / amount) * i;
            
            GameObject entity = Instantiate(prefab, transform);

            float x = pos.x + velocity.x * nowTime;
            float y = pos.y + velocity.y * nowTime - ((gravity * Mathf.Pow(nowTime, 2)) / 2f);
            entity.transform.position = new Vector3(x, y, pos.z);

            debugObjects.Add(entity);
        }
    }

    void Clear() {
        foreach (var item in debugObjects)
            Destroy(item);
    }
}

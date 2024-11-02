using System;
using System.Collections.Generic;
using UnityEngine;

public enum BallAreaType {
    Blue,
    Red
}

[Serializable]
struct BallArea {
    public BallAreaType type;
    public GoalPostArea area;
}

public class BallGoalSimulateManager : MonoBehaviour
{
    [SerializeField] private int loopAmount = 30; // 많을수록 더 먼곳이어도 감지가 가능함
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private Collider point; // 이걸로 공이 충돌 하는지 체크함
    [SerializeField] private BallArea[] areas;
    
    private Dictionary<BallAreaType, Action> callbacks = new();

    
    public void RegisterCallback(BallAreaType type, Action cb) {
        if (callbacks.TryGetValue(type, out Action value)) {
            callbacks[type] += cb;
        } else callbacks[type] = cb;
    }

    public void UnRegisterCallback(BallAreaType type, Action cb) {
        callbacks[type] -= cb;
        
        if (callbacks[type] == null)
            callbacks.Remove(type);
    }

    private void SendCallback(BallAreaType type) {
        if (callbacks.TryGetValue(type, out Action cb))
            cb?.Invoke();
    }

    public void SimulateBall(Vector3 ballPos, Vector3 kickDir /* 힘도 있음 */) {
        float between = Vector3.Distance(GetPos(ballPos, kickDir, 0), GetPos(ballPos, kickDir, 0.1f));
        
        Vector3 lastPos = GetPos(ballPos, kickDir, 0);
        for (int i = 1; i < loopAmount; i++)
        {
            float nowTime = i / between;
            Vector3 pos = GetPos(ballPos, kickDir, nowTime);

            // 벌래
            Debug.DrawLine(lastPos, pos, Color.red, 10f);

            // 골대에 들어감???
            if (FindGoalPost(pos, out BallAreaType detectArea)) {
                Debug.Log($"Detect GoalPos {detectArea.ToString()}");
                SendCallback(detectArea);
                break;
            }

            // 어디 맞아서 더 이상 안해도 됨
            if (HitObstacle(lastPos, pos))
                break;

            lastPos = pos;
        }
    }

    private Vector3 GetPos(Vector3 ballPos, Vector3 kickDir, float time) {
        float x = ballPos.x + kickDir.x * time;
        float y = ballPos.y + kickDir.y * time - ((gravity * Mathf.Pow(time, 2)) / 2f);
        float z = ballPos.z + kickDir.z * time;
        return new Vector3(x, y, z);
    }
    
    private RaycastHit[] hitResults = new RaycastHit[1];
    private bool HitObstacle(Vector3 start, Vector3 end) {
        Vector3 dir = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        int count = Physics.RaycastNonAlloc(start, dir, hitResults, distance, whatIsObstacle);
        return count > 0;
    }

    private bool FindGoalPost(Vector3 pos, out BallAreaType type) {
        point.enabled = false; // collider 수동 업뎃
        point.transform.position = pos;
        point.enabled = true;

        // point.GetPositionAndRotation(out Vector3 asdf, out var _);
        // Debug.Log($"point coords: {asdf}");
        
        foreach (var value in areas) {
            bool result = value.area.HasPointIn();

            if (result) {
                type = value.type;
                return true;
            }
        }

        type = default;
        return false;
    }
}

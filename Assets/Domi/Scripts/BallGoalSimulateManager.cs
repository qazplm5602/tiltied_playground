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
    [SerializeField] int loopAmount = 30; // 많을수록 더 먼곳이어도 감지가 가능함
    [SerializeField] float gravity;
    [SerializeField] private BallArea[] areas;
    
    private Dictionary<BallAreaType, Action> callbacks;

    
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

    public void SimulateBall(Vector3 ballPos, Vector3 kickDir /* 힘도 있음 */) {
        float between = Vector3.Distance(GetPos(ballPos, kickDir, 0), GetPos(ballPos, kickDir, 0.1f));
        
        Vector3 lastPos = GetPos(ballPos, kickDir, 0);
        for (int i = 1; i < loopAmount; i++)
        {
            float nowTime = i / between;
            print(nowTime);
            Vector3 pos = GetPos(ballPos, kickDir, nowTime);
            
            Debug.DrawLine(lastPos, pos, Color.red, 10f);

            lastPos = pos;
        }
    }

    private Vector3 GetPos(Vector3 ballPos, Vector3 kickDir, float time) {
        float x = ballPos.x + kickDir.x * time;
        float y = ballPos.y + kickDir.y * time - ((gravity * Mathf.Pow(time, 2)) / 2f);
        float z = ballPos.z + kickDir.z * time;
        return new Vector3(x, y, z);
    }
}

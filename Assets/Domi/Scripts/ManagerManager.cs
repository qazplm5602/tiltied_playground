using System.Collections.Generic;
using UnityEngine;

public class ManagerManager : MonoBehaviour
{
    static private ManagerManager instance;
    private Dictionary<System.Type, MonoBehaviour> scripts = new();

    static public T GetManager<T>() where T : MonoBehaviour {
        if (ManagerManager.instance == null) { // 없다.
            ManagerManager.instance = FindAnyObjectByType<ManagerManager>();
            
            if (ManagerManager.instance == null) { // 그냥 안만들었는듯
                GameObject newEntity = new GameObject("ManagerManager");
                ManagerManager.instance = newEntity.AddComponent<ManagerManager>();
            }
        }

        if (ManagerManager.instance.scripts.TryGetValue(typeof(T), out var instance)) {
            return instance as T;
        }

        // 캐싱 안되어있음
        T manager = FindAnyObjectByType<T>();
        if (manager == null) {
            Debug.LogWarning($"{typeof(T)} Not Found Manager.");
            return null;
        }

        ManagerManager.instance.scripts[typeof(T)] = manager;
        return manager;
    }
}

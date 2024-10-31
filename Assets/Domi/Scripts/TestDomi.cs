using UnityEngine;

public class TestDomi : MonoBehaviour
{
    [SerializeField] PlayerControlSO controls;

    private void Awake() {
        controls.ItemUseEvent += OnItemUse;
        controls.SkillEvent += OnSkill;
    }

    private void OnItemUse() {
        print("OnItemUse!");
    }

    private void OnSkill() {
        print("OnSkill!");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // controls.StartBindChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

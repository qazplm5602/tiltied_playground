using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86;
public class UIController : MonoBehaviour
{
    private VisualElement _movingBG;
    private Label _tweenLabel;
    private Button _openSceneBtn;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;


        _movingBG = root.Q<VisualElement>("MovingBG");

        _openSceneBtn = root.Q<Button>("OpenSceneBtn");

    }

    void Update()
    {
        _openSceneBtn.RegisterCallback<ClickEvent>(vt =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }
}

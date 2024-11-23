using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private PlayerControlSO _inputSO1;

    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _bgmSlider;

    private void Start()
    {
        float t = Mathf.InverseLerp(-60f, 0f, SoundManager.Instance.GetAudioValue("SFX"));
        _sfxSlider.value = Mathf.Lerp(0.001f, 1, t);
        
        t = Mathf.InverseLerp(-60f, 0f, SoundManager.Instance.GetAudioValue("BGM"));
        _bgmSlider.value = Mathf.Lerp(0.001f, 1, t);

        _sfxSlider.onValueChanged.AddListener(HandleSfxValueChange);
        _bgmSlider.onValueChanged.AddListener(HandleBGMValueChange);
    }

    private void HandleBGMValueChange(float value)
    {
        SoundManager.Instance.SetAudioValue("BGM", value);
    }

    private void HandleSfxValueChange(float value)
    {
        SoundManager.Instance.SetAudioValue("SFX", value);
    }

    private void OnEnable()
    {
        _inputSO1.CloseUIEvent += HandleCloseUIEvent;
    }

    private void HandleCloseUIEvent()
    {
        UI_Manager.Instance.UIOpenOrClose(transform.parent.gameObject, false, transform.parent.gameObject);
    }

    private void OnDisable()
    {
        _inputSO1.CloseUIEvent -= HandleCloseUIEvent;
    }

}

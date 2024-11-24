using UnityEngine;

public class MenuSoundHelper : MonoBehaviour
{
    [SerializeField] private SoundSO _changeSound;
    [SerializeField] private SoundSO _selectSound;

    public void ChangeSound()
    {
        SoundManager.Instance.PlaySFX(transform.position, _changeSound);
    }

    public void SelectSound()
    {
        SoundManager.Instance.PlaySFX(transform.position, _selectSound);
    }
}

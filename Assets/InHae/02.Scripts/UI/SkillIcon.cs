using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private BallAreaType _areaType;
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _alphaImage;
    [SerializeField] private TextMeshProUGUI _coolTimer;

    private Player _owner;
    private SkillBase _skill;

    private void Start()
    {
        _owner = ManagerManager.GetManager<PlayerManager>().GetPlayer(_areaType);
        _skill = _owner.CurrentSkill;
        
        _skillImage.sprite = _skill.skillData.skillIcon;
        
        _skill.coolChangeEvent += HandleCooltimeChange;
        _skill.coolChangeEvent += HandleTextChange;
    }

    private void OnDestroy()
    {
        _skill.coolChangeEvent -= HandleCooltimeChange;
        _skill.coolChangeEvent -= HandleTextChange;
    }

    private void HandleTextChange(float current, float total)
    {
        if (current <= 0)
        {
            _coolTimer.gameObject.SetActive(false);
            return;
        }
        
        if(!_coolTimer.gameObject.activeInHierarchy)
            _coolTimer.gameObject.SetActive(true);

        _coolTimer.SetText(current.ToString("F0"));
    }

    private void HandleCooltimeChange(float current, float total)
    {
        _alphaImage.fillAmount = current / total;
    }
}

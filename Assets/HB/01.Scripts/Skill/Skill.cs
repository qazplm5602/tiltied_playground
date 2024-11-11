using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Image[] skillImages;
    public float coolTime;

    public virtual void Use()
    {
        Debug.Log("스킬 사용");
    }
}

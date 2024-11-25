using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TypeChar
{
    public bool IsComplete = false;
    public bool IsDisappearComplete = false;

    private Vector3[] _originPostion;
    private float _currentTime = 0;
    private float _delayTime = 0;
    private float _effectTime = 0;
    private float _disappearDelay = 0.5f;
    private int _startIndex;

    private Color _startColor;
    private Color _endColor;
    private Image _image;

    public TypeChar(int start, Vector3[] vertices, Color32[] colors, Color startColor, Color endColor, float delayTime, float effectTime = 0.5f, float offset = 15f)
    {
        _originPostion = new Vector3[4];

        for (int i = 0; i < 4; ++i)
        {
            Vector3 point = vertices[start + i];
            _originPostion[i] = point;


            vertices[start + i] = point + new Vector3(offset + 400f, 0, 0);

            colors[start + i].a = 0;
        }

        _delayTime = delayTime;
        _effectTime = effectTime;
        _startIndex = start;
        _startColor = startColor;
        _endColor = endColor;
    }

    public TypeChar SetImg(Image img)
    {
        _image = img;
        return this;
    }

    public void UpdateChar(Vector3[] vertices, Color32[] colors)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _delayTime || IsComplete) return;

        float time = _currentTime - _delayTime;
        float percent = time / _effectTime;


        for (int i = 0; i < 4; ++i)
        {
            vertices[_startIndex + i] = Vector3.Lerp(vertices[_startIndex + i], _originPostion[i], percent);
            colors[_startIndex + i] = Color.Lerp(_startColor, _endColor, percent);
            colors[_startIndex + i].a = (byte)Mathf.Lerp(0, 255, percent);
        }

        if (percent > 1)
        {
            IsComplete = true;
            _currentTime = 0;
        }
    }

    public void DisappearChar(Vector3[] vertices, Color32[] colors)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _disappearDelay || IsDisappearComplete) return;

        float time = _currentTime - _disappearDelay;
        float percent = time / _effectTime;


        // 왼쪽으로 이동하며 사라지도록 수정
        for (int i = 0; i < 4; ++i)
        {

            vertices[_startIndex + i] = Vector3.Lerp(_originPostion[i], _originPostion[i] - new Vector3(400f, 0, 0), percent);
            colors[_startIndex + i] = Color.Lerp(_endColor, _startColor, percent);
            colors[_startIndex + i].a = (byte)Mathf.Lerp(255, 0, percent); // 투명해짐
        }

        if (percent > 1)
        {
            IsDisappearComplete = true;
            _currentTime = 0;
        }
    }
}

public class UI_EffectText : MonoBehaviour
{
    [SerializeField] private Image _bgImg;
    [SerializeField]
    private float _oneCharacterTime = 0.2f;
    [SerializeField]
    private Color _startColor, _endColor;

    private Image _bgColor1;
    private Image _bgColor2;

    private bool _isType = false;

    private TMP_Text _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
        _bgColor1 = _bgImg.transform.GetChild(0).GetComponent<Image>();
        _bgColor2 = _bgImg.transform.GetChild(1).GetComponent<Image>();
    }

    
    public void StartEffect(string text, Color overrideEndColor)
    {
        _bgColor1.color = overrideEndColor;
        _bgColor2.color = overrideEndColor;
        _endColor = overrideEndColor;
        _tmpText.SetText(text);
        _tmpText.color = _endColor;
        _tmpText.ForceMeshUpdate();

        TypeText();
    }

    private void TypeText()
    {
        _isType = true;

        List<TypeChar> charList = new List<TypeChar>();


        TMP_TextInfo textInfo = _tmpText.textInfo;
        Vector3[] vertices = textInfo.meshInfo[0].vertices;  //매티리얼은 한개라고 가정한다 여러개면 몰루..
        Color32[] vertexColor = textInfo.meshInfo[0].colors32;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (charInfo.isVisible == false) continue;
            charList.Add(new TypeChar(charInfo.vertexIndex, vertices, vertexColor, _startColor, _endColor, i * 0.1f, _oneCharacterTime)
                .SetImg(_bgImg));
        }

        _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);

        StartCoroutine(TypeCoroutine(vertices, vertexColor, charList));

    }

    private IEnumerator TypeCoroutine(Vector3[] vertices, Color32[] colors, List<TypeChar> list)
    {
        _bgImg.rectTransform.DOKill();
        _bgImg.rectTransform.localPosition = new Vector3(2500, 300, 0);

        bool complete = false;
        int cnt = 0;
        _bgImg.rectTransform.DOLocalMove(Vector3.zero, .5f).SetEase(Ease.OutCubic);//.SetEase(Ease.OutCirc);
        // 나타나는 애니메이션
        while (!complete)
        {

            yield return null;
            complete = true;

            foreach (TypeChar c in list)
            {
                c.UpdateChar(vertices, colors);
                if (!c.IsComplete) complete = false;
            }

            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);

            cnt++;
            if (cnt >= 1000000000)
            {
                Debug.Log("안전코드 발동");
                break;
            }
        }
        yield return new WaitForSeconds(0.7f);
        // 잠시 대기

        // 왼쪽으로 사라지는 애니메이션
        complete = false;
        cnt = 0;
        _bgImg.rectTransform.DOLocalMove(new Vector3(-2500, -300, 0), .7f).SetEase(Ease.InCubic);

        while (!complete)
        {
            yield return null;
            complete = true;

            foreach (TypeChar c in list)
            {
                c.DisappearChar(vertices, colors);
                if (!c.IsDisappearComplete) complete = false;
            }
            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);


            cnt++;
            if (cnt >= 1000000000)
            {
                Debug.Log("안전코드 발동");
                break;
            }
        }

        _isType = false;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Char_Selector : MonoBehaviour
{
    [SerializeField] private MenuSoundHelper _soundHelper;
    
    [SerializeField] private Image _player1PosImg;
    [SerializeField] private Image _player2PosImg;


    [SerializeField] private GameObject mapSelectUI;

    [SerializeField] private UI_Characters[] _characters;
    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;
    [SerializeField] private int rightMaxIdx = 0;  // 오른쪽으로 몇개까지 캐릭터가 있는가. 아래있는 캐릭터를 charIndex % 이값으로 나타날 예정.

    [SerializeField] private UI_PlayerStatShow showingStat1;
    [SerializeField] private UI_PlayerStatShow showingStat2;


    private int charIndex1 = 0;
    private int charIndex2 = 0;
    private float charDelay1 = 0;
    private float charDelay2 = 0;

    private bool isBothSelected = false;

    private event Action IsReady;
    private void Start()
    {
        _characters = GetComponentsInChildren<UI_Characters>();

        showingStat1.OnStatChange(_characters[charIndex1].playerStat);
        showingStat2.OnStatChange(_characters[charIndex2].playerStat);
    }
    private void OnEnable()
    {

        isBothSelected = false;
        _inputSO1.ItemUseEvent += HandleSelectCharacter1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;

        _inputSO1.CloseUIEvent += HandleCloseUIEvent;
        IsReady += HandleGoToMapSelect;
    }

    private void HandleCloseUIEvent()
    {

        UI_Manager.Instance.UIOpenOrClose(mapSelectUI, false, transform.parent.gameObject);
    }

    private void HandleGoToMapSelect()
    {
        isBothSelected = true;
        UI_Manager.Instance.UIOpenOrClose(mapSelectUI, true, transform.parent.gameObject);
    }

    private void HandleMoveEvent1()
    {
        _soundHelper.ChangeSound();
        
        if (_characters[charIndex1].IsSelected1)
        {
            return;
        }

        float lastTime = charDelay1;
        charDelay1 = Time.time;

        if (GamePadSystem.UseGamepad && (Time.time - lastTime) < 0.1f) {
            return;
        }

        Vector2 dir = _inputSO1.GetMoveDirection().normalized;
        
        // bool movePos = Mathf.Abs(dir.x) > 0.3f || Mathf.Abs(dir.y) > 0.3f;
        // print($"test1: {test1} / movePos: {movePos} / {dir}");
        // if (!test1) {
        //     if (!movePos)
        //         test1 = true;

        //     return;
        // }
        // else if (movePos) {
        //     test1 = false;
        // }


        int tmpindex = charIndex1;
        Debug.Log(charIndex1);
        charIndex1 += Mathf.RoundToInt(dir.x);
        charIndex1 += rightMaxIdx * -Mathf.RoundToInt(dir.y);  //-1 이 들어오면..
        if (charIndex1 >= _characters.Length || charIndex1 < 0)
        {
            charIndex1 = tmpindex;
        }
        IsOnUp(1);
    }

    private void HandleSelectCharacter1()
    {
        _soundHelper.SelectSound();
        
        if (isBothSelected)
            return;
        GameDataManager.Instance.player1_ObjData = _characters[charIndex1].SelectCharacter1();
        GameDataManager.Instance.player1_StatData = _characters[charIndex1].playerStat;


        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
        }
    }

    private void HandleMoveEvent2()
    {
        _soundHelper.ChangeSound();

        if (_characters[charIndex2].IsSelected2)
        {
            return;
        }

        Vector2 dir = _inputSO2.GetMoveDirection().normalized;
        float lastTime = charDelay2;
        charDelay2 = Time.time;

        if (GamePadSystem.UseGamepad && (Time.time - lastTime) < 0.1f) {
            return;
        }

        int tmpindex = charIndex2;
        charIndex2 += Mathf.RoundToInt(dir.x);
        charIndex2 += rightMaxIdx * -Mathf.RoundToInt(dir.y);  //-1 이 들어오면..
        if (charIndex2 >= _characters.Length || charIndex2 < 0)
        {
            charIndex2 = tmpindex;
        }
        IsOnUp(2);
    }

    private void HandleSelectCharacter2()
    {
        _soundHelper.SelectSound();
        
        if (isBothSelected)
            return;
        GameDataManager.Instance.player2_ObjData = _characters[charIndex2].SelectCharacter2();
        GameDataManager.Instance.player2_StatData = _characters[charIndex2].playerStat;

        if (_characters[charIndex1].IsSelected1 == true && _characters[charIndex2].IsSelected2 == true)
        {
            IsReady?.Invoke();
        }
    }
    public void IsOnUp(int objIdx)  // 여기서 능력치를 보여줘야해..
                                    // 움직일때마다 캐릭터를 바꿔주니까
                                    // _character[charIndex1]에서 statSO값 가져와서 이미지에 넣어주자..
    {
        if (objIdx == 1)
        {
            //_player1PosCam = _characters[charIndex1].   여기 하기   /   stat SO 안에 Pos사진을 넣어줘야해 한별
            _player1PosImg.sprite = _characters[charIndex1].playerStat.playerIcon;
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage1.gameObject.SetActive(false);
            }
            showingStat1.OnStatChange(_characters[charIndex1].playerStat);
            _characters[charIndex1]._isOnTopImage1.gameObject.SetActive(true);

        }
        else
        {
            _player2PosImg.sprite = _characters[charIndex2].playerStat.playerIcon;
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i]._isOnTopImage2.gameObject.SetActive(false);
            }
            showingStat2.OnStatChange(_characters[charIndex2].playerStat);
            _characters[charIndex2]._isOnTopImage2.gameObject.SetActive(true);
        }
    }


    private void OnDisable()
    {

        _inputSO1.ItemUseEvent -= HandleSelectCharacter1;
        _inputSO1.MoveEvent -= HandleMoveEvent1;

        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
        _inputSO2.MoveEvent -= HandleMoveEvent2;

        _inputSO1.CloseUIEvent -= HandleCloseUIEvent;
        IsReady -= HandleGoToMapSelect;
    }

    public void ResetSelectCharacter(int idx)
    {
        if (idx == 1)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected1 = false;
                _characters[i]._selectImage1.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
        else
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].IsSelected2 = false;
                _characters[i]._selectImage2.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
    }
}

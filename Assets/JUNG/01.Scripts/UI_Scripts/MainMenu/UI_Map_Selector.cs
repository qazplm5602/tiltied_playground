using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class UI_Map_Selector : MonoBehaviour
{

    [SerializeField] private GameObject _charSelectUI;

    [SerializeField] private PlayerControlSO _inputSO1;
    [SerializeField] private PlayerControlSO _inputSO2;
    [SerializeField] private UI_Map[] _maps;

    [SerializeField] private int rightMaxIdx = 0;  // 오른쪽으로 몇개까지 캐릭터가 있는가. 아래있는 캐릭터를 charIndex % 이값으로 나타날 예정.


    [SerializeField] private EventMapSO selectSO1 = null;
    [SerializeField] private EventMapSO selectSO2 = null;

    private int charIndex1 = 0;
    private int charIndex2 = 0;

    private void Start()
    {
        _maps = GetComponentsInChildren<UI_Map>();
    }

    private void OnEnable()
    {
        _inputSO1.ItemUseEvent += HandleMapSelectEvent1;
        _inputSO1.MoveEvent += HandleMoveEvent1;

        _inputSO1.CloseUIEvent += HandleCloseUiEvent;

        _inputSO2.ItemUseEvent += HandleSelectCharacter2;
        _inputSO2.MoveEvent += HandleMoveEvent2;

    }

    private void OnDisable()
    {
        _inputSO1.ItemUseEvent -= HandleMapSelectEvent1;
        _inputSO1.MoveEvent -= HandleMoveEvent1;

        _inputSO1.CloseUIEvent -= HandleCloseUiEvent;

        _inputSO2.ItemUseEvent -= HandleSelectCharacter2;
        _inputSO2.MoveEvent -= HandleMoveEvent2;
    }

    private void HandleCloseUiEvent()
    {
        UI_Manager.Instance.UIOpenOrClose(_charSelectUI, true, transform.parent.gameObject);
    }


    private void HandleMoveEvent2()
    {
        if (_maps[charIndex2].IsSelected2)
        {
            return;
        }
        Vector2 dir = _inputSO2.GetMoveDirection().normalized;
        int tmpindex = charIndex2;
        charIndex2 += (int)dir.x;
        charIndex2 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex2 >= _maps.Length || charIndex2 < 0)
        {
            charIndex2 = tmpindex;
        }
        IsOnUp(2);
    }

    private void HandleSelectCharacter2()
    {
        selectSO2 = _maps[charIndex2].SelectMap2();
        if (_maps[charIndex1].IsSelected1 == true && _maps[charIndex2].IsSelected2 == true)
        {
            PercentSelect();
        }
    }

    private void HandleMoveEvent1()
    {
        if (_maps[charIndex1].IsSelected1)
        {
            return;
        }
        Vector2 dir = _inputSO1.GetMoveDirection().normalized;
        int tmpindex = charIndex1;
        charIndex1 += (int)dir.x;
        charIndex1 += rightMaxIdx * -(int)dir.y;  //-1 이 들어오면..
        if (charIndex1 >= _maps.Length || charIndex1 < 0)
        {
            charIndex1 = tmpindex;

        }
        IsOnUp(1);
    }
    private void HandleMapSelectEvent1()
    {
        selectSO1 = _maps[charIndex1].SelectMap1();
        if (_maps[charIndex1].IsSelected1 == true && _maps[charIndex2].IsSelected2 == true)
        {
            PercentSelect();
        }
    }

    private void IsOnUp(int objIdx)
    {
        if (objIdx == 1)
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i]._isOnTopImage1.enabled = false;
            }
            _maps[charIndex1]._isOnTopImage1.enabled = true;
        }
        else
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i]._isOnTopImage2.enabled = false;
            }
            _maps[charIndex2]._isOnTopImage2.enabled = true;
        }
    }
    public void ResetSelectMap(int idx)
    {
        if (idx == 1)
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i].IsSelected1 = false;
                _maps[i]._selectImage1.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
        else
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i].IsSelected2 = false;
                _maps[i]._selectImage2.enabled = false; // 선택 된거 표현 해줄 그림 꺼주기.
            }
        }
    }

    private void PercentSelect()
    {
        if (selectSO1 == selectSO2)
        {
            LoadingManager.LoadScene($"{selectSO1.mapType}Scene");
            //SceneManager.LoadScene() 맵 // selectSO1 에 있는 MapName 을 해주자 .
        }
        else
        {
            int randIdx = Random.Range(1, 3);
            if (randIdx == 1)
            {
                LoadingManager.LoadScene($"{selectSO1.mapType}Scene");
            }
            else if (randIdx == 2)
            {
                LoadingManager.LoadScene($"{selectSO2.mapType}Scene");
            }
        }
    }
}

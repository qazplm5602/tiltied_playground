using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameModeSO gameModeData;
    
    GameMode mode;


    private void Awake() {
        mode = Instantiate(gameModeData.system, transform);
    }

    private void Start() {
        GameStart(); // 이건 테스트
    }

    public void GameStart() {
        mode.GameStart();
    }

    public GameMode GetMode() => mode;
}

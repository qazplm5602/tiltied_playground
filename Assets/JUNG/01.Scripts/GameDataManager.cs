using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    #region Single
    private static GameDataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public PlayerStatsSO player1_StatData;
    public PlayerStatsSO player2_StatData;

    public GameObject player1_ObjData;
    public GameObject player2_ObjData;

    private void Start()
    {
        
    }
    public void LoadingGame()
    {

    }

}

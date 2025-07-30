using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is Null");
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }

    public void PauseTheGame()
    {
        
    }

    public void EnemyGotDestroyed()
    {
        Debug.Log("Enemy got killed");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }


    public Text enemyCount;

    public void UpdateUIElements()
    {
        if(enemyCount!=null) enemyCount.text = "" + GameManager.Instance.GetEnemyCount();
    }
}

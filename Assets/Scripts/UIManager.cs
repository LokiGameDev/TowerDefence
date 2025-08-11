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
    public Text playerScore;
    public Text waveLevel;
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void UpdateUIElements()
    {
        if (enemyCount != null) enemyCount.text = "" + GameManager.Instance._enemyCount;
        if (playerScore != null) playerScore.text = "" + GameManager.Instance._playerScore;
        if (waveLevel != null) waveLevel.text = "" + GameManager.Instance._waveLevel;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}

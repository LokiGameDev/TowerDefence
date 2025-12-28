using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuManager: MonoBehaviour
{
    public GameObject mainMenuPanel,
                      startMenuPanel,
                      settingsMenuPanel,
                      newGamePanel,
                      modelMenu;
    public TMP_InputField saveNameField;
    private int newGameIndex;
    private string saveGamePath;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        startMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        newGamePanel.SetActive(false);
        modelMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsMenu()
    {
        mainMenuPanel.SetActive(false);
        startMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(true);
        modelMenu.SetActive(false);
        SettingsSetup();
    }

    private void MainMenu()
    {
        mainMenuPanel.SetActive(true);
        startMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        modelMenu.SetActive(true);
        newGamePanel.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
        modelMenu.SetActive(false);
    }

    private void SettingsSetup()
    {
        
    }

    public void LoadSaveGame(int saveSlot)
    {
        string path = Application.persistentDataPath + "/" + saveSlot;
        if (Directory.Exists(path))
        {
            string[] folders = Directory.GetDirectories(path);
            string spath = Path.Combine(path, folders[0]);
            PlayerPrefs.SetString("CurrentGamePath", spath);
            PlayerPrefs.Save();
            Debug.Log(spath);
            saveGamePath = spath;
            StartTheGame();
        }
        else
        {
            NewGame(saveSlot);
        }
    }

    public void BactToMainMenu()
    {
        MainMenu();
    }

    public void NewGame(int saveSlot)
    {
        newGamePanel.SetActive(true);
        newGameIndex = saveSlot;
    }

    public void SaveAndPlayNewGame()
    {
        if(saveNameField.text == "") return;
        string path = Path.Combine(Application.persistentDataPath, newGameIndex.ToString());
        string spath = Path.Combine(path, saveNameField.text);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(spath);

        PlayerPrefs.SetString("CurrentGamePath", spath);
        PlayerPrefs.Save();
        Debug.Log(spath);
        saveGamePath = spath;
        StartTheGame();
    }

    public void StartTheGame()
    {
        if(PlayerPrefs.HasKey("CurrentGamePath") && PlayerPrefs.GetString("CurrentGamePath") == saveGamePath)
        {
            StartCoroutine(DelayedLoadScene());
        }
        else
        {
            Debug.LogError("Save game path not set correctly!");
        }
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main");
    }
}

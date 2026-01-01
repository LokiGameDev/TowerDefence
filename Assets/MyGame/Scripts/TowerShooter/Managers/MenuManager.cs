using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class MenuManager: MonoBehaviour
{
    #region Varialbes
    public GameObject mainMenuPanel,
                      startMenuPanel,
                      settingsMenuPanel,
                      newGamePanel,
                      modelMenu;
    public TMP_InputField saveNameField;
    public LoadSaveFileDetails loadSaveFileDetails;
    public GameObject infoBox;
    public TMP_Text infoDisplay;
    private int newGameIndex;
    private string saveGamePath;

    public Toggle toggleFullscreen;
    public Slider volumeSlider;
    #endregion

    #region Startup and Panel Functions
    void Start()
    {
        mainMenuPanel.SetActive(true);
        startMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        newGamePanel.SetActive(false);
        modelMenu.SetActive(true);
        infoBox.SetActive(false);
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

    public void StartMenu()
    {
        mainMenuPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
        modelMenu.SetActive(false);
    }

    private void SettingsSetup()
    {
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
            Screen.fullScreen = isFullscreen;
        }
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            AudioListener.volume = volume;
        }
    }

    #endregion

    #region Settings Functions

    public void FullscreenToggle()
    {
        SetFullscreen(toggleFullscreen.isOn);
    }

    public void VolumeChanged()
    {
        SetVolume(volumeSlider.value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    #endregion

    #region Helping functions

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BactToMainMenu()
    {
        MainMenu();
    }

    public void DisplayInformation(string msg)
    {
        if (!infoBox.activeSelf)
        {
            infoBox.SetActive(true);
            infoDisplay.text = msg;
            StartCoroutine(DisplayMessageCooldown());
        }
        else
        {
            infoDisplay.text = msg;
        }
    }

    IEnumerator DisplayMessageCooldown()
    {
        yield return new WaitForSeconds(2);
        infoBox.gameObject.SetActive(false);
    }

    #endregion

    #region Save Game Functions

    public void LoadSaveGame(int saveSlot)
    {
        string path = Application.persistentDataPath + "/" + saveSlot;
        if (Directory.Exists(path))
        {
            if(loadSaveFileDetails.GetTowerHealthValue(saveSlot) <= 0)
            {
                DisplayInformation("Tower is destroyed. Cannot Play.");
                return;
            }
            string[] folders = Directory.GetDirectories(path);
            string spath = Path.Combine(path, folders[0]);
            PlayerPrefs.SetString("CurrentGamePath", spath);
            PlayerPrefs.Save();
            saveGamePath = spath;
            StartTheGame();
        }
        else
        {
            NewGame(saveSlot);
        }
    }

    public void NewGame(int saveSlot)
    {
        newGamePanel.SetActive(true);
        newGameIndex = saveSlot;
    }

    public void CancelNewGame()
    {
        newGamePanel.SetActive(false);
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
        saveGamePath = spath;
        StartTheGame();
    }

    #endregion

    public void StartTheGame()
    {
        if(PlayerPrefs.HasKey("CurrentGamePath") && PlayerPrefs.GetString("CurrentGamePath") == saveGamePath)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.LogError("Save game path not set correctly!");
        }
    }
}
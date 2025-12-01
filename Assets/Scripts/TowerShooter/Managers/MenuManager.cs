using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public GameObject mainMenuPanel,
                      startMenuPanel,
                      settingsMenuPanel,
                      modelMenu;
    void Start()
    {
        mainMenuPanel.SetActive(true);
        startMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
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
        SceneManager.LoadScene("Main");
    }

    public void BactToMainMenu()
    {
        MainMenu();
    }
}

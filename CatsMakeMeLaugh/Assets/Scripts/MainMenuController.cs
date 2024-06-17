using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static MainMenuController instance;

    public static MainMenuController Instance { get { return instance; } }


    public GameObject mainMenuCanvas;
    public GameObject aboutUsCanvas;
    public GameObject confirmPrompt;
    [SerializeField] TextMeshProUGUI startButtonText;
    [SerializeField] Button startDesktopButton;

    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] string findCatScene = "TeeganDevGrounds";
    [SerializeField] string desktopModeScene = "GameScene_UnityTransparentApp";

    private void Awake()
    {
        if (instance == null) { instance = this; }

        InitializeMenu();
    }

    public void InitializeMenu()
    {
        //startButtonText.text = GameData.Instance.savedCats.cats.Count != 0 ? "Play With Cat" : "Find Your Cat";
        if (GameData.Instance.savedCats.cats.Count == 0) { startDesktopButton.interactable = false; }
        sceneLoader.nextSceneName = GameData.Instance.savedCats.cats.Count != 0 ? desktopModeScene : findCatScene;
    }

    public void StartGame()
    {
        sceneLoader.nextSceneName = desktopModeScene;
        sceneLoader.LoadScene();
    }

    public void AdoptNewCat()
    {
        sceneLoader.nextSceneName = findCatScene;
        sceneLoader.LoadScene();
    }

    public void SettingsToggle()
    {
        if(Settings.Instance.isSettingsMenuOpen == false)
            Settings.Instance.ToggleSettingsMenu();
    }

    public void AboutUs()
    {
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);
        aboutUsCanvas.SetActive(!aboutUsCanvas.activeSelf);
    }

    public void ConfirmQuitPrompt()
    {
        confirmPrompt.SetActive(true);
    }

    public void CloseGame()
    {
        GameData.Instance.SaveAndClose();
    }


    
}

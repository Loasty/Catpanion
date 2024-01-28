using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static MainMenuController instance;

    public static MainMenuController Instance { get { return instance; } }


    public GameObject mainMenuCanvas;
    public GameObject confirmPrompt;
    [SerializeField] TextMeshProUGUI startButtonText;

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
        startButtonText.text = GameData.Instance.savedCat.type != Enums.CatType.NONE ? "Play With Cat" : "Find Your Cat";
        sceneLoader.nextSceneName = GameData.Instance.savedCat.type != Enums.CatType.NONE ? desktopModeScene : findCatScene;
    }

    public void StartGame()
    {
        sceneLoader.LoadScene();
    }

    public void SettingsToggle()
    {
        if(Settings.Instance.isPanelOpen == false)
            Settings.Instance.ToggleSettingsMenu();
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

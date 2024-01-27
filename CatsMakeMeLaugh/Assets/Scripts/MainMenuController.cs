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

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void InitializeMenu()
    {
        startButtonText.text = GameData.Instance.isSaveDataPresent ? "Play With Cat" : "Find Your Cat";
    }

    public void StartGame()
    {
        
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

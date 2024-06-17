using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static Settings instance;

    public static Settings Instance { get { return instance; } }

    /////////////
    /// Settings
    /// 
    [Header("Option Panels Objects")]
    public GameObject settingsPanel;
    [SerializeField] private GameObject optionsMenu_Display;
    [SerializeField] private GameObject optionsMenu_Audio;
    [SerializeField] private GameObject optionsMenu_Game;
    public GameObject confirmPrompt;

    [Header("Options Panels Variables")]
    [SerializeField] private CanvasGroup settingsPanelCanvas;
    bool panelIsFading = false;
    public bool isSettingsMenuOpen = false;
    public bool isConfirmResetPanelOpen = false;

    [Header("Tracked Display Settings")]

    [SerializeField] TMP_InputField pixelCount;
    [SerializeField] Slider taskbar;

    [Header("Tracked Audio Settings")]
    [SerializeField] AudioMixer masterMix;
    [SerializeField] Slider masterVol;
    [SerializeField] Slider meowVol;
    [SerializeField] Slider attentionVol;

    [Header("Tracked Game Settings")]
    [SerializeField] Toggle desktopLaunch;

    /////////////
    /// Events
    /// 
    public delegate void OpenSettings();

    public static event OpenSettings openSettingsMenu;

    /////////////
    /// Unity Functions
    /// 

    private void Awake()
    {
        if (instance == null) { instance = this; }

        isConfirmResetPanelOpen = false;

        isSettingsMenuOpen = false;
        panelIsFading = false;
        settingsPanelCanvas.alpha = 0.0f;
        settingsPanelCanvas.interactable = false;
        settingsPanelCanvas.blocksRaycasts = false;
    }

    private void Start()
    {
        openSettingsMenu += ToggleSettingsMenu;

        taskbar.maxValue = Screen.height;
        taskbar.minValue = 0;

        desktopLaunch.isOn = GameData.Instance.savedSettings.launchInDesktopMode;
        taskbar.value = GameData.Instance.savedSettings.taskbarHeight;
        masterVol.value = GameData.Instance.savedSettings.masterVolume;
        meowVol.value = GameData.Instance.savedSettings.meowVolume;
        attentionVol.value = GameData.Instance.savedSettings.attentionSeekVolume;
    }

    /////////////
    /// Other Functions
    /// 

    public void ToggleSettingsMenu()
    {
        isSettingsMenuOpen = !isSettingsMenuOpen;
        TogglePanel(isSettingsMenuOpen, false);
        //StartCoroutine(SlidePanel(targetPos.position, true));

    }

    public void TogglePanel(bool onOff, bool instant) {

        if (panelIsFading) { return; }

        //GameData.Instance.SaveData();
        
        StartCoroutine(FadePanel(onOff ? 1 : 0, instant));
        settingsPanelCanvas.interactable = onOff;
        settingsPanelCanvas.blocksRaycasts = onOff;
    }

    public IEnumerator FadePanel(float desiredVal, bool instant)
    {
        float curTime = 0;
        float delay = .25f;
        float initVal = settingsPanelCanvas.alpha;

        if(!instant) { 

            while (curTime < delay)
            {
                settingsPanelCanvas.alpha = Mathf.Lerp(initVal, desiredVal, curTime / delay);
                curTime += Time.deltaTime;
                yield return null;
            }
        }

        settingsPanelCanvas.alpha = desiredVal;
        panelIsFading = false;
    }

    public void ToggleLaunchDesktopMode(bool toggle)
    {
        GameData.Instance.savedSettings.launchInDesktopMode = toggle;
    }

    public void UpdateTaskbar()
    {
        pixelCount.text = GameData.Instance.savedSettings.taskbarHeight.ToString();

        if (DesktopMode.Instance != null)
        {
            DesktopMode.Instance.taskbar.transform.position = new Vector2(DesktopMode.Instance.taskbar.transform.position.x, GameData.Instance.savedSettings.taskbarHeight);
        }
    }

    public void AdjustTaskbarLocation(float height)
    {
        GameData.Instance.savedSettings.taskbarHeight = (int)height;

        UpdateTaskbar();
    }

    public void AdjustTaskbar(bool increase)
    {
        if(increase)
        {
            Mathf.Clamp(GameData.Instance.savedSettings.taskbarHeight + 1, taskbar.minValue, taskbar.maxValue);
        } 
        else
        {
            Mathf.Clamp(GameData.Instance.savedSettings.taskbarHeight - 1, taskbar.minValue, taskbar.maxValue);
        }

        taskbar.value = GameData.Instance.savedSettings.taskbarHeight;

        UpdateTaskbar();
    }

    public void AdjustMasterVolume(float volume)
    {
        GameData.Instance.savedSettings.masterVolume = volume;
        masterMix.SetFloat("Master", volume);
    }

    public void AdjustMeowVolume(float volume)
    {
        GameData.Instance.savedSettings.meowVolume = volume;
        masterMix.SetFloat("Meow", volume);
    }

    public void AdjustAttentionSeekVolume(float volume)
    {
        GameData.Instance.savedSettings.attentionSeekVolume = volume;
        masterMix.SetFloat("Attention", volume);
    }

    public void ConfirmResetGameDataPrompt()
    {
        confirmPrompt.SetActive(true);
    }

    public void MainMenu()
    {
        if(MainMenuController.Instance != null)
        {
            MainMenuController.Instance.mainMenuCanvas.SetActive(true);
            TogglePanel(false, true);
            isSettingsMenuOpen = false;
        }
        else
        {
            gameObject.TryGetComponent(out SceneLoader sceneLoader);
            if (sceneLoader == null)
            {
                sceneLoader = gameObject.AddComponent<SceneLoader>();
            }
            sceneLoader.nextSceneName = "MainMenu";
            sceneLoader.unloadPreviousScene = true;
            sceneLoader.LoadScene();
        }

        GameData.Instance.SaveData();
        
    }


    //public IEnumerator SlidePanel(Vector2 desiredPos, bool toggle)
    //{
    //    float curTime = 0;
    //    float delay = .25f;
    //    while(Mathf.Abs(settingsPanel.transform.position.x - desiredPos.x) >= 0.5f)
    //    {
    //        curTime += 0.05f * Time.deltaTime;
    //        settingsPanel.transform.position = Vector2.Lerp(settingsPanel.transform.position, desiredPos, curTime / delay);


    //        yield return new WaitForEndOfFrame();
    //    }


    //    if (DesktopMode.Instance != null)
    //    {
    //        DesktopMode.Instance.taskbar.GetComponent<Image>().enabled = toggle;
    //    }

    //    settingsPanel.SetActive(toggle);

    //}
}

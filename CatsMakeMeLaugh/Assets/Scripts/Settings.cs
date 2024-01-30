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

    public GameObject settingsMenu;
    public GameObject settingsPanel;
    public GameObject confirmPrompt;
    [SerializeField] Vector2 startPos;
    [SerializeField] Transform targetPos;
    public bool isPanelOpen = false;

    [SerializeField] Toggle desktopLaunch;
    [SerializeField] Slider taskbar;
    [SerializeField] Slider masterVol;
    [SerializeField] Slider meowVol;
    [SerializeField] Slider attentionVol;
    [SerializeField] TMP_InputField pixelCount;


    [SerializeField] AudioMixer masterMix;

    public delegate void OpenSettings();

    public static event OpenSettings openSettingsMenu;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        startPos = new Vector2(settingsPanel.transform.position.x, settingsPanel.transform.position.y);

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

    public void ToggleSettingsMenu()
    {
        isPanelOpen = true;
        settingsPanel.SetActive(true);
        StartCoroutine(SlidePanel(targetPos.position, true));

    }

    public IEnumerator SlidePanel(Vector2 desiredPos, bool toggle)
    {
        float curTime = 0;
        float delay = 1.5f;
        while(settingsPanel.transform.position.x - desiredPos.x >= 0.5f)
        {
            curTime += 0.05f * Time.deltaTime;
            settingsPanel.transform.position = Vector2.Lerp(settingsPanel.transform.position, desiredPos, curTime / delay);
            

            yield return new WaitForEndOfFrame();
        }


        if (DesktopMode.Instance != null)
        {
            DesktopMode.Instance.taskbar.GetComponent<Image>().enabled = toggle;
        }

        settingsPanel.SetActive(toggle);

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
            ResetPanelPos();
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

    public void ResetPanelPos()
    {
        isPanelOpen = false;
        settingsPanel.transform.position = new Vector2(startPos.x, startPos.y);
        StartCoroutine(SlidePanel(startPos, false));
        
        if(MouseHandler.Instance != null)
        {
            MouseHandler.Instance.LeftObject();
        }

        GameData.Instance.SaveData();
    }
}

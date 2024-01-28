using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
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

    [SerializeField] Slider taskbar;
    [SerializeField] TMP_InputField pixelCount;

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

        taskbar.maxValue = 1080;
        taskbar.minValue = 0;
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
        settingsPanel.SetActive(toggle);
    }

    public void ToggleLaunchDesktopMode(bool toggle)
    {
        GameData.Instance.launchInDesktopMode = toggle;
    }

    public void UpdateTaskbar()
    {
        pixelCount.text = GameData.Instance.taskbarHeight.ToString();

        if (DesktopMode.Instance != null)
        {
            DesktopMode.Instance.taskbar.transform.position = new Vector2(DesktopMode.Instance.taskbar.transform.position.x, GameData.Instance.taskbarHeight);
        }
    }

    public void AdjustTaskbarLocation(float height)
    {
        GameData.Instance.taskbarHeight = (int)height;

        UpdateTaskbar();
    }

    public void AdjustTaskbar(bool increase)
    {
        if(increase)
        {
            Mathf.Clamp(GameData.Instance.taskbarHeight + 1, taskbar.minValue, taskbar.maxValue);
        } 
        else
        {
            Mathf.Clamp(GameData.Instance.taskbarHeight - 1, taskbar.minValue, taskbar.maxValue);
        }

        taskbar.value = GameData.Instance.taskbarHeight;

        UpdateTaskbar();
    }

    public void AdjustMasterVolume(float volume)
    {
        GameData.Instance.masterVolume = volume;
    }

    public void AdjustMeowVolume(float volume)
    {
        GameData.Instance.meowVolume = volume;
    }

    public void AdjustAttentionSeekVolume(float volume)
    {
        GameData.Instance.attentionSeekVolume = volume;
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
    }
}

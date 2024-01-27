using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Awake()
    {
        if (instance == null) { instance = this; }
        startPos = new Vector2(settingsPanel.transform.position.x, settingsPanel.transform.position.y);
    }

    public void ToggleSettingsMenu()
    {
        isPanelOpen = true;
        settingsPanel.SetActive(true);
        StartCoroutine(SlidePanel());
    }

    public IEnumerator SlidePanel()
    {
        float curTime = 0;
        float delay = 1.5f;
        while(settingsPanel.transform.position.x - targetPos.position.x >= 0.5f)
        {
            curTime += 0.05f * Time.deltaTime;
            settingsPanel.transform.position = Vector2.Lerp(settingsPanel.transform.position, targetPos.position, curTime / delay);
            

            yield return new WaitForEndOfFrame();
        }
    }

    public void ToggleLaunchDesktopMode(bool toggle)
    {
        GameData.Instance.launchInDesktopMode = toggle;
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
        MainMenuController.Instance.mainMenuCanvas.SetActive(true);
        ResetPanelPos();
    }

    public void ResetPanelPos()
    {
        isPanelOpen = false;
        settingsPanel.transform.position = new Vector2(startPos.x, startPos.y);
        settingsPanel.SetActive(false);
    }
}

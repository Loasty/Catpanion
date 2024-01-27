using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static GameData instance;

    public static GameData Instance { get { return instance; } }

    /////////////
    /// Variables
    /// 
    public bool isSaveDataPresent;
    public bool isInDesktopMode;
    public bool launchInDesktopMode;

    public float masterVolume;
    public float meowVolume;
    public float attentionSeekVolume;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void SaveAndClose()
    {
        Application.Quit();
    }

    public void ClearSaveData()
    {

    }
}

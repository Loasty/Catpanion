using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static class Cat
    {
        public static string name;
        public static Enums.CatType type;
        public static int affectionLevel;
    }
    
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

    public List<Sprite> emoteSprites;

    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        StartCoroutine(LoadSaveData());
    }

    public IEnumerator LoadSaveData()
    {

        yield return null;

        sceneLoader.LoadScene();
    }

    public void SaveAndClose()
    {
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void ClearSaveData()
    {

    }
}

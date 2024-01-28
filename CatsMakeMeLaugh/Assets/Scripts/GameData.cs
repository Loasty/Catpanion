using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public class SavedCatNonStatic
    {
        public string name = string.Empty;
        public Enums.CatType type = Enums.CatType.NONE;
        public int affectionLevel = 0;
        public bool acquiredCat = false;
    }

    public class SavedSettings
    {
        public bool launchInDesktopMode = false;
        public int taskbarHeight = 0;
        public float masterVolume = 0;
        public float meowVolume = 0;
        public float attentionSeekVolume = 0;
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

    public List<Sprite> emoteSprites;
    public List<GameObject> catPrefabs;

    [SerializeField] private SceneLoader sceneLoader;

    public delegate void JustForMainMenuJoe();

    public static event JustForMainMenuJoe justForMainMenuJoe;

    public SavedCatNonStatic savedCat;
    public SavedSettings savedSettings;

    public string saveDataLocation;
    public string settingsLocation;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        settingsLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveSettings.json";
        ////REMOVE THIS AFTER TESTING
        //SavedCat.type = Enums.CatType.WHITE;

        StartCoroutine(LoadSaveData());
    }
    

    public IEnumerator LoadSaveData()
    {
        //Enter default values for cat info
        savedCat = new SavedCatNonStatic();
        savedSettings = new SavedSettings();

        LookForSaveData();
        LoadData();

        yield return new WaitForEndOfFrame();


        if(savedSettings.launchInDesktopMode && savedCat.type != Enums.CatType.NONE) { sceneLoader.nextSceneName = "GameScene_UnityTransparentApp"; }
        sceneLoader.LoadScene();
    }

    public void SaveAndClose()
    {
        SaveData();

        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void InvokeJoeSwap()
    {
        justForMainMenuJoe?.Invoke();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(savedCat);
        Debug.Log(json);

        using (StreamWriter write = new StreamWriter(saveDataLocation))
        {
            write.Write(json);
        }

        json = JsonUtility.ToJson(savedSettings);
        Debug.Log(json);

        using (StreamWriter write = new StreamWriter(settingsLocation))
        {
            write.Write(json);
        }

        isSaveDataPresent = true;
    }

    public void LookForSaveData()
    {
        if (File.Exists(saveDataLocation) && File.Exists(settingsLocation))
        {
            Debug.Log("Exists");

        }
        else
        {
            Debug.Log("Doesn't exist. Save thus create.");
            SaveData();
        }
    }

    public void LoadData()
    {
        string data = "";

        using (StreamReader read = new StreamReader(saveDataLocation))
        {
            data = read.ReadToEnd();
        }
        savedCat = JsonUtility.FromJson<SavedCatNonStatic>(data);

        if(savedCat.type != Enums.CatType.NONE)
        {
            isSaveDataPresent = true;
        }

        using (StreamReader read = new StreamReader(settingsLocation))
        {
            data = read.ReadToEnd();
        }
        savedSettings = JsonUtility.FromJson<SavedSettings>(data);
    }

    public void AcquiredCat(CatCharacter cat)
    {
        savedCat.acquiredCat = true;
        savedCat.affectionLevel = cat.affectionLevel;
        savedCat.name = cat.Name;
        savedCat.type = cat.type;

        isSaveDataPresent = true;

        SaveData();

    }

    public void DeleteSaveData()
    {
        savedCat.acquiredCat = false;
        savedCat.affectionLevel = 0;
        savedCat.name = "";
        savedCat.type = Enums.CatType.NONE;

        SaveAndClose();
    }

    public void ChangeCatName(string inName)
    {
        savedCat.name = inName;
        SaveData();
    }



}

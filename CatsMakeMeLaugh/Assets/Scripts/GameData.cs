using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static class SavedCat
    {
        public static string name;
        public static Enums.CatType type;
        public static int affectionLevel;
    }

    public class SavedCatNonStatic
    {
        public  string name;
        public  Enums.CatType type;
        public  int affectionLevel;
        public bool acquiredCat;
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

    public int taskbarHeight = 0;

    public float masterVolume;
    public float meowVolume;
    public float attentionSeekVolume;

    public List<Sprite> emoteSprites;
    public List<GameObject> catPrefabs;

    [SerializeField] private SceneLoader sceneLoader;

    public delegate void JustForMainMenuJoe();

    public static event JustForMainMenuJoe justForMainMenuJoe;

    public SavedCatNonStatic savedCat;

    public string location;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        location = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        //REMOVE THIS AFTER TESTING
        SavedCat.type = Enums.CatType.WHITE;

        StartCoroutine(LoadSaveData());
    }
    

    public IEnumerator LoadSaveData()
    {
        LookForSaveData();
        LoadData();


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

    public void InvokeJoeSwap()
    {
        justForMainMenuJoe?.Invoke();
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(savedCat);
        Debug.Log(json);

        using (StreamWriter write = new StreamWriter(location))
        {
            write.Write(json);
        }



    }
    public void LookForSaveData()
    {
        if (File.Exists(location))
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

        using (StreamReader read = new StreamReader(location))
        {
            data = read.ReadToEnd();
        }
        savedCat = JsonUtility.FromJson<SavedCatNonStatic>(data);

    }

    public void AcquiredCat(CatCharacter cat)
    {
        savedCat.acquiredCat = true;
        savedCat.affectionLevel = cat.affectionLevel;
        savedCat.name = cat.Name;
        SaveData();

    }
    public void DeleteSaveData()
    {
        savedCat.acquiredCat = false;
        savedCat.affectionLevel = 0;
        savedCat.name = "";
        SaveData();
    }


    
}

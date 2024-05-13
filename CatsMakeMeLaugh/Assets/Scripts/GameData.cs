using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public List<Sprite> emoteSprites;
    public List<GameObject> catPrefabs;

    [SerializeField] private SceneLoader sceneLoader;

    public delegate void JustForMainMenuJoe();

    public static event JustForMainMenuJoe justForMainMenuJoe;

    public SavedCats savedCats;
    public SavedSettings savedSettings;

    public string v1_saveDataLocation;
    public string saveDataLocation;
    public string settingsLocation;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        v1_saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SavedCatData.json";
        settingsLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveSettings.json";

        StartCoroutine(LoadSaveData());
    }
    

    public IEnumerator LoadSaveData()
    {
        //Enter default values for cat info
        savedCats = new SavedCats();
        savedSettings = new SavedSettings();

        LookForSaveData();
        LoadData();

        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(2f);

        if(savedSettings.launchInDesktopMode && savedCats.cats.Count != 0) { sceneLoader.nextSceneName = "GameScene_UnityTransparentApp"; }
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
        string json = JsonUtility.ToJson(savedCats);
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

        //There is a version 1 cat we found! overwrite the blank save data to contain this old kitty :)
        if (File.Exists(v1_saveDataLocation))
        {
            Debug.Log("Pushing old cat data to new cat system!");

            Cat newCat;

            string data = "";

            using (StreamReader read = new StreamReader(v1_saveDataLocation))
            {
                data = read.ReadToEnd();
            }

            data = data.Replace("name", "catName");

            newCat = JsonUtility.FromJson<Cat>(data);

            savedCats.cats.Add(newCat);
            SaveData();

            File.Delete(v1_saveDataLocation);
        }
    }

    public void LoadData()
    {
        string data = "";

        using (StreamReader read = new StreamReader(saveDataLocation))
        {
            data = read.ReadToEnd();
        }
        savedCats = JsonUtility.FromJson<SavedCats>(data);

        if(savedCats.cats.Count != 0)
        {
            isSaveDataPresent = true;
        }

        using (StreamReader read = new StreamReader(settingsLocation))
        {
            data = read.ReadToEnd();
        }
        savedSettings = JsonUtility.FromJson<SavedSettings>(data);
    }
    public void AcquiredCat(Character cat)
    {
        Cat newCat = new Cat();
        newCat.acquiredCat = true;
        newCat.affectionLevel = (int)cat.affectionLevel;
        newCat.catName = cat.speakerName;
        newCat.type = cat.cat;

        savedCats.cats.Add(newCat);

        isSaveDataPresent = true;

        SaveData();
    }
    public void AcquiredCat(CatCharacter cat)
    {
        Cat newCat = new Cat();
        newCat.acquiredCat = true;
        newCat.affectionLevel = cat.affectionLevel;
        newCat.catName = cat.Name;
        newCat.type = cat.type;

        savedCats.cats.Add(newCat);

        isSaveDataPresent = true;

        SaveData();

    }

    public void DeleteSaveData()
    {
        savedCats.cats.Clear();

        SaveAndClose();
    }

    public void ChangeCatName(Cat desiredCat, string inName)
    {
        desiredCat.catName = inName;
        SaveData();
    }



}

using Discord;
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
    public ReferenceSprites referenceSprites;
    
    public bool isSaveDataPresent;
    public bool isInDesktopMode;

    public List<Sprite> emoteSprites;
    public List<GameObject> catPrefabs;

    [SerializeField] private SceneLoader sceneLoader;

    public delegate void JustForMainMenuJoe();

    public static event JustForMainMenuJoe justForMainMenuJoe;

    public delegate void SaveDataLoadCompleted();

    public static event SaveDataLoadCompleted saveDataLoadCompleted;

    public SavedCats savedCats;
    public SavedSettings savedSettings;

    
    public string saveDataLocation;
    public string settingsLocation;

    public string archive1_saveDataLocation;
    public string archive2_saveDataLocation;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        // Current Data
        saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SavedCatData_V1.0.sav";
        settingsLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveSettings.json";
        
        // Archived Data To Be Updated
        archive1_saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        archive2_saveDataLocation = Application.dataPath + Path.AltDirectorySeparatorChar + "SavedCatData.json";
        
        StartCoroutine(LoadSaveData());
    }
    

    public IEnumerator LoadSaveData()
    {
        //Enter default values for cat info
        savedCats = new SavedCats();
        savedSettings = new SavedSettings();

        LookForSaveData();
        LoadData(true);

        yield return new WaitForEndOfFrame();

        saveDataLoadCompleted?.Invoke();

        yield return new WaitForSeconds(2f);

        if(savedSettings.launchInDesktopMode && savedCats.cats.Count != 0) { sceneLoader.nextSceneName = "GameScene_UnityTransparentApp"; }
            sceneLoader.LoadScene();
    }

    public void SaveAndClose()
    {
        SaveData(saveDataLocation, settingsLocation);

        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void InvokeJoeSwap()
    {
        justForMainMenuJoe?.Invoke();
    }

    public void SaveData(string catDataLocation, string settingDataLocation, bool encryptData = true)
    {
        string json = JsonUtility.ToJson(savedCats, true);
        Debug.Log(json);

        if (encryptData)
        {
            string encryptedJson = EncryptDecryptData(json);

            using (StreamWriter write = new StreamWriter(catDataLocation))
            {
                write.Write(encryptedJson);
            }
        }
        else
        {
            using (StreamWriter write = new StreamWriter(catDataLocation))
            {
                write.Write(json);
            }
        }

        

        json = JsonUtility.ToJson(savedSettings, true);
        Debug.Log(json);

        using (StreamWriter write = new StreamWriter(settingDataLocation))
        {
            write.Write(json);
        }

        isSaveDataPresent = true;
    }

    [Serializable]
    class archive2OldCat
    {
        public bool acquiredCat;
        public string catName;
        public Enums.CatType type;
        public int affectionLevel;
    }

    [Serializable]
    class archive2OldCats
    {
        private archive2OldCats()
        {
            cats = new List<archive2OldCat>();
        }

        public List<archive2OldCat> cats;
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
            SaveData(saveDataLocation, settingsLocation);
        }

        //There is a version 1 cat we found! overwrite the blank save data to contain this old kitty :)
        if (File.Exists(archive1_saveDataLocation))
        {
            Debug.Log("Pushing old cat data to new cat system!");

            Cat newCat;

            string data = "";

            using (StreamReader read = new StreamReader(archive1_saveDataLocation))
            {
                data = read.ReadToEnd();
            }

            data = data.Replace("name", "catName");

            newCat = JsonUtility.FromJson<Cat>(data);

            savedCats.cats.Add(newCat);
            SaveData(archive2_saveDataLocation, settingsLocation, false);

            File.Delete(archive1_saveDataLocation);
        }

        if(File.Exists(archive2_saveDataLocation))
        {
            Debug.Log("Pushing old cat data to new cat system!");

            Cat newCat;
            archive2OldCats oldCats;

            string data = "";

            using (StreamReader read = new StreamReader(archive2_saveDataLocation))
            {
                data = read.ReadToEnd();
            }
            Debug.Log($" Cat Data Read In: {data}");

            oldCats = JsonUtility.FromJson<archive2OldCats>(data);

            Debug.Log("Cats Read In Correctly: " + oldCats.cats.Count);

            foreach(archive2OldCat oldCat in oldCats.cats)
            {
                newCat = new Cat();

                newCat.acquiredCat = oldCat.acquiredCat;
                newCat.isEnabled = true;
                newCat.isMuted = false;
                newCat.canHumanSpeak = false;

                newCat.catName = oldCat.catName;
                newCat.gender = Enums.CatGender.Unknown;
                newCat.type = oldCat.type;
                newCat.personality = Enums.CatPersonality.Average;

                newCat.meowType = Enums.MeowType.Normal;
                newCat.meowPitch = 0f;

                newCat.currentMood = Enums.CatMood.Normal;
                newCat.affectionLevel = oldCat.affectionLevel;
                newCat.foodLevel = 0;
                newCat.waterLevel = 0;

                newCat.ownerName = "Unknown";
                newCat.adoptionDate = DateTime.Today;

                newCat.clothing_Hat_ID = 0;
                newCat.clothing_Shirt_ID = 0;
                newCat.clothing_Booties_ID = 0;
                newCat.clothing_Extra_ID = 0;


                savedCats.cats.Add(newCat);
            }
            
            SaveData(saveDataLocation, settingsLocation);

            File.Delete(archive2_saveDataLocation);
        }
    }

    private string EncryptDecryptData(string data)
    {
        string result = "";

        for(int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ SecureData.EncryptionKey[i % SecureData.EncryptionKey.Length]);
        }

        return result;
    }

    public void LoadData(bool encrypted)
    {
        string data = "";

        using (StreamReader read = new StreamReader(saveDataLocation))
        {
            data = read.ReadToEnd();
        }
        savedCats = encrypted ? JsonUtility.FromJson<SavedCats>(EncryptDecryptData(data)) : JsonUtility.FromJson<SavedCats>(data);

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

        SaveData(saveDataLocation, settingsLocation);
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

        SaveData(saveDataLocation, settingsLocation);

    }

    public void DeleteSaveData()
    {
        savedCats.cats.Clear();

        SaveAndClose();
    }

    public void ChangeCatName(Cat desiredCat, string inName)
    {
        desiredCat.catName = inName;
        SaveData(saveDataLocation, settingsLocation);
    }



}

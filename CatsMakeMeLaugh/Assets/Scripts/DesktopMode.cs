using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesktopMode : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static DesktopMode instance;

    public static DesktopMode Instance { get { return instance; } }

    public GameObject taskbar;
    [SerializeField] public List<AudioClip> meowSounds;
    [SerializeField] public List<AudioClip> attentionSounds;
    [SerializeField] public List<AudioClip> playSounds;

    private void Awake()
    {
        if (instance == null) instance = this;
        taskbar = GameObject.FindGameObjectWithTag("Taskbar");
        LoadAllCatsIntoScene();
    }

    public void LoadAllCatsIntoScene()
    {
        float screenWidth = Screen.width;
        int numOfCats = GameData.Instance.savedCats.cats.Count;
        float distanceBetweenCats = screenWidth / numOfCats;
        float offsetDistance = distanceBetweenCats / 2;
        int catCounter = 1;

        foreach(Cat cat in GameData.Instance.savedCats.cats)
        {
            Vector3 catSpawnPos = new Vector3((distanceBetweenCats * catCounter) - offsetDistance, 0, 0) + new Vector3(0,0, 0.01f * catCounter);

            GameObject desiredCat = GameData.Instance.catPrefabs[(int)cat.type];
            GameObject spawnCat = Instantiate(desiredCat, taskbar.transform);

            spawnCat.transform.position = catSpawnPos;
            spawnCat.GetComponent<CatController>().LoadCatData(cat);

            spawnCat.GetComponent<SpriteRenderer>().sortingOrder = catCounter * -1;
            catCounter++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Settings.Instance.UpdateTaskbar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

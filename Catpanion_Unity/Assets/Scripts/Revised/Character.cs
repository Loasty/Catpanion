using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public string speakerName;
    public Enums.CatType cat = Enums.CatType.NONE;
    public Enums.CatPersonality catPersonality = Enums.CatPersonality.Average;
    public Enums.AdoptionDifficulty difficulty = Enums.AdoptionDifficulty.Easy;
    public bool chosenCat;
    public string accessKey = "{}";
    public Animator animator;
    public GameObject characterObj;
    public Emote emote;
    public float affectionLevel = 0;
    public Color colorTextbox = Color.white;
    public Color colorSpeakerbox = Color.white;
    







    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Animations
{
    public Enums.Actions actions = Enums.Actions.SIT;
    public string animationName;

    public Animations(){ }
    public Animations(Enums.Actions actions, string animationName) { this.actions = actions; this.animationName = animationName; }


}
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

    //public List<Animations> animations = new List<Animations>();
    public Dictionary<Enums.Actions, string> animationDict = new Dictionary<Enums.Actions, string>();
    
    public void SetUpAnimationDict(List<Animations> inAnims)
    {
        foreach(Animations a in inAnims)
        {
           animationDict.Add(a.actions, a.animationName);
        }
    }
    public string GetAnimation(Enums.Actions a)
    {
        string val = "";
        animationDict.TryGetValue(a, out val);
        return val;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

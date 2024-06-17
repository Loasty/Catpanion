using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cat
{
    public bool acquiredCat = false;

    public bool isEnabled = true;
    public bool isMuted = false;
    public bool canHumanSpeak = false;

    public string catName = string.Empty;
    public Enums.CatGender gender = Enums.CatGender.Unknown;
    public Enums.CatType type = Enums.CatType.NONE;
    public Enums.CatPersonality personality = Enums.CatPersonality.Average;

    public Enums.MeowType meowType = Enums.MeowType.Normal;
    public float meowPitch = 0f;

    public Enums.CatMood currentMood = Enums.CatMood.Normal;
    public int affectionLevel = 0;
    public int foodLevel = 0;
    public int waterLevel = 0;

    public string ownerName = string.Empty;
    public DateTime adoptionDate = DateTime.Today;

    public int clothing_Hat_ID = 0;
    public int clothing_Shirt_ID = 0;
    public int clothing_Booties_ID = 0;
    public int clothing_Extra_ID = 0;
}


[Serializable]
public class SavedCats
{
    public SavedCats()
    {
        cats = new List<Cat>();
    }

    public List<Cat> cats;
}

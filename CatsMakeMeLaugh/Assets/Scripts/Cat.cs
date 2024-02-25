using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cat
{
    public string catName = string.Empty;
    public Enums.CatType type = Enums.CatType.NONE;
    public int affectionLevel = 0;
    public bool acquiredCat = false;
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

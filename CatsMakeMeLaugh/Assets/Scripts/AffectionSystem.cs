using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[Serializable]
public class CatCharacter
{
    public string Name;
    public Enums.CatType type = Enums.CatType.NONE;
    public int affectionLevel = 0;
    public Color color;
    public GamePlayLoop gamePlayLoop;
}

public class AffectionSystem : MonoBehaviour
{
    [SerializeField]
    private List<CatCharacter> cats;
    public Dictionary<Enums.CatType, CatCharacter> catsDict = new Dictionary<Enums.CatType, CatCharacter> ();

    public static AffectionSystem Instance { get { return instance; } }
    [SerializeField]
    private static AffectionSystem instance;

    public List<OptionsButton> optionButtons = new List<OptionsButton>();
    
   
    // Start is called before the first frame update
    
    void Start()
    {
        SetInstance();
        SortCats();
    }
    public void SetInstance()
    {
        if (instance == null) { instance = this; }
        SortCats();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SortCats()
    {

        if (cats.Count != catsDict.Count)
        {
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    try
                    {
                        catsDict.Add(cats[i].type, cats[i]);
                    }
                    catch
                    {
                        Debug.LogError("Something went wrong. Make sure that there is no multiple of enum cat type. There should be one of each.");
                    }
                }
            }
        }
    }

    public void ModifyAffectionPoints(int val, Enums.CatType inCatType)
    {
        CatCharacter cat;
        catsDict.TryGetValue(inCatType, out cat);
        if (cats != null) { cat.affectionLevel += val; }
    }
    
    
    
   



   

    
    
   
}

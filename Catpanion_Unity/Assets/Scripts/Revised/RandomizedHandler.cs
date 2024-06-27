using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RandomizedCatHandler
{
    //We're trying to be as generic as possible
    public Enums.CatType catType;
    public GameObject prefab;
    public int limitPerRun;
    public int count;

    public RandomizedCatHandler(Enums.CatType catType, GameObject prefab, int limitPerRun, int count)
    {
        this.catType = catType;
        this.prefab = prefab;
        this.limitPerRun = limitPerRun;
        this.count = count;
    }

    public RandomizedCatHandler() { }
}
[System.Serializable]
public class RandomizedPersonalityPool
{
    //We're trying to be as generic as possible
    public Enums.AdoptionDifficulty adoptionDifficulty;
    public List<RandomizedPersonality> personalityList = new List<RandomizedPersonality>();
    public int limitPerRun;
    public int count;


    public RandomizedPersonalityPool(Enums.AdoptionDifficulty adoptionDifficulty, List<RandomizedPersonality> personalityList, int limitPerRun, int count)
    {
        this.adoptionDifficulty = adoptionDifficulty;
        foreach(RandomizedPersonality p in personalityList)
        {
            RandomizedPersonality randomizedPersonality = new RandomizedPersonality(p.catPersonality, p.personalityLimitPerRun, p.count);
            this.personalityList.Add(randomizedPersonality);
        }
        //this.personalityList = personalityList;
        this.limitPerRun = limitPerRun;
        this.count = count;
    }
    public RandomizedPersonalityPool()
    {

    }
}
[System.Serializable]
public class RandomizedPersonality
{
    //Again, generic as possible.
    public Enums.CatPersonality catPersonality;
    public int personalityLimitPerRun = 0; //No limit
    public int count;

    public RandomizedPersonality(Enums.CatPersonality catPersonality, int personalityLimitPerRun, int count)
    {
        this.catPersonality = catPersonality;
        this.personalityLimitPerRun = personalityLimitPerRun;
        this.count = count;
    }
    public RandomizedPersonality() { }
}



public class RandomizedHandler : MonoBehaviour
{
    [SerializeField]
    int MaxCatCount = 4;
    [SerializeField]
    Transform spawnPos;



    public List<Character> catsList = new List<Character>();
    public List<RandomizedCatHandler> catTypes = new List<RandomizedCatHandler>();
    public List<RandomizedPersonalityPool> personalityPools = new List<RandomizedPersonalityPool>();


    public bool debugMode = false;
    // Start is called before the first frame update
    void Start()
    {


        RandomizeCat();
        StartCoroutine(test());

        //maxEnums = System.Enum.GetValues(typeof(Enums.CatPersonality)).Cast<Enums.CatPersonality>().ToList<Enums.CatPersonality>().Count - 1;

    }

    public void Wipe()
    {
        
        for (int i = 0; i < catsList.Count; i++)
        {
            Destroy(catsList[i].characterObj);
            Destroy(catsList[i]);
        }
        catsList.Clear();
    }
    public void DisplayNames()
    {
        string message = "Cats: ";
        foreach (Character c in catsList)
        {
            message += c.speakerName + ", ";
        }
        Debug.Log(message);

    }
    IEnumerator test()
    {
        DisplayNames();
        while (true)
        {

            yield return new WaitForSeconds(2f);
            Wipe();
            RandomizeCat();
            DisplayNames();
        }
        

    }

    public void RandomizeCat()
    {
        ////Made to be as generic as possible to allow for more cats to be added in the future as easily as possible
        List<Character> list = new List<Character>();
      


        List<Enums.CatType> validCatTypes = System.Enum.GetValues(typeof(Enums.CatType)).Cast<Enums.CatType>().ToList();
        validCatTypes.Remove(Enums.CatType.NONE);
        Dictionary<Enums.CatType, RandomizedCatHandler> catHandler = new Dictionary<Enums.CatType, RandomizedCatHandler>(); foreach (RandomizedCatHandler c in catTypes) { catHandler.Add(c.catType, c); }
        

        Dictionary<Enums.AdoptionDifficulty, RandomizedPersonalityPool> personalityHandler = new Dictionary<Enums.AdoptionDifficulty, RandomizedPersonalityPool>();
        List<RandomizedPersonalityPool> personalityPoolsLocal = new List<RandomizedPersonalityPool>(); foreach (RandomizedPersonalityPool p in personalityPools) { personalityPoolsLocal.Add(new RandomizedPersonalityPool(p.adoptionDifficulty, p.personalityList, p.limitPerRun, p.count)); }
        foreach (RandomizedPersonalityPool p in personalityPoolsLocal) { personalityHandler.Add(p.adoptionDifficulty, p);}
        List<Enums.AdoptionDifficulty> validDifficulties = System.Enum.GetValues(typeof(Enums.AdoptionDifficulty)).Cast<Enums.AdoptionDifficulty>().ToList<Enums.AdoptionDifficulty>();
  
        for (int i = 0; i < MaxCatCount; i++)
        {
            Character cat = this.AddComponent<Character>();

            //Cat Type
            int rand = Random.Range(0, validCatTypes.Count);
            Debug.Log("Rand: " + rand + validCatTypes[rand]);
            catHandler[validCatTypes[rand]].count++;
            cat.cat = validCatTypes[rand];
            cat.characterObj = GameObject.Instantiate(catHandler[validCatTypes[rand]].prefab, spawnPos);

             if (catHandler[validCatTypes[rand]].limitPerRun > 0 && catHandler[validCatTypes[rand]].count >= catHandler[validCatTypes[rand]].limitPerRun) { validCatTypes.RemoveAt(rand); } 

            //Cat difficulty
            int randDifficulty = Random.Range(0, validDifficulties.Count);
            Debug.Log("randDifficulty: " + randDifficulty);
            Debug.Log("difficulty = " + validDifficulties[randDifficulty]);
            personalityHandler[validDifficulties[randDifficulty]].count++;
            cat.difficulty = validDifficulties[randDifficulty];
             if (personalityHandler[validDifficulties[randDifficulty]].limitPerRun >= 0 && personalityHandler[validDifficulties[randDifficulty]].count == personalityHandler[validDifficulties[randDifficulty]].limitPerRun) { validDifficulties.RemoveAt(randDifficulty); };
            
            //Cat personality
            RandomizedPersonalityPool pool = new RandomizedPersonalityPool(cat.difficulty, personalityHandler[cat.difficulty].personalityList, personalityHandler[cat.difficulty].limitPerRun, personalityHandler[cat.difficulty].count);

            //filter out
            foreach (Character c in list)
            {
                if (c.cat == cat.cat && c.difficulty == cat.difficulty)
                {
                    for (int j = 0; j < pool.personalityList.Count; j++)
                    {
                        if (pool.personalityList[j].catPersonality == c.catPersonality)
                        {
                            Debug.Log("Found existing");
                            pool.personalityList.RemoveAt(j);
                            break;

                        }
                    }
                }
            }
            
            
            for (int j = 0; j < pool.personalityList.Count; j++)
            {
                if (pool.personalityList[j].personalityLimitPerRun > 0 && pool.personalityList[j].count >= pool.personalityList[j].personalityLimitPerRun)
                {
                    pool.personalityList.RemoveAt(j);
                }
            }


            int randPersonality = Random.Range(0, pool.personalityList.Count);

            Debug.Log("Rand Personality: " + randPersonality);
            cat.catPersonality = pool.personalityList[randPersonality].catPersonality;
            foreach(RandomizedPersonality c in personalityHandler[cat.difficulty].personalityList)
            {
                if (c.catPersonality == cat.catPersonality)
                {
                    c.count++;
                    Debug.Log(c.count);
                }
            }

            cat.speakerName = cat.catPersonality.ToString().ToLower().FirstCharacterToUpper() + " " + cat.cat.ToString().ToLower().FirstCharacterToUpper() + " Cat";
            cat.characterObj.transform.localScale = new Vector3(3, 3, 3);
            if (debugMode)
            {
                cat.characterObj.GetComponent<CatController>().nameDisplay.text = cat.speakerName;

            }
            else
            {
                cat.characterObj.GetComponent<CatController>().nameDisplay.gameObject.SetActive(false);
            }
            cat.characterObj.GetComponent<CatController>().enabled = false;
            list.Add(cat);
        }

        
        foreach (RandomizedCatHandler c in catTypes)
        {
            c.count = 0;
        }

        foreach(Enums.AdoptionDifficulty e in System.Enum.GetValues(typeof(Enums.AdoptionDifficulty)).Cast<Enums.AdoptionDifficulty>().ToList<Enums.AdoptionDifficulty>())
        {
            foreach (RandomizedPersonality c in personalityHandler[e].personalityList)
            {
                c.count = 0;
              
            }
        }
      

   
        catsList = list;


















    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

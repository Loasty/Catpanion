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

}
public class RandomizedPersonalityPool
{
    //We're trying to be as generic as possible
    public Enums.AdoptionDifficulty adoptionDifficulty;
    public List<RandomizedPersonality> personalityList;
    public int limitPerRun;
    public int count;

}
public class RandomizedPersonality
{
    //Again, generic as possible.
    public Enums.CatPersonality catPersonality;
    int personalityLimitPerRun = 0; //No limit
    public int count;

}



public class RandomizedHandler : MonoBehaviour
{
    [SerializeField]
    int MaxCatCount = 4;
 
  

    public List<Character> catsList = new List<Character>();
    public List<RandomizedCatHandler> catTypes = new List<RandomizedCatHandler>();
    public List<RandomizedPersonalityPool> personalityPools = new List<RandomizedPersonalityPool>();
 

















    // Start is called before the first frame update
    void Start()
    {

        
        //easy.Add(Enums.CatPersonality.Average);
        //easy.Add(Enums.CatPersonality.Energetic);
        //easy.Add(Enums.CatPersonality.Gentle);
        //easy.Add(Enums.CatPersonality.Goofy);
        //difficultyDict.Add(Enums.AdoptionDifficulty.Easy, easy);
        

        //medium.Add(Enums.CatPersonality.Grumpy);
        //medium.Add(Enums.CatPersonality.Lazy);
        //medium.Add(Enums.CatPersonality.Confident);
        //difficultyDict.Add(Enums.AdoptionDifficulty.Medium, medium);

        //hard.Add(Enums.CatPersonality.Timid);
        //hard.Add(Enums.CatPersonality.Anxious); 
        //hard.Add(Enums.CatPersonality.Wizard);
        //difficultyDict.Add(Enums.AdoptionDifficulty.Hard, hard);





        //maxEnums = System.Enum.GetValues(typeof(Enums.CatPersonality)).Cast<Enums.CatPersonality>().ToList<Enums.CatPersonality>().Count - 1;

    }

    public void RandomizeCat()
    {
        List<Character> list = new List<Character>();


        ////Made to be as generic as possible to allow for more cats to be added in the future as easily as possible

        List<Enums.CatType> validCatTypes = System.Enum.GetValues(typeof(Enums.CatType)).Cast<Enums.CatType>().ToList();
        validCatTypes.Remove(Enums.CatType.NONE);
        //List<Enums.CatType> validCatTypes = System.Enum.GetValues(typeof(Enums.CatType)).Cast<Enums.CatType>().ToList();

        Dictionary<Enums.CatType, RandomizedCatHandler> catHandler = new Dictionary<Enums.CatType, RandomizedCatHandler>();
        foreach (RandomizedCatHandler c in catTypes) { if (catHandler[c.catType] == null) { catHandler.Add(c.catType, c); } }

        Dictionary<Enums.AdoptionDifficulty, RandomizedPersonalityPool> personalityHandler = new Dictionary<Enums.AdoptionDifficulty, RandomizedPersonalityPool>();
        foreach (RandomizedPersonalityPool p in personalityPools) { if (personalityHandler[p.adoptionDifficulty] == null) { personalityHandler.Add(p.adoptionDifficulty, p); } }
  

        for (int i = 0; i < MaxCatCount; i++)
        {
            //Cat Type
            int rand = Random.Range(0, validCatTypes.Count);
            catHandler[(Enums.CatType)rand].count++;
            if (catHandler[(Enums.CatType)rand].limitPerRun > 0) { if (catHandler[(Enums.CatType)rand].count > catHandler[(Enums.CatType)rand].limitPerRun) { validCatTypes.Remove((Enums.CatType)rand); } }
            
            

        }

        ////Made to be as generic as possible, for more difficulties to be added as easily as possible.
        //List<Enums.AdoptionDifficulty> validDifficulties = System.Enum.GetValues(typeof(Enums.AdoptionDifficulty)).Cast<Enums.AdoptionDifficulty>().ToList();
        //List<int> difficultyCount = new List<int>(validDifficulties.Count);
        //List<int> difficultyLimit = new List<int>(validDifficulties.Count);
        //difficultyLimit[0] = 1;
        //difficultyLimit[0] = 1;
        //difficultyLimit[0] = 2;



        //for (int i = 0; i < MaxCatCount; i++)
        //{
        //    int rand = Random.Range(0, validCatTypes.Count);
        //    catCount[rand]++;
        //    if (catCount[rand] > typelimit) { validCatTypes.Remove((Enums.CatType)rand); }

        //    int randDifficulties = Random.Range(0, catCount.Count);
        //    difficultyCount[randDifficulties]++;
        //    if (difficultyCount[randDifficulties] > difficultyLimit[randDifficulties])
        //    {
        //        difficultyCount.RemoveAt(randDifficulties);
        //        difficultyLimit.RemoveAt(randDifficulties);
        //    }

        //    List<Enums.CatPersonality> catPersonalities = new List<Enums.CatPersonality>();
        //    difficultyDict.TryGetValue((Enums.AdoptionDifficulty)randDifficulties, out catPersonalities);

        //    //Filters out on the local version
        //    if (list.Count > 0)
        //    {
        //        foreach(Character c in list)
        //        {
        //            //If it's the same cat type
        //            if (c.cat == (Enums.CatType)rand)
        //            {
        //                catPersonalities.Remove(c.catPersonality);   
        //            }
        //        }
        //    }
        //    //Now only the valid versions can be given.
        //    int catPersonality = Random.Range(0, catPersonalities.Count);


        //    Character cat = new Character();
        //    cat.cat = (Enums.CatType)rand;
        //    cat.difficulty = (Enums.AdoptionDifficulty)randDifficulties;
        //    cat.catPersonality = (Enums.CatPersonality)catPersonality;
        //    cat.speakerName = catPersonality.ToString().ToLower().FirstCharacterToUpper() + " " + cat.cat.ToString().ToLower().FirstCharacterToUpper() + " Cat";
        //    cat.characterObj = 

        //    list.Add(cat);



        //}



























    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

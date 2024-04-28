using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    List<Character> characters = new List<Character>();

    public Dictionary<Enums.CatType, Character> charactersDict = new Dictionary<Enums.CatType, Character>();
    public static CharacterManager Instance { get { return instance; } }
    private static CharacterManager instance;
    public Dictionary<string, Enums.CatType> secretKey = new Dictionary<string, Enums.CatType>();
    public string whiteCatKey = "$WhiteCat$";
    public string tabbyCatKey = "$TabbyCat$";
    public string blackCatKey = "$BlackCat$";
    public string calicoCatKey = "$CalicoCat$";
    int MAX_AFFECTION = 30;
    


    private void Awake()
    {
        instance = this;
        for (int i = 0; i < characters.Count; i++)
        {
            try
            {
                charactersDict.Add(characters[i].cat, characters[i]);
            }
            catch
            {
                Debug.LogError("Cat configuration not correct, check enums on characters.");
            }
        }
        secretKey.Add(whiteCatKey, Enums.CatType.WHITE);
        secretKey.Add(blackCatKey, Enums.CatType.BLACK);
        secretKey.Add(tabbyCatKey, Enums.CatType.TABBY);
        secretKey.Add(calicoCatKey, Enums.CatType.CALICO);

    }
    private void OnDestroy()
    {
        instance = null;
    }
    public Character GetCharacter(Enums.CatType type)
    {
       
            Character chara;
            charactersDict.TryGetValue(type, out chara);
            return chara;
       
    }
    public Character GetCharacterFromKey(string key)
    {
        try
        {
            Character character;
            character = GetCharacter(secretKey[key]);
            if (character != null)
            {
                return character;
            }
        }
        catch
        {
            Debug.LogError("Not a valid key, check enums on characters.");
        }
        return null;
    }
    
    public string GetNameFromKey(string key)
    {
        try
        {
            Character character;
            character = GetCharacter(secretKey[key]);
            if (character != null)
            {
                return character.name;
            }
        }
        catch
        {
            Debug.LogError("Not a valid key, check enums on characters.");
        }
        return "";
    }
    public void DisableAllCharacterObj()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].characterObj.SetActive(false);
            if (characters[i].emote != null)
            {
                characters[i].emote.gameObject.SetActive(false);
            }
        }
    }

    public void ManageVisible(List<OnScreenCharacter> catsActive)
    {
        DisableAllCharacterObj();
        if (catsActive.Count > 0)
        {
            for (int i = 0; i < catsActive.Count; i++)
            {
                Character cat;
                charactersDict.TryGetValue(catsActive[i].characterType, out cat);
                if (cat != null) { cat.characterObj.SetActive(true); }
                if (catsActive[i].emoteEffect != Enums.EmoteEffect.NONE)
                {
                    if (cat.emote != null)
                    {

                        cat.emote.gameObject.SetActive(true);
                        cat.emote.PlayEmote(catsActive[i].emotes, catsActive[i].emoteEffect, catsActive[i].emoteDuration, catsActive[i].emoteRepeats, catsActive[i].emoteSpeed);
                    }
                }
            }
        }
    }
    public void HandleAnim(Animator animator, Enums.Actions action)
    {
        if (action == Enums.Actions.SWIPE)
        {
            animator.SetTrigger("Swipe");


        }
        else if (action == Enums.Actions.GREET)
        {
            animator.SetTrigger("Greet");
        }
        
    }

    public void HandleAnims(List<OnScreenCharacter> cats, bool enable)
    {
        for (int i = 0; i < cats.Count; i++)
        {

            Character cat;
            charactersDict.TryGetValue(cats[i].characterType, out cat);
            if (cat != null)
            {

                if (enable)
                {
                    HandleAnim(cat.animator, cats[i].action);
                }
                else
                {
                    cat.animator.SetBool("WalkLeft", false);
                    cat.animator.SetBool("WalkRight", false);
                }
            }
            
        }
    }
  
    public void HandleAffectionImpact(AffectType affectType)
    {
        try
        {
            charactersDict[affectType.affectCat].affectionLevel += affectType.affectionImpact;
        }
        catch
        {
            Debug.LogError("Check cat type");
        }
    }
    public bool WonAffectionsCheck(Enums.CatType catType)
    {
        try
        {
            if (charactersDict[catType].affectionLevel >= MAX_AFFECTION)
            {
                return true;
            }
        }
        catch
        {
            Debug.LogError("Check enums");
        }
        return false;
    }
    public void HandleIfWonAffections(Enums.CatType catType)
    {
        if (WonAffectionsCheck(catType))
        {
            MassDialogueManager.Instance.WonAffections(catType);
        }
        else
        {
            
        }
    }
}

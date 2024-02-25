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
    }

    public Character GetCharacter(Enums.CatType type)
    {
       
            Character chara;
            charactersDict.TryGetValue(type, out chara);
            return chara;
       
    }
    public void DisableAllCharacterObj()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].characterObj.SetActive(false);
            characters[i].emote.gameObject.SetActive(false);
        }
    }

    public void ManageVisible(List<OnScreenCharacter> catsActive)
    {
        DisableAllCharacterObj();
        for (int i = 0; i < catsActive.Count; i++)
        {
            Character cat;
            charactersDict.TryGetValue(catsActive[i].characterType, out cat);
            if (cat != null) { cat.characterObj.SetActive(true); }
            if (catsActive[i].emoteEffect != Enums.EmoteEffect.NONE)
            {
                cat.emote.gameObject.SetActive(true);
                cat.emote.PlayEmote(catsActive[i].emotes, catsActive[i].emoteEffect, catsActive[i].emoteDuration, catsActive[i].emoteRepeats, catsActive[i].emoteSpeed);
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
  
}

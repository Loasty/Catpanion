using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GamePlayLoop : MonoBehaviour
{
    //public CatCharacter savedCat;
    int maxAffection = 20;

    [SerializeField]
    List<DialogueManager> dialogueManagerLoop;
    public DialogueManager acquiredAffectionOfCat;
    public Enums.CatType catType;
    public DialogueManager beginning;
    public NameCat nameCat;
    CatCharacter cat;
    int currentIndex = 0;
    public GameObject speakerBox;
    public GameObject uiBox;


    // Start is called before the first frame update
    void Awake()
    {
        if (AffectionSystem.Instance == null)
        {
            AffectionSystem inst = FindObjectOfType<AffectionSystem>();
            if (inst == null) { Debug.LogError("There needs to be at least ONE Affection System in the Scene!!"); }
            else
            {
                inst.SetInstance();
            }
        }


    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        currentIndex = 0;
        dialogueManagerLoop[currentIndex].gameObject.SetActive(true);

    }
    

    public void UIOn()
    {
        speakerBox.SetActive(true);
        uiBox.SetActive(true);
    }
    public void Next()
    {
        dialogueManagerLoop[currentIndex].gameObject.SetActive(false);
        currentIndex++;
        if (currentIndex >= (dialogueManagerLoop.Count))
        {

            if (cat.affectionLevel >= maxAffection)
            {
                currentIndex = 0;
                acquiredAffectionOfCat.gameObject.SetActive(true);
            }
            else
            {
                currentIndex = 0;
                //currentIndex = 0;
                dialogueManagerLoop[currentIndex].gameObject.SetActive(true);
            }
        }
        else
        {
            //currentIndex = 0;
            dialogueManagerLoop[currentIndex].gameObject.SetActive(true);
        }
    }
    public void DisableAll()
    {
        for (int i = 0; i < dialogueManagerLoop.Count; i++)
        {
            dialogueManagerLoop[i].gameObject.SetActive(false);
        }
    }
    public void GoToNameCat()
    {
        acquiredAffectionOfCat.gameObject.SetActive(false);
        nameCat.characterCat = cat;
        nameCat.gameObject.SetActive(true);
    }


}
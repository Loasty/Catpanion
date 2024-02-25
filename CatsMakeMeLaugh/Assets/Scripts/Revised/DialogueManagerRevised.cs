using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[Serializable]
public class DialogueRevised
{
    public string dialogue;
    public Enums.DialogueSpeed speed = Enums.DialogueSpeed.NORMAL;
    public List<OptionsRevised> options = new List<OptionsRevised>();
    public List<OnScreenCharacter> OnScreen = new List<OnScreenCharacter>();
    public Enums.CatType speakerCharacter = Enums.CatType.NONE;
    public bool changeLocation;
    public Enums.Locations newLocation = Enums.Locations.NONE;
    public UnityEvent startEvents;
    public UnityEvent endEvents;

}
[Serializable]
public class OptionsRevised
{
    public string dialogueName;
    public DialogueManagerRevised transitonTo;
    public List<AffectType> impact;
    public bool returnToHere;

}
[Serializable]
public class OnScreenCharacter
{
    public Enums.CatType characterType;
    public Enums.Actions action;
}


public class DialogueManagerRevised : MonoBehaviour
{
    [Header("Dialogues")]
    public List<DialogueRevised> dialogues  = new List<DialogueRevised>();
    public int currentIndex = -1;
    //float speed = 0.01f;

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI textDialogueBox;
    [SerializeField]
    TextMeshProUGUI textSpeaker;
    [SerializeField]
    Image speakerBox;
    [SerializeField]
    Image textBox;

  
    [Header("Other")]
    [SerializeField]
    KeyCode nextDialogueBtn = KeyCode.Mouse0;
    public bool readInComplete = false;
    public bool lastDialogueManager = false;
    public bool canContinue = true;
    public bool lastDialogue = false;
    public delegate void DialogueEvent();
    public DialogueEvent NotifyDialogueComplete;

   
    [HideInInspector]
    public DialogueManagerRevised returnTo;


    private void OnEnable()
    {
        canContinue = true;
        DialogueForward();
        
    }
    private void OnDisable()
    {
        
        canContinue = false;
        readInComplete = true;
        StopAllCoroutines();
        if (returnTo != null)
        {
            returnTo.gameObject.SetActive(true);
        }
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (canContinue)
        {
            if (Input.GetKeyDown(nextDialogueBtn))
            {
                if (readInComplete)
                {
                    if (lastDialogue)
                    {
                        NotifyDialogueComplete?.Invoke();
                        //if (dialogues[currentIndex].endEvents != null) { dialogues[currentIndex].endEvents.Invoke(); }
                        lastDialogue = false;
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        CharacterManager.Instance.HandleAnims(dialogues[currentIndex].OnScreen, false);
                        if (canContinue) { DialogueForward(); }
                    }
                }
                else
                {
                    StopAllCoroutines();
                    textDialogueBox.text = dialogues[currentIndex].dialogue;
                    readInComplete = true;
                    if (dialogues[currentIndex].options.Count > 0)
                    {
                        canContinue = false;
                        MassDialogueManager.Instance.DisplayButtons(dialogues[currentIndex].options, this);
                    }

                }
            }
        }


    }
    
    public void DialogueForward()
    {
        currentIndex++;
        if (currentIndex >= (dialogues.Count -1))
        {
            currentIndex = dialogues.Count - 1;
            lastDialogue = true;
        }
        UpdateUI(dialogues[currentIndex]);
        

    }
    public void IncreaseIndex()
    {
        currentIndex++;
        if (currentIndex >= (dialogues.Count - 1))
        {
            currentIndex = dialogues.Count - 1;
            lastDialogue = true;
        }
    }
    public void UpdateUI(DialogueRevised inDialogue)
    {
        if (inDialogue.changeLocation) { RevisedLocationManager.Instance.ChangeLocation(inDialogue.newLocation); }
        if (textBox.gameObject != null) { if (textBox != null) { textBox.color = Color.white; } }
        if (inDialogue.OnScreen.Count > 0) { CharacterManager.Instance.ManageVisible(inDialogue.OnScreen); }
        if (inDialogue.speakerCharacter != Enums.CatType.NONE)
        {
            Character character = CharacterManager.Instance.GetCharacter(inDialogue.speakerCharacter);
            if (character != null)
            {
                speakerBox.gameObject.SetActive(true);
                speakerBox.color = character.colorSpeakerbox;
                textSpeaker.text = character.speakerName;
                textBox.color = character.colorTextbox;
            }
            else 
            {
               speakerBox.gameObject.SetActive(false);
            }
            

        }
        else { speakerBox.gameObject.SetActive(false); }

        if (inDialogue.startEvents != null) { inDialogue.startEvents.Invoke(); }
        
        CharacterManager.Instance.HandleAnims(inDialogue.OnScreen, true);
        float speed = 0.1f;
        MassDialogueManager.Instance.dialogueSpeeds.TryGetValue(inDialogue.speed, out speed);
        if (inDialogue.options.Count > 0) { StartCoroutine(ReadInText(speed, inDialogue.dialogue, inDialogue)); }
        else { StartCoroutine(ReadInText(speed, inDialogue.dialogue)); }
    }
    public IEnumerator ReadInText(float speed, string dialogue)
    {
        readInComplete = false;
        textDialogueBox.text = "";
        for (int i = 0; i < dialogue.Length; i++)
        {
            textDialogueBox.text += dialogue[i];
            yield return new WaitForSeconds(speed);
        }
        readInComplete = true;
    }
    public IEnumerator ReadInText(float speed, string dialogue, DialogueRevised inDialogue)
    {
        readInComplete = false;
        textDialogueBox.text = "";
        for (int i = 0; i < dialogue.Length; i++)
        {
            textDialogueBox.text += dialogue[i];
            yield return new WaitForSeconds(speed);
        }
        readInComplete = true;
        if (inDialogue.options.Count > 0)
        {
            canContinue = false;
            MassDialogueManager.Instance.DisplayButtons(inDialogue.options, this);
        }
       
    }



    private void OnDestroy()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            dialogues[i].startEvents = null;
            dialogues[i].endEvents = null;
        }
        NotifyDialogueComplete = null;

    }

    public void TransitionTo(DialogueManager dialogueManager)
    {
        
        dialogueManager.gameObject.SetActive(true);
        canContinue = false;
        dialogueManager.returnTo = dialogueManager;
        this.gameObject.SetActive(false);
    }

    
    
   

}

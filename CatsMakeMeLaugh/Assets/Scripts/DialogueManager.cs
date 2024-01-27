using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[Serializable]
public class dialogue
{
    [Header("Text")]
    public string speakerName;
    public string txt;
    public Color boxColor = Color.white;
    public Enums.DialogueSpeed dialogueSpeed;

    [Header("Sprites")]
    public List<Sprite> sprites;
    public Emote currentEmote;

    [Header("Special Events")]
    public Events events;
    public List<Options> dialogueOptions;
}
[Serializable]
public class Options
{
    public string OptionName;
    public DialogueManager TransitionToBranch;
    public bool permanentTransition;

    
}

[Serializable]
public class Events
{
    public UnityEvent specialEventOpen;
    public UnityEvent specialEventDialogueEnd;
}


public class DialogueManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    TextMeshProUGUI textBoxText;
    [SerializeField]
    TextMeshProUGUI speakerNameTextBox;

    [Header("Images")]
    [SerializeField]
    Image textBoxImage;
    [SerializeField]
    Image speakerBoxImage;
    [SerializeField]
    Image spriteLocation;


    [Header("Speed")]
    [SerializeField]
    float normalSpeed = 0.1f;
    [SerializeField]
    float slowSpeed = 0.1f;
    [SerializeField]
    float verySlowSpeed = 0.3f;
    [SerializeField]
    float fastSpeed = 0.05f;
    [SerializeField]
    float veryFastSpeed = 0.06f;

    //public Image prefab;

    [Header("Bools")]
    public bool canContinue = true;
    public bool readInProgress = false;
    public bool destroyOnComplete = true;
    public bool specialEventOnNext = false;
    public bool lastDialogue = false;
    public bool prematureDestroy = false;
    public bool returning = false;

    [Header("Input")]
    public KeyCode proceed = KeyCode.Return;

    [Header("Lists/Dictionaries")]
    public List<dialogue> dialogues = new List<dialogue>();
    Dictionary<Enums.DialogueSpeed, float> associatedDialogueSpeeds = new Dictionary<Enums.DialogueSpeed, float>();
    public List<Image> spriteSlots = new List<Image>();
    public List<OptionsButton> optionSlots = new List<OptionsButton>();
    int currentIndex = 0;
    public DialogueManager returnTo = null;


    









    // Start is called before the first frame update

    private void OnEnable()
    {
        speakerBoxImage.gameObject.SetActive(true);
        textBoxImage.gameObject.SetActive(true);
        if (!returning) { currentIndex = 0; }
        if (currentIndex < dialogues.Count) { ProceedNext(); }

    }
    void Start()
    {
        associatedDialogueSpeeds.Add(Enums.DialogueSpeed.NORMAL, normalSpeed);
        associatedDialogueSpeeds.Add(Enums.DialogueSpeed.SLOW, slowSpeed);
        associatedDialogueSpeeds.Add(Enums.DialogueSpeed.VERYSLOW, verySlowSpeed);
        associatedDialogueSpeeds.Add(Enums.DialogueSpeed.FAST, fastSpeed);
        associatedDialogueSpeeds.Add(Enums.DialogueSpeed.VERYFAST, veryFastSpeed);

        canContinue = true;
        //ProceedNext();


    }

    // Update is called once per frame
    void Update()
    {
        if (canContinue && Input.GetKeyDown(proceed))
        {
            if (readInProgress) { StopAllCoroutines(); }
            if (!lastDialogue)
            {
                if (specialEventOnNext)
                {
                    if (dialogues[currentIndex - 1] != null && dialogues[currentIndex - 1].events.specialEventDialogueEnd.GetPersistentEventCount() > 0)
                        dialogues[currentIndex - 1].events.specialEventDialogueEnd.Invoke();
                    //Unsubscribe
                    dialogues[currentIndex - 1].events.specialEventDialogueEnd = null;
                }
                ProceedNext();
            }
            else
            {
                if (specialEventOnNext)
                {
                    dialogues[currentIndex].events.specialEventDialogueEnd.Invoke();
                    //Unsubscribe
                    dialogues[currentIndex].events.specialEventDialogueEnd = null;
                }
                EndResponse();
            }

        }
    }

    public void ProceedNext()
    {
        DisplayInfo(dialogues[currentIndex]);
        currentIndex++;
        if (currentIndex > (dialogues.Count - 1))
        {
            lastDialogue = true;
            //Bring it back into scope for safety
            currentIndex--;
        }
    }

    public void EndResponse()
    {
        speakerBoxImage.gameObject.SetActive(false);
        textBoxImage.gameObject.SetActive(false);
        DisableAllSpriteSlots();
        if (returnTo != null)
        {
            ReturnTo();
        }
        else
        {
            if (destroyOnComplete) { GameObject.Destroy(this.gameObject); }
            else { canContinue = false; };
        }

    }
    public void DisplayInfo(dialogue inDialogue)
    {
        ////Speaker Box
        //If there is a designated speaker, then enable the box & set it up, otherwise, disable the box if it is active.
        if (!string.IsNullOrEmpty(inDialogue.speakerName))
        {
            speakerBoxImage.color = inDialogue.boxColor;
            speakerNameTextBox.text = inDialogue.speakerName;
            speakerBoxImage.gameObject.SetActive(true);
        }
        else if (speakerBoxImage.gameObject.activeSelf) //If the speaker box is already active, then disable it.
        {
            speakerBoxImage.gameObject.SetActive(false);
        }

        ////Sprites
        //If there is sprites, then put them into their slots.
        if (inDialogue.sprites != null)
        {
            HandleSpritesSlots(inDialogue.sprites);
        }
        else //If there is no sprites, disable all the slots.
        {
            DisableAllSpriteSlots();
        }

        ////Events
        ///Handle Open
        if (inDialogue.events.specialEventOpen != null && inDialogue.events.specialEventOpen.GetPersistentEventCount() > 0)
        {
            inDialogue.events.specialEventOpen.Invoke();
            //Unsubscribe
            inDialogue.events.specialEventOpen = null;
        }
        ///Handle Close
        if (inDialogue.events.specialEventDialogueEnd != null && inDialogue.events.specialEventDialogueEnd.GetPersistentEventCount() > 0) { specialEventOnNext = true; }
        else { specialEventOnNext = false; }

        ////Textbox
        //Just in case, make sure there actually is dialogue.
        if (!string.IsNullOrEmpty(inDialogue.txt))
        {
            //Set color
            textBoxImage.color = inDialogue.boxColor;
            //Enable
            textBoxImage.gameObject.SetActive(true);
            //Make sure the text from the box is cleared.
            textBoxText.text = "";
            float speed = 0f;
            associatedDialogueSpeeds.TryGetValue(inDialogue.dialogueSpeed, out speed);
            if (inDialogue.dialogueOptions != null && inDialogue.dialogueOptions.Count > 0)
            {
                StartCoroutine(ReadText(speed, inDialogue.txt, currentIndex));
            }
            else if (inDialogue.dialogueOptions != null)
            {
                DisableAllOptionSlots();
                //Start Coroutine
                StartCoroutine(ReadText(speed, inDialogue.txt));
            }
        }
        else
        {
            //There isn't textbox text so disable
            textBoxImage.gameObject.SetActive(false);
        }


    }

    IEnumerator ReadText(float speed, string text)
    {
        readInProgress = true;
        //Read in gradually
        for (int i = 0; i < text.Length; i++)
        {
            textBoxText.text += text[i];
            yield return new WaitForSeconds(speed);
        }
        readInProgress = false;
    }
    IEnumerator ReadText(float speed, string text, int index)
    {
        readInProgress = true;
        //Read in gradually
        for (int i = 0; i < text.Length; i++)
        {
            textBoxText.text += text[i];
            yield return new WaitForSeconds(speed);
        }
        readInProgress = false;
        HandleOptionSlots(dialogues[index].dialogueOptions, index);
        canContinue = false;
    }

    public void HandleSpritesSlots(List<Sprite> inSprites)
    {
        for (int i = 0; i < spriteSlots.Count; i++)
        {
            if (i >= inSprites.Count) { spriteSlots[i].gameObject.SetActive(false); }
            else
            {
                spriteSlots[i].sprite = inSprites[i];
                spriteSlots[i].gameObject.SetActive(true);
            }
        }
    }

    public void DisableAllSpriteSlots()
    {
        for (int i = 0; i < spriteSlots.Count; i++)
        {
            spriteSlots[i].gameObject.SetActive(false);
        }
    }

    public void EventTest1()
    {
        Debug.Log("Test 1 complete");
    }
    public void EventTest2()
    {
        Debug.Log("Test 2 complete");
    }
    public void EventTest3()
    {
        Debug.Log("Test 2 complete");
    }
    public void EventTest4()
    {
        Debug.Log("Test 2 complete");
    }

    public void affection(int affection)
    {
        Debug.Log("Test " + affection);
    }


    public void OptionsButtonPressed(int optionsIndex, int dialogueIndex)
    {
        if (dialogues[dialogueIndex].dialogueOptions[optionsIndex].permanentTransition)
        {
            PermanentTransition(dialogues[dialogueIndex].dialogueOptions[optionsIndex].TransitionToBranch);
           
        }
        else
        {
            TemporaryTransition(dialogues[dialogueIndex].dialogueOptions[optionsIndex].TransitionToBranch);
        }
        
    }
    public void PermanentTransition(DialogueManager inDialogueManager)
    {
        inDialogueManager.gameObject.SetActive(true);
        if (currentIndex < dialogues.Count) { prematureDestroy = true; }
        GameObject.Destroy(this.gameObject);
    }

    public void TemporaryTransition(DialogueManager inDialogueManager)
    {
        returning = true;
        inDialogueManager.returnTo = this;
        inDialogueManager.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

    }

    public void HandleOptionSlots(List<Options> inOptions, int inIndex)
    {
       for (int i = 0; i < optionSlots.Count; i++)
        {
            if (i >= inOptions.Count) { optionSlots[i].gameObject.SetActive(false); }
            else
            {
                optionSlots[i].btnText.text = inOptions[i].OptionName;
                optionSlots[i].index = i;
                optionSlots[i].instance = this;
                optionSlots[i].dialogueIndex = inIndex;
                optionSlots[i].gameObject.SetActive(true);
            }
        }
    }
    public void DisableAllOptionSlots()
    {
        for(int i = 0; i < optionSlots.Count; i++)
        {
            optionSlots[i].gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        
        if (prematureDestroy)
        {
            //Unsubscribe from the remaining events
            HandleMassUnsubscribe(currentIndex);
            
        }
        
    }

    private void OnApplicationQuit()
    {
        //Unsubscribe from remaining events c:
        HandleMassUnsubscribe(0);
    }

    public void HandleMassUnsubscribe(int startVal)
    {
        for (int i = startVal; i < dialogues.Count; i++)
        {
            if (dialogues[i].events != null)
            {
                HandlePrematureUnsubscribe(ref dialogues[i].events);
            }
        }
    }
    public void HandlePrematureUnsubscribe(ref Events inEvents)
    {
        inEvents.specialEventDialogueEnd = null;
        inEvents.specialEventOpen = null;
    }

    public void ReturnTo()
    {
        if (returnTo != null)
        {
            returnTo.gameObject.SetActive(true);
            returnTo.canContinue = true;
            if (destroyOnComplete && returnTo != null) { GameObject.Destroy(this.gameObject); }
            else if (!destroyOnComplete && returnTo != null) { this.gameObject.SetActive(false); }
   
        }
    }


}

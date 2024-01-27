using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[Serializable]
public class dialogue
{
    public string speakerName;
    public string txt;
    public Color boxColor = Color.white;
    public Enums.DialogueSpeed dialogueSpeed;
    public List<Sprite> sprites;

    [Header("EmoteEffects")]
    public Enums.Emotes emote = Enums.Emotes.NONE;
    //public Enums.EmoteEffect emoteEffect = null;

    [Header("Special Events")]
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

    [Header("Input")]
    public KeyCode proceed = KeyCode.Return;

    [Header("Lists/Dictionaries")]
    public List<dialogue> dialogues = new List<dialogue>();
    Dictionary<Enums.DialogueSpeed, float> associatedDialogueSpeeds = new Dictionary<Enums.DialogueSpeed, float>();
    public List<Image> spriteSlots = new List<Image>();
    int currentIndex = 0;









    // Start is called before the first frame update

    private void OnEnable()
    {
        speakerBoxImage.gameObject.SetActive(true);
        textBoxImage.gameObject.SetActive(true);
        currentIndex = 0;
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
        if (canContinue)
        {
            
            if (Input.GetKeyDown(proceed))
            {
                if (readInProgress) { StopAllCoroutines(); }
                if (!lastDialogue)
                {
                    if (specialEventOnNext)
                    {
                        if (dialogues[currentIndex -1] != null && dialogues[currentIndex - 1].specialEventDialogueEnd.GetPersistentEventCount() > 0)
                        dialogues[currentIndex -1].specialEventDialogueEnd.Invoke();
                        //Unsubscribe
                        dialogues[currentIndex - 1].specialEventDialogueEnd = null;
                    }
                    ProceedNext();
                }
                else 
                { 
                    if (specialEventOnNext)
                    {
                        dialogues[currentIndex].specialEventDialogueEnd.Invoke();
                        //Unsubscribe
                        dialogues[currentIndex].specialEventDialogueEnd = null;
                    }
                    EndResponse();
                }
            }
        }
    }

    public void ProceedNext()
    {
        DisplayInfo(dialogues[currentIndex]);
        currentIndex++;
        if (currentIndex > (dialogues.Count-1)) 
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
        if (destroyOnComplete) { GameObject.Destroy(this.gameObject); }
        else { canContinue = false; };

    }
    public void DisplayInfo(dialogue inDialogue)
    {
        //Set color
        textBoxImage.color = inDialogue.boxColor;

        //If there is a designated speaker, then enable the box & set it up, otherwise, disable the box if it is active.
        if (!string.IsNullOrEmpty(inDialogue.speakerName))
        {
            speakerBoxImage.color = inDialogue.boxColor;
            speakerNameTextBox.text = inDialogue.speakerName;
            speakerBoxImage.gameObject.SetActive(true);
        }
        else if (speakerBoxImage.gameObject.activeSelf) 
        {
            speakerBoxImage.gameObject.SetActive(false);
        }

        if (inDialogue.sprites != null)
        {
            HandleSpritesSlots(inDialogue.sprites);
        }
        else
        {
            DisableAllSpriteSlots();
        }
        
        //Handle Special Events(Like displaying three at a time for example
        if (inDialogue.specialEventOpen != null && inDialogue.specialEventOpen.GetPersistentEventCount() > 0)
        {
            inDialogue.specialEventOpen.Invoke();
            //Unsubscribe
            inDialogue.specialEventOpen = null;
        }
        if (inDialogue.specialEventDialogueEnd != null && inDialogue.specialEventDialogueEnd.GetPersistentEventCount() > 0) { specialEventOnNext = true; }
        else { specialEventOnNext = false; }


        //Just in case, make sure there actually is dialogue.
        if (!string.IsNullOrEmpty(inDialogue.txt))
        {
            //Make sure the text from the box is cleared.
            textBoxText.text = "";
            float speed = 0f;
            associatedDialogueSpeeds.TryGetValue(inDialogue.dialogueSpeed, out speed);
            //Start Coroutine
            StartCoroutine(ReadText(speed, inDialogue.txt));
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






}

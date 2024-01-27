using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public struct dialogue
{
    public string txt;
    public string speakerName;
    public Color boxColor;
    public Enums.DialogueSpeed dialogueSpeed;
}


public class DialogueManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    TextMeshProUGUI textBoxText;
    [SerializeField]
    TextMeshProUGUI speakerNameTextBox;
    [SerializeField]
    Image textBoxImage;
    [SerializeField]
    Image speakerBoxImage;


    [Header("Speed")]
    [SerializeField]
    float normalSpeed = 0.3f;
    [SerializeField]
    float slowSpeed = 0.2f;
    [SerializeField]
    float verySlowSpeed = 0.1f;
    [SerializeField]
    float fastSpeed = 0.5f;
    [SerializeField]
    float veryFastSpeed = 0.6f;

    [Header("Bools")]
    public bool canContinue = true;
    public bool readInProgress = false;
    public bool destroyOnComplete = true;
    public bool lastDialogue = false;

    [Header("Input")]
    public KeyCode proceed = KeyCode.Return;

    [Header("Lists/Dictionaries")]
    [SerializeField]
    List<dialogue> dialogues = new List<dialogue>();
    Dictionary<Enums.DialogueSpeed, float> associatedDialogueSpeeds = new Dictionary<Enums.DialogueSpeed, float>();
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



    }

    // Update is called once per frame
    void Update()
    {
        if (canContinue)
        {
            if (Input.GetKeyDown(proceed))
            {
                if (readInProgress) { StopAllCoroutines(); }
                if (!lastDialogue) { ProceedNext(); }
                else { EndResponse(); }
            }
        }
    }

    public void ProceedNext()
    {
        DisplayInfo(dialogues[currentIndex]);
        currentIndex++;
        if (currentIndex > dialogues.Count) 
        { 
            lastDialogue = true;
            //Bring it back into scope... just in case.
            currentIndex--;
        }
    }

    public void EndResponse()
    {
        speakerBoxImage.gameObject.SetActive(false);
        textBoxImage.gameObject.SetActive(false);
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
            speakerNameTextBox.gameObject.SetActive(true);
        }
        else if (speakerBoxImage.gameObject.activeSelf) { speakerBoxImage.gameObject.SetActive(false); }
        
        //Just in case, make sure there actually is dialogue.
        if (!string.IsNullOrEmpty(inDialogue.txt))
        {
            //Make sure the text from the box is cleared.
            textBoxText.text = "";
            float speed = 0.1f;
            associatedDialogueSpeeds.TryGetValue(inDialogue.dialogueSpeed, out speed);
            //Start Coroutine
            ReadText(speed, inDialogue.txt);
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


    
    

}

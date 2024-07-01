using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

[Serializable]
public class DialogueV3
{
    [Header("Character")]
    public string speakerName;
    public string dialogueText;
    public List<OnScreen> onScreen;
    public Enums.DialogueSpeed speed = Enums.DialogueSpeed.NORMAL;
    public bool canContinueAfter = true;
    

    [Header("Location")]
    public bool changeLocation;
    public Enums.Locations newLocation = Enums.Locations.NONE;

    [Header("Audio")]
    public AudioClip bgmAudioClip;
    public AudioClip sfxAudioClip;

    [Header("Options")]
    public bool showAfterReadIn = true;
    public List<OptionsV3> options = new List<OptionsV3>();

    [Header("Events")]
    public UnityEvent startEvents;
    public UnityEvent endEvents;

}

[Serializable]
public class OnScreen
{
    public string accessKey;
    public Enums.Actions actions = Enums.Actions.SIT;
    public Enums.Emotes emote = Enums.Emotes.NONE;


}
[Serializable]
public class OptionsV3
{
    public string optionName;
    public UnityEvent optionEvent;
   
}
public class DialogueManagerV3 : MonoBehaviour
{
    [Header("Dialogue")]
    public List<DialogueV3> dialogues = new List<DialogueV3>();

    [Header("UI")]
    //Speaker
    [SerializeField]
    TextMeshProUGUI speaker;
    [SerializeField]
    GameObject speakerBox;
    [SerializeField]
    TextMeshProUGUI dialogue;
    


    [Header("Audio")]
    [SerializeField]
    AudioSource bgmAudioSource;
    [SerializeField]
    AudioSource sfxAudioSource;

    
    bool canContinue = true;
    bool readInComplete = false;


    // Start is called before the first frame update
    void Start()
    {


        //Test
        Debug.Log(Parse("{Cat1} testing {3}"));


    }    // Update is called once per frame
    void Update()
    {

    }

    public void ReadIn(DialogueV3 inDialogue)
    {
        //Audio
        if (bgmAudioSource != null && inDialogue.bgmAudioClip) { bgmAudioSource.clip = inDialogue.bgmAudioClip; bgmAudioSource.Play(); }
        if (sfxAudioSource != null & inDialogue.sfxAudioClip != null) { sfxAudioSource.clip = inDialogue.sfxAudioClip; sfxAudioSource.Play(); }    

        //Location
        if (inDialogue.changeLocation) { MassDialogueManagerV3.Instance?.ChangeLocation(inDialogue.newLocation); }

        //Event
        inDialogue.startEvents?.Invoke();

        //Character
        MassDialogueManagerV3.Instance?.HandleVisible(inDialogue.onScreen);

        //Options
        if (!inDialogue.showAfterReadIn)
        {
            if (inDialogue.options.Count > 0)
            {
                MassDialogueManagerV3.Instance?.HandleOptions(inDialogue.options); 
            }
        }

        //Read
        if (inDialogue.speakerName != "")
        {
            speaker.text = Parse(inDialogue.speakerName);
            speakerBox?.gameObject.SetActive(true);
        }
        else {speakerBox.gameObject.SetActive(false); }
        StartCoroutine(Read(Parse(inDialogue.dialogueText),inDialogue));
    }

    IEnumerator Read(string dialogueParsed, DialogueV3 inDialogue)
    {
        //Prep
        readInComplete = false;
        dialogue.text = "";
        float speed = MassDialogueManagerV3.Instance.GetSpeed(inDialogue.speed);

        //Read to the screen
        for (int i = 0; i < dialogueParsed.Length; i++)
        {
            dialogue.text += dialogueParsed[i];
            yield return new WaitForSeconds(speed);
        }
        
        //Final steps
        if (inDialogue.showAfterReadIn) {MassDialogueManagerV3.Instance?.HandleOptions(inDialogue.options); }
        inDialogue.endEvents?.Invoke();
        readInComplete = true;

       
    }
    public string Parse(string val)
    {
        if (MassDialogueManagerV3.Instance == null) { Debug.LogWarning("MassDialogueManagerV3 instance is null, make sure one is in the scene if you want to have it parse."); return val; }
        
        string replaced = val;
        List<int> opening = new List<int>();
        List<int> closing = new List<int>();
        bool waitingNext = false;
        
       
        if (val.IndexOf('{') != -1 && val.IndexOf('}') != -1)
        {

            for (int i = 0; i < val.Length; i++)
            {
                if (!waitingNext)
                {
                    if (val[i] == '{')
                    {
                        if (i > 0)
                        {
                            if (val[i - 1] == '/')
                            {
                                replaced.Remove(i - 1, 1);
                                continue;
                            }
                        }
                        opening.Add(i);
                        waitingNext = true;


                    }
                }
                else
                {

                    if (val[i] == '}')
                    {
                        if (i > 0)
                        {
                            if (val[i - 1] == '/')
                            {
                                replaced.Remove(i - 1, 1);
                                continue;
                            }
                        }
                        closing.Add(i);
                        waitingNext = false;


                    }
                }

            }
        }

        if (waitingNext)
        {
            opening.RemoveAt(opening.Count - 1);
            waitingNext = false;
        }

        if (opening.Count > 0)
        {
            for (int i = 0; i < opening.Count; i++)
            {
                string name = val.Substring(opening[i], closing[i]+1 - opening[i]);
                replaced = replaced.Replace(name, MassDialogueManagerV3.Instance.GetProperName(name));

            }
        }

        return replaced;

    }

    

}

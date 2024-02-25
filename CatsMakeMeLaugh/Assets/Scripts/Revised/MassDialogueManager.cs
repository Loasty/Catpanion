using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MassDialogueManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    GameObject buttonsLocation;
    [SerializeField]
    List<GameObject> buttonsOnScreen = new List<GameObject>();


    [Header("Speed")]
    [SerializeField]
    private float normalSpeed = 0.01f;
    [SerializeField]
    private float slowSpeed = 0.3f;
    [SerializeField]
    private float verySlowSpeed = 0.5f;
    [SerializeField]
    private float fastSpeed = 0.001f;
    [SerializeField]
    private float veryFastSpeed = 0.001f;
     [SerializeField]
    public Dictionary<Enums.DialogueSpeed, float> dialogueSpeeds = new Dictionary<Enums.DialogueSpeed, float>();

    
    private static MassDialogueManager instance;
    public static MassDialogueManager Instance { get { return instance; } }
    
    

    


    void Awake()
    {
        instance = this;
        dialogueSpeeds.Add(Enums.DialogueSpeed.NORMAL, normalSpeed);
        dialogueSpeeds.Add(Enums.DialogueSpeed.SLOW, slowSpeed);
        dialogueSpeeds.Add(Enums.DialogueSpeed.VERYSLOW, verySlowSpeed);
        dialogueSpeeds.Add(Enums.DialogueSpeed.FAST, fastSpeed);
        dialogueSpeeds.Add(Enums.DialogueSpeed.VERYFAST, veryFastSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayButtons(List<OptionsRevised> options, DialogueManagerRevised requester)
    {
        for (int i = 0; i < options.Count; i++)
        {
            GameObject btn = GameObject.Instantiate(buttonPrefab, buttonsLocation.transform);
            RevisedDialogueButton diaButton = btn.GetComponent<RevisedDialogueButton>();
            //Fill in info
            diaButton.btnText.text = options[i].dialogueName;
            diaButton.transitionTo = options[i].transitonTo;
            diaButton.impact = options[i].impact;
            diaButton.from = requester;
            if (options[i].returnToHere)
            {
                diaButton.transitionTo.returnTo = requester;
            }

            //Add To Screen
            buttonsOnScreen.Add(btn);


            

        }
    }
    
    public void ClearButtons()
    {
        for (int i = 0; i < buttonsOnScreen.Count; i++)
        {
            GameObject.Destroy(buttonsOnScreen[i]);
        }
        buttonsOnScreen.Clear();
    }

    public void HandleButtonDown(OptionsRevised inRevised, DialogueManager requester)
    {
        requester.gameObject.SetActive(false);
        inRevised.transitonTo.gameObject.SetActive(true);
    }
    public void HandleButtonDown(RevisedDialogueButton inButton)
    {
        inButton.from.gameObject.SetActive(false);
        inButton.transitionTo.gameObject.SetActive(true);
        ClearButtons();
    }

    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameCat : MonoBehaviour
{
    // Start is called before the first frame update
    int gagCount = 0;
    int maxGagCount = 2;
    [SerializeField]
    List<string> gagText = new List<string>();
    int gagCounter = 0;

    

    string noText = "Brevity is the soul of wit, however, methinks you overdid it! Name this poor cat!";
    public GameObject dialogueStuff;
    public DialogueManager dialogueNext;
    public DialogueManager dialogueWhite;
    public DialogueManager dialogueBlack;
    public DialogueManager dialogueCalico;
    public DialogueManager dialogueTabby;


    [Header("UI")]
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    TextMeshProUGUI textBox;
    [SerializeField]
    Button button;

    [SerializeField]
    float textTimeNormal = 0.06f;
    [SerializeField]
    float disappointedSpeed = 0.3f;

    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UIOff()
    {
        textBox.gameObject.SetActive(false);
        inputField.interactable = false;
        inputField.gameObject.SetActive(false);
        button.interactable = false;
        button.gameObject.SetActive(false);
        
        
    }
    public void Submit()
    {

        if (string.IsNullOrEmpty(inputField.text)) { StartCoroutine(ReadText(disappointedSpeed, noText)); }
        else
        {
            if (gagCount >= gagText.Count)
            {
                GameData.Instance.ChangeCatName(inputField.text);
                UIOff();
                dialogueStuff.SetActive(true);
                dialogueNext.gameObject.SetActive(true);

            }
            else
            {
                StartCoroutine(ReadText(textTimeNormal, gagText[gagCount]));
                gagCount++;

            }
        }
    }


    IEnumerator ReadText(float speed, string text)
    {
        textBox.text = "";
        button.interactable = false;
        inputField.interactable = false;
        //Read in gradually
        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];
            yield return new WaitForSeconds(speed);
        }
        inputField.text = "";
        inputField.interactable = true;
        button.interactable = true;
      
    }
    public void DialogueAfter()
    {
        switch(GameData.Instance.savedCat.type)
        {
            case Enums.CatType.WHITE:
                dialogueWhite.gameObject.SetActive(true);
                break;
            case Enums.CatType.BLACK:
                dialogueBlack.gameObject.SetActive(true);
                break;
            case Enums.CatType.CALICO:
                dialogueCalico.gameObject.SetActive(true);
                break;
            case Enums.CatType.TABBY:
                dialogueTabby.gameObject.SetActive(true);
                break;
          
        }
    }

    

  
}

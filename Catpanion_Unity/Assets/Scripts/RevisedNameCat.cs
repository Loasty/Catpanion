using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevisedNameCat : MonoBehaviour
{
    // Start is called before the first frame update
    int gagCount = 0;
    //int maxGagCount = 2;
    [SerializeField]
    List<string> gagText = new List<string>();
    //int gagCounter = 0;
    public bool reading = false;



    string noText = "Brevity is the soul of wit, however, methinks you overdid it! Name this poor cat!";
    public GameObject dialogueStuff;
    public DialogueManagerRevised dialogueNext;
    public DialogueManagerRevised dialogueWhite;
    public DialogueManagerRevised dialogueBlack;
    public DialogueManagerRevised dialogueCalico;
    public DialogueManagerRevised dialogueTabby;
    //public DialogueManagerRevised returnTo;
    

    public Character characterCat;
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

    private void OnEnable()
    {
        //BeginNameGag();
    }
    public void SetCat(Enums.CatType cat)
    {
        characterCat = CharacterManager.Instance.GetCharacter(cat);
    }
   
    public void SetCatTabby()
    {
        characterCat = CharacterManager.Instance.GetCharacter(Enums.CatType.TABBY);
    }
    public void SetCatCalico()
    {
        characterCat = CharacterManager.Instance.GetCharacter(Enums.CatType.CALICO);
    }
   
    public void SetCatWhite()
    {
        characterCat = CharacterManager.Instance.GetCharacter(Enums.CatType.WHITE);
    }
    public void SetCatBlack()
    {
        characterCat = CharacterManager.Instance.GetCharacter(Enums.CatType.BLACK);
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
                
                characterCat.speakerName = inputField.text;
                GameData.Instance.AcquiredCat(characterCat);
                UIOff();
                dialogueStuff.SetActive(true);
                DialogueAfter();

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
        if (!reading)
        {
            reading = true;
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
            reading = false;
        }

    }
    public void DialogueAfter()
    {

        switch (characterCat.cat)
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

    public void ReturnToMainMenu()
    {
        gameObject.TryGetComponent(out SceneLoader sceneLoader);
        if (sceneLoader == null)
        {
            sceneLoader = gameObject.AddComponent<SceneLoader>();
        }
        sceneLoader.nextSceneName = "MainMenu";
        sceneLoader.unloadPreviousScene = true;
        sceneLoader.LoadScene();
    }
    
    public void BeginNameGag()
    {
        dialogueStuff.SetActive(false);
        UIOn();
        Begin();

    }

    public void UIOn()
    {
        textBox.gameObject.SetActive(true);
        inputField.interactable = true;
        inputField.gameObject.SetActive(true);
        button.interactable = true;
        button.gameObject.SetActive(true);


    }
    public void Begin()
    {
        switch (characterCat.cat)
        {
            case Enums.CatType.WHITE:
                StartCoroutine(ReadText(textTimeNormal, "You have won the affections of the white cat, please, give it a name! :)"));
                break;
            case Enums.CatType.BLACK:
                StartCoroutine(ReadText(textTimeNormal, "You have won the affections of the black cat, please, give it a name! :)"));
                break;
            case Enums.CatType.CALICO:
                StartCoroutine(ReadText(textTimeNormal, "You have won the affections of the calico cat, please, give it a name! :)"));
                break;
            case Enums.CatType.TABBY:
                StartCoroutine(ReadText(textTimeNormal, "You have won the affections of the tabby cat, please, give it a name! :)"));
                break;

        }

    }
   
}

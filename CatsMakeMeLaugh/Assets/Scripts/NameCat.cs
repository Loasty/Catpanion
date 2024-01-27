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

    [Header("UI")]
    [SerializeField]
    InputField inputField;
    [SerializeField]
    TextMeshProUGUI textBox;

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
    public void Submit()
    {
        if (string.IsNullOrEmpty(inputField.text)) { StartCoroutine(ReadText(disappointedSpeed, noText)); }
        else
        {
            StartCoroutine(ReadText(textTimeNormal, gagText[gagCount]));
            
        }
    }

    IEnumerator ReadText(float speed, string text)
    {
        textBox.text = "";
        //Read in gradually
        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];
            yield return new WaitForSeconds(speed);
        }
      
    }

}

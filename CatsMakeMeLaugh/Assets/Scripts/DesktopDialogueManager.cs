using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesktopDialogueManager : MonoBehaviour
{

    public List<string> dialogues;

    public GameObject textboxImage;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI textBox;
    public float waitTime;
 
    public float readInSpeed = 0.06f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaySomething()
    {
        StartCoroutine(ReadText(readInSpeed, RandomDialogue()));
    }
    public string RandomDialogue()
    {
        return dialogues[Random.Range(0, dialogues.Count)];
    }
    public IEnumerator ReadText(float speed, string text)
    {
        
        textboxImage.gameObject.SetActive(true);
        textBox.text = "";
        //Read in gradually
        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];
            yield return new WaitForSeconds(speed);
        }

        yield return new WaitForSeconds(waitTime);
        textboxImage.gameObject.SetActive(false);
        textBox.text = "";
        
    }

 
}

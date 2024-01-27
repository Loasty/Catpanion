using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    // Start is called before the first frame update

    public int index;
    public TextMeshProUGUI btnText;
    public DialogueManager instance;
    public int dialogueIndex;
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonDown()
    {
        instance.OptionsButtonPressed(index, dialogueIndex);
    }
}

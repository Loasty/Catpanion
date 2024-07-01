using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OptionsButtonV3 : MonoBehaviour
{
    public UnityEvent optionEvent;
    public TextMeshProUGUI buttonText;
  

    public void ButtonPressed()
    {
        optionEvent?.Invoke();
        MassDialogueManagerV3.Instance?.DestroyAllButtons();
        
    }
}

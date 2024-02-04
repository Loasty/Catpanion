using JetBrains.Annotations;
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
    public Enums.CatType type = Enums.CatType.NONE;
    public int relationshipImpact = 0;


    private void Awake()
    {
        if (AffectionSystem.Instance == null)
        {
            AffectionSystem inst = FindObjectOfType<AffectionSystem>();
            if (inst == null) { Debug.LogError("There needs to be at least ONE Affection System in the Scene!!"); }
            else
            {
                inst.SetInstance();
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonDown()
    {
        if (type != Enums.CatType.NONE) { instance.AffectAffection(relationshipImpact, type); }
        instance.OptionsButtonPressed(index, dialogueIndex);
       
    }
}

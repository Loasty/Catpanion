using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[Serializable]
public class AffectType
{
    public Enums.CatType affectCat = Enums.CatType.NONE;
    public float affectionImpact = 0;
}

public class RevisedDialogueButton : MonoBehaviour
{
    public TextMeshProUGUI btnText;
    public DialogueManagerRevised transitionTo;
    public DialogueManagerRevised from;
    public List<AffectType> impact;

    public void ButtonPressed()
    {
        foreach (AffectType affect in impact)
        {
            CharacterManager.Instance.HandleAffectionImpact(affect);
        }
        MassDialogueManager.Instance.HandleButtonDown(this);
    }

}

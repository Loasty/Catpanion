using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

[RequireComponent(typeof(Button)), RequireComponent(typeof(CanvasGroup))]
public class DesktopMenuItem : MonoBehaviour
{
    [HideInInspector] public MenuOptions_Main menuType;
    [HideInInspector] public Button button;
    [HideInInspector] public CanvasGroup canvasGroup;

    [HideInInspector] public MenuOptions_Main subMenuType;
    [HideInInspector] public MenuOptions_Cats menuType_Cats;
    [HideInInspector] public MenuOptions_Closet menuType_Closet;
    [HideInInspector] public MenuOptions_Visit menuType_Visit;
    [HideInInspector] public MenuOptions_Options menuType_Options;

    public Transform startingPos;
    public Transform endingPos;

    private void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        InitializeButton();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckToSubscribe());
    }

    private void OnDisable()
    {
        DesktopMenuController.Instance.UnSubscribeMenuItem(this);
    }

    void InitializeButton()
    {
        if(menuType == MenuOptions_Main.Main && subMenuType == MenuOptions_Main.Main)
        {
            EnableButton();
        } 
        else
        {
            DisableButton();
        }
    }

    public void EnableButton()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DisableButton()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ToggleButton()
    {
        if (canvasGroup.interactable) { DisableButton(); } else { EnableButton(); }
    }

    IEnumerator CheckToSubscribe()
    {
        if (DesktopMenuController.Instance == null) { yield return null; }

        bool foundSelf = false;

        if (DesktopMenuController.Instance.menuItems.Count > 0)
        {
            foreach (DesktopMenuItem item in DesktopMenuController.Instance.menuItems)
            {
                if (item == this) { foundSelf = true; break; }
            }
        }
        else
        {
            foundSelf = false;
        }

        if (!foundSelf) { DesktopMenuController.Instance.SubscribeMenuItem(this); }
    }
}

//Custom inspector starts here
#if UNITY_EDITOR
[CustomEditor(typeof(DesktopMenuItem)), Serializable]
public class ExpandedDesktopMenuItem : Editor
{

    public override void OnInspectorGUI()
    {
        
        //cast target
        var enumScript = target as DesktopMenuItem;
        //Enum drop down
        EditorGUILayout.LabelField("Main Button Type", EditorStyles.boldLabel);
        enumScript.menuType = (MenuOptions_Main)EditorGUILayout.EnumPopup(enumScript.menuType);
        //switch statement for different variables
        switch (enumScript.menuType)
        {
            case MenuOptions_Main.Main:
                EditorGUILayout.LabelField("Sub Button Type", EditorStyles.label);
                enumScript.subMenuType = (MenuOptions_Main)EditorGUILayout.EnumPopup(enumScript.subMenuType);
                break;

            case MenuOptions_Main.Cats:

                EditorGUILayout.LabelField("Sub Button Type", EditorStyles.label);
                enumScript.menuType_Cats = (MenuOptions_Cats)EditorGUILayout.EnumPopup(enumScript.menuType_Cats);

                break;

            case MenuOptions_Main.Closet:

                break;

            case MenuOptions_Main.Visit:
                EditorGUILayout.LabelField("Sub Button Type", EditorStyles.label);
                enumScript.menuType_Visit = (MenuOptions_Visit)EditorGUILayout.EnumPopup(enumScript.menuType_Visit);
                break;

            case MenuOptions_Main.Options:
                EditorGUILayout.LabelField("Sub Button Type", EditorStyles.label);
                enumScript.menuType_Options = (MenuOptions_Options)EditorGUILayout.EnumPopup(enumScript.menuType_Options);
                break;
                
        }//end switch

        base.OnInspectorGUI();
    }
}//end inspectorclass
#endif

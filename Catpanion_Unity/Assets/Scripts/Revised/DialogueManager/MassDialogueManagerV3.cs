using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class MassDialogueManagerV3 : MonoBehaviour
{
    static MassDialogueManagerV3 instance;
    public static MassDialogueManagerV3 Instance { get { return instance; } }

    public Enums.HANDLER parsingHandler = Enums.HANDLER.RANDOMIZEDHANDLER;
    public Enums.HANDLER visibleHandler = Enums.HANDLER.RANDOMIZEDHANDLER;
    public Enums.HANDLER locationHandler = Enums.HANDLER.LOCATIONMANAGER;

    [Header("Options")]
    [SerializeField]
    Transform buttonLocation;
    [SerializeField]
    GameObject optionsPrefab;

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

    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    private void OnDisable()
    {
        instance = null;
    }

    public string GetProperName(string name)
    {
        switch (parsingHandler)
        {
            case Enums.HANDLER.RANDOMIZEDHANDLER:
                return RandomizedHandler.Instance.GetCharacterName(name);
            case Enums.HANDLER.ITEMHANDLER:
                //Example:
                //return ItemHandler.Instance.GetFromAccessKey(name);
                return "";
            default:
                return name;
        }
    }
    public void HandleVisible(List<OnScreen> onScreen)
    {
        switch (visibleHandler)
        {
            case Enums.HANDLER.RANDOMIZEDHANDLER:
                RandomizedHandler.Instance.ManageVisible(onScreen);
                break;
            case Enums.HANDLER.ITEMHANDLER:
                //Example:
                //ItemHandler.Instance.ManageVisible(onScreen);
                break;
            default:
                break;
        }
    }
    public void ChangeLocation(Enums.Locations location)
    {

        switch (locationHandler)
        {

            case Enums.HANDLER.LOCATIONMANAGER:
                RevisedLocationManager.Instance?.ChangeLocation(location);
                break;
            default:
                //idk this is mostly just a just in case thing
                break;

        }

    }

    public void HandleOptions(List<OptionsV3> optionsV3)
    {
        foreach (OptionsV3 o in optionsV3)
        {
            GameObject obj = GameObject.Instantiate(optionsPrefab, buttonLocation);
            OptionsButtonV3 opt = obj.GetComponent<OptionsButtonV3>();
            opt.buttonText.text = o.optionName;
            opt.optionEvent = o.optionEvent;

        }
    }



    public float GetSpeed(Enums.DialogueSpeed speed)
    {
        switch(speed)
        {
            case Enums.DialogueSpeed.SLOW:
                return slowSpeed;
            case Enums.DialogueSpeed.VERYSLOW:
                return verySlowSpeed;
            case Enums.DialogueSpeed.FAST:
                return fastSpeed;
            case Enums.DialogueSpeed.VERYFAST:
                return veryFastSpeed;
            default:
                return normalSpeed;
        }

       
    }

    public void DestroyAllButtons()
    {
        for (int i = 0; i < buttonLocation.childCount; i++)
        {
            Destroy(buttonLocation.GetChild(i));

        }
    }

}

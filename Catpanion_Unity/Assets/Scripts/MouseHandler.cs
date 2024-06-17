using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseHandler : MonoBehaviour
{
    private static MouseHandler instance;

    public static MouseHandler Instance { get { return instance; } }

    [SerializeField] private GameObject hoveredObject;

    private void Awake()
    {
        instance = this;   
    }

    public bool IsOverUIElement()
    {
        if (hoveredObject == null ) 
        { 
            return false; 
        } 
        else 
        { 
            if(hoveredObject.tag == "Taskbar")
            {
                return false;
            }
            else
            {

                return true;
            }
        }
    }

    public void StoreObject(GameObject obj)
    {
        hoveredObject = obj;
    }

    public void LeftObject()
    {
        hoveredObject = null;
    }
}

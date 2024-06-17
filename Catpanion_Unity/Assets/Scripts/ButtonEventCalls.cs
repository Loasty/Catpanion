using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventCalls : MonoBehaviour
{
    public void OpenSettingsMenu()
    {
        Settings.Instance.ToggleSettingsMenu();
    }

    public void NotifyHoveringToMouseHandler(GameObject obj)
    {
        if (MouseHandler.Instance != null)
        {
            MouseHandler.Instance.StoreObject(obj);
        }
    }

    public void NotifyLeftObjToMouseHandler()
    {
        if (MouseHandler.Instance != null) { MouseHandler.Instance.LeftObject(); }
    }
}

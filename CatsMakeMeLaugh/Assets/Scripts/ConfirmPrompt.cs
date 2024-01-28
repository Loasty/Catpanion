using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPrompt : MonoBehaviour
{
    public GameObject parentCanvas;

    public void QuitGame()
    {
        GameData.Instance.SaveAndClose();
    }

    public void ResetData()
    {
        GameData.Instance.DeleteSaveData();
        ClosePrompt();
    }

    public void ClosePrompt()
    {
        parentCanvas.SetActive(false);
    }
}

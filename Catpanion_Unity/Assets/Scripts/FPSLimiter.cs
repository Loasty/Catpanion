using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] int desiredFPS = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = desiredFPS;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
    }
}

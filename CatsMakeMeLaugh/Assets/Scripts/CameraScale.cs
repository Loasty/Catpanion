using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScale : MonoBehaviour
{

    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = Screen.width;

    Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
        sceneWidth = Screen.width;
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        _camera.orthographicSize = desiredHalfHeight;

        _camera.transform.position = new Vector3 (Screen.width/2, Screen.height/2, -10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;    
    }
}

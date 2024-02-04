using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOverheadLoad : MonoBehaviour
{
    public List<MonoBehaviour> scriptsToLoad;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < scriptsToLoad.Count; i++)
        {
            scriptsToLoad[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

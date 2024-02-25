using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevisedGameLoopManager : MonoBehaviour
{
    
    public List<DialogueManagerRevised> loopList = new List<DialogueManagerRevised>();
    public int currentIndex = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        ProcessLoopList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        currentIndex = 0;
    }
    public void ProcessLoopList()
    {
        for (int i = 0; i < loopList.Count;i++)
        {
            loopList[i].NotifyDialogueComplete += NextIndex;
        }
        if (loopList.Count > 0)
        {
            loopList[currentIndex].gameObject.SetActive(true);
        }
    }
   
    public void NextIndex()
    {
        loopList[currentIndex].currentIndex = -1;
        currentIndex++;
        if (currentIndex >= loopList.Count)
        {
            currentIndex = 0;
        }
        loopList[currentIndex].gameObject.SetActive(true);

    }
    public void PrevIndex()
    {
        loopList[currentIndex].currentIndex = -1;
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = loopList.Count - 1;
        }
        loopList[currentIndex].gameObject.SetActive(true);

    }

}

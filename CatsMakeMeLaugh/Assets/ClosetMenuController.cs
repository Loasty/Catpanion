using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetMenuController : MonoBehaviour
{ 
    /////////////
    /// Instance
    /// 
    private static ClosetMenuController instance;

    public static ClosetMenuController Instance { get { return instance; } }

    public GameObject closetObj;
    public Transform open_Pos;
    public Transform close_Pos;
    public bool isOpen;
    bool panelIsMoving;

    private void OnEnable()
    {
        if(instance == null) instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        panelIsMoving = false;
        closetObj.transform.position = close_Pos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu()
    {
        if (panelIsMoving) { return; }
        panelIsMoving = true;

        isOpen = !isOpen;
        Transform targetPos = isOpen ? open_Pos : close_Pos;
        
        StartCoroutine(SlidePanel(targetPos.position));
    }

    public IEnumerator SlidePanel(Vector2 desiredPos)
    {
        float curTime = 0;
        float delay = .25f;
        while (Mathf.Abs(closetObj.transform.position.x - desiredPos.x) >= 0.5f)
        {
            curTime += 0.05f * Time.deltaTime;
            closetObj.transform.position = Vector2.Lerp(closetObj.transform.position, desiredPos, curTime / delay);


            yield return new WaitForEndOfFrame();
        }
        panelIsMoving = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesktopMode : MonoBehaviour
{
    [SerializeField] GameObject taskbar;
    [SerializeField] GameObject cat;
    [SerializeField] Animator catAnim;

    [Header("Cat Behavior Valeus")]
    [SerializeField] int idleDelay = 15;

    private void Awake()
    {
        taskbar = GameObject.FindGameObjectWithTag("Taskbar");
        LoadCatData();
    }

    public void LoadCatData()
    {
        GameObject spawnCat = GameData.Instance.catPrefabs[(int)GameData.SavedCat.type];
        cat = Instantiate(spawnCat, taskbar.transform);
        catAnim = cat.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PickNewAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SitIdle()
    {
        Debug.Log("SittingIdle");
        ResetAllAnimTriggers();
        yield return new WaitForSeconds(15f);
        PickNewAction();
    }

    public void PickNewAction()
    {
        int selectedOption = Random.Range(0, 100);
        selectedOption = 50;
        Debug.Log(selectedOption);
        //{ SIT, WALK, GREET, SWIPE, AFFECTION, ATTENTION, MEOW, PLAY }
        switch (selectedOption)
        {
            case <= (int)Enums.Actions.SIT:
                StartCoroutine(SitIdle());
                break;
            case <= (int)Enums.Actions.WALK:
                StartCoroutine(Walk());
                break;
            case <= (int)Enums.Actions.GREET:
                StartCoroutine(Greet());
                break;
            case <= (int)Enums.Actions.SWIPE:
                StartCoroutine(Swipe());
                break;
            case <= (int)Enums.Actions.AFFECTION:
                StartCoroutine(Affection());
                break;
            case <= (int)Enums.Actions.ATTENTION:
                StartCoroutine(Attention());
                break;
            case <= (int)Enums.Actions.MEOW:
                StartCoroutine(Meow());
                break;
            case <= (int)Enums.Actions.PLAY:
                StartCoroutine(Play());
                break;
        }
    }

    void ResetAllAnimTriggers()
    {
        foreach (var param in catAnim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                catAnim.ResetTrigger(param.name);
            }

            if(param.type == AnimatorControllerParameterType.Bool)
            {
                catAnim.SetBool(param.name, false);
            }
        }
    }

    IEnumerator Walk()
    {
        Debug.Log("Walk");
        float randomX;
        if (cat.transform.position.x > 0)
        {
            //Walk Left
            randomX = Random.Range(-850, 0);
            catAnim.SetTrigger("WalkLeft");
        } 
        else 
        {
            //Walk Right
            randomX = Random.Range(0, 850);
            catAnim.SetTrigger("WalkRight");

        }

        Vector2 newTargetPos = new Vector2(randomX, cat.transform.position.y);

        while (Vector2.Distance(newTargetPos, cat.transform.position) > 1)
        {
            cat.transform.position = Vector2.MoveTowards(cat.transform.position, newTargetPos, .5f);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(SitIdle());
    }

    IEnumerator Greet()
    {
        Debug.Log("Greet");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Swipe()
    {
        Debug.Log("Swipe");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Affection()
    {
        Debug.Log("Affection");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Attention()
    {
        Debug.Log("Attention");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Meow()
    {
        Debug.Log("MEOW");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Play()
    {
        Debug.Log("Play");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }
}

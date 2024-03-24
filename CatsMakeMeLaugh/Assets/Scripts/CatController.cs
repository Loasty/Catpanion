using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [SerializeField] Cat cat;
    [SerializeField] Animator catAnim;

    [SerializeField] AudioSource catMeowSource;
    [SerializeField] AudioSource catAttentionSource;

    [SerializeField] TextMeshProUGUI nameDisplay;

    [Header("Cat Behavior Valeus")]
    [SerializeField] int idleDelay = 15;

    int curDemoAnimIndex = 0;
    bool demoMode = false;

    private void Awake()
    {
        demoMode = true;
        //LoadCatData();
    }

    public void LoadCatData(Cat data)
    {
        cat = data;
        nameDisplay.text = cat.catName;
    }

    // Start is called before the first frame update
    void Start()
    {
        PickNewAction();
    }

    IEnumerator SitIdle()
    {
        Debug.Log("SittingIdle");
        ResetAllAnimTriggers();
        yield return new WaitForSeconds(idleDelay);
        PickNewAction();
    }

    public void PickNewAction()
    {
        int selectedOption = Random.Range(0, 100);

        if(demoMode)
        {
            switch(curDemoAnimIndex)
            {
                case 0:
                    selectedOption = (int)Enums.Actions.SIT;
                    break;
                case 1:
                    selectedOption = (int)Enums.Actions.WALK;
                    break;
                case 2:
                    selectedOption = (int)Enums.Actions.WALK;
                    break;
                case 3:
                    selectedOption = (int)Enums.Actions.GREET;
                    break;
                case 4:
                    selectedOption = (int)Enums.Actions.SWIPE;
                    break;
                case 5:
                    selectedOption = (int)Enums.Actions.AFFECTION;
                    break;
            }
            curDemoAnimIndex++;
            if(curDemoAnimIndex >= 5) { curDemoAnimIndex = 0; }
        }

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

            if (param.type == AnimatorControllerParameterType.Bool)
            {
                catAnim.SetBool(param.name, false);
            }
        }
    }

    IEnumerator Walk()
    {
        Debug.Log("Walk");
        //float width = taskbar.GetComponent<RectTransform>().rect.width;
        float randomX;

        float width = Screen.width;
        float offset = width * .20f;

        if (transform.position.x > width / 2)
        {
            //Walk Left
            randomX = Random.Range(0 - offset, width / 2);
            //randomX = Random.Range(-850, 0);
            catAnim.SetTrigger("WalkLeft");
        }
        else
        {
            //Walk Right
            randomX = Random.Range(width / 2, width + offset);
            //randomX = Random.Range(0, 850);
            catAnim.SetTrigger("WalkRight");

        }

        Vector2 newTargetPos = new Vector2(randomX, DesktopMode.Instance.taskbar.transform.position.y);

        while (Vector2.Distance(newTargetPos, transform.position) > 1)
        {
            newTargetPos = new Vector2(randomX, DesktopMode.Instance.taskbar.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, newTargetPos, .5f);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(SitIdle());
    }

    IEnumerator Greet()
    {
        Debug.Log("Greet");
        catAnim.SetTrigger("Greet");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Swipe()
    {
        Debug.Log("Swipe");
        catAnim.SetTrigger("Swipe");
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Affection()
    {
        Debug.Log("Affection");
        catAnim.SetTrigger("Affection");

        while (catAnim.GetCurrentAnimatorStateInfo(0).length >
           catAnim.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Attention()
    {
        Debug.Log("Attention");
        catAnim.SetTrigger("Attention");

        int num = Random.Range(0, DesktopMode.Instance.attentionSounds.Count);
        catAttentionSource.clip = DesktopMode.Instance.attentionSounds[num];
        catAttentionSource.Play();

        while (catAnim.GetCurrentAnimatorStateInfo(0).length >
           catAnim.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Meow()
    {
        Debug.Log("MEOW");
        catAnim.SetTrigger("Meow");
        if (cat.type != Enums.CatType.BLACK)
        {
            int num = Random.Range(1, DesktopMode.Instance.meowSounds.Count);
            catMeowSource.clip = DesktopMode.Instance.meowSounds[num];
            catMeowSource.Play();
        }
        else
        {
            catMeowSource.PlayOneShot(DesktopMode.Instance.meowSounds[0]);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Play()
    {
        Debug.Log("Play");
        catAnim.SetBool("Play", true);

        int num = Random.Range(0, DesktopMode.Instance.playSounds.Count);
        catAttentionSource.Stop();
        catAttentionSource.clip = DesktopMode.Instance.playSounds[num];
        catAttentionSource.Play();

        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }
}

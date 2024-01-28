using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesktopMode : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static DesktopMode instance;

    public static DesktopMode Instance { get { return instance; } }


    public GameObject taskbar;
    [SerializeField] GameObject cat;
    [SerializeField] Animator catAnim;

    [SerializeField] AudioSource catMeowSource;
    [SerializeField] AudioSource catAttentionSource;
    [SerializeField] List<AudioClip> meowSounds;
    [SerializeField] List<AudioClip> attentionSounds;
    [SerializeField] List<AudioClip> playSounds;

    [Header("Cat Behavior Valeus")]
    [SerializeField] int idleDelay = 15;

    private void Awake()
    {
        if (instance == null) instance = this;
        taskbar = GameObject.FindGameObjectWithTag("Taskbar");
        LoadCatData();
    }

    public void LoadCatData()
    {
        GameObject spawnCat = GameData.Instance.catPrefabs[(int)GameData.Instance.savedCat.type];
        cat = Instantiate(spawnCat, taskbar.transform);
        catAnim = cat.GetComponent<Animator>();
        catMeowSource = GameObject.FindGameObjectWithTag("MeowSource").GetComponent<AudioSource>();
        catAttentionSource = GameObject.FindGameObjectWithTag("AttentionSource").GetComponent<AudioSource>();
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
        //float width = taskbar.GetComponent<RectTransform>().rect.width;
        float randomX;
        if (cat.transform.position.x > 0)
        {
            //Walk Left
            //randomX = Random.Range(-(width / 2), 0);
            randomX = Random.Range(-850, 0);
            catAnim.SetTrigger("WalkLeft");
        } 
        else 
        {
            //Walk Right
            //randomX = Random.Range(0, width/2);
            randomX = Random.Range(0, 850);
            catAnim.SetTrigger("WalkRight");

        }

        Vector2 newTargetPos = new Vector2(randomX, taskbar.transform.position.y);

        while (Vector2.Distance(newTargetPos, cat.transform.position) > 1)
        {
            newTargetPos = new Vector2(randomX, taskbar.transform.position.y);
            cat.transform.position = Vector2.MoveTowards(cat.transform.position, newTargetPos, .5f);
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

        int num = Random.Range(0, attentionSounds.Count);
        catAttentionSource.clip = attentionSounds[num];
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
        if(GameData.Instance.savedCat.type != Enums.CatType.BLACK)
        {
            int num = Random.Range(1, meowSounds.Count);
            catMeowSource.clip = meowSounds[num];
            catMeowSource.Play();
        } 
        else
        {
            catMeowSource.PlayOneShot(meowSounds[0]);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }

    IEnumerator Play()
    {
        Debug.Log("Play");
        catAnim.SetBool("Play", true);

        int num = Random.Range(0, playSounds.Count);
        catAttentionSource.Stop();
        catAttentionSource.clip = playSounds[num];
        catAttentionSource.Play();

        yield return new WaitForEndOfFrame();
        StartCoroutine(SitIdle());
    }
}

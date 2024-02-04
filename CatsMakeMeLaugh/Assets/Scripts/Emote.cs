using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    [SerializeField] Enums.Emotes typeOfEmote;
    [SerializeField] Enums.EmoteEffect effectOfEmote;
    [SerializeField] float durationOfEffect;
    [SerializeField] float numOfRepeats;
    [SerializeField] float effectSpeed;
    [SerializeField] Image image;
    [SerializeField] Transform topPosPlaceholder;
    [SerializeField] Transform botPosPlaceholder;

    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SetEmoteImage();
        StartCoroutine(PlayEmoteEffect());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void PlayEmote(Enums.Emotes emote, Enums.EmoteEffect effect, float duration, float repeats, float speed)
    {
        typeOfEmote = emote;
        effectOfEmote = effect;
        durationOfEffect = duration;
        numOfRepeats = repeats;
        effectSpeed = speed;

        SetEmoteImage();

        StartCoroutine(PlayEmoteEffect());
    }

    public void SetEmoteImage()
    {
        image.sprite = GameData.Instance.emoteSprites[(int)typeOfEmote];
        image.gameObject.SetActive(true);
    }

    public IEnumerator PlayEmoteEffect()
    {
        float curTime = 0;


        Quaternion leftRot = Quaternion.Euler(0, 0, 45f);
        Quaternion rightRot = Quaternion.Euler(0, 0, -45f);
        bool rotatingClockwise = false;

        bool increasing = true;
        Vector3 minSize = new Vector3(0.5f, 0.5f, 0);
        Vector3 maxSize = new Vector3(1.5f, 1.5f, 0);

        Vector2 targetUpPos = topPosPlaceholder.position;

        Vector2 targetDownPos = botPosPlaceholder.position;

        for (int i = 0; i < numOfRepeats; i++)
        {
            curTime = 0;

            switch (effectOfEmote)
            {
                case Enums.EmoteEffect.NONE:

                    yield return new WaitForSeconds(durationOfEffect);
                    break;

                case Enums.EmoteEffect.WIGGLE:

                    while (curTime < durationOfEffect)
                    {
                        curTime += Time.deltaTime;

                        image.transform.localScale = Vector3.one;

                        if (rotatingClockwise)
                        {
                            image.transform.rotation = Quaternion.Lerp(image.transform.rotation, rightRot, Time.deltaTime * effectSpeed);
                        }
                        else
                        {
                            image.transform.rotation = Quaternion.Lerp(image.transform.rotation, leftRot, Time.deltaTime * effectSpeed);
                        }

                        if (Quaternion.Angle(image.transform.rotation, leftRot) <= 1 || Quaternion.Angle(image.transform.rotation, rightRot) <= 1)
                        {
                            rotatingClockwise = !rotatingClockwise;
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    break;

                case Enums.EmoteEffect.RESIZE:
                    
                    while (curTime < durationOfEffect)
                    {
                        curTime += Time.deltaTime;

                        image.transform.rotation = Quaternion.identity;

                        if (!increasing)
                            image.transform.localScale = Vector3.Lerp(image.transform.localScale, minSize, Time.deltaTime * effectSpeed);
                        else
                            image.transform.localScale = Vector3.Lerp(image.transform.localScale, maxSize, Time.deltaTime * effectSpeed);

                        if(Vector2.Distance(image.transform.localScale, minSize) <= 0.01f || Vector2.Distance(image.transform.localScale, maxSize) <= 0.01f)
                        {
                            increasing = !increasing;
                            GameData.Instance.InvokeJoeSwap();
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    break;

                case Enums.EmoteEffect.GO_UP:
                    
                    while (Vector2.Distance(image.transform.position, targetUpPos) >= 0.1f)
                    {
                        curTime += Time.deltaTime;

                        image.transform.rotation = Quaternion.identity;
                        image.transform.localScale = Vector3.one;

                        image.transform.position = Vector2.Lerp(image.transform.position, targetUpPos, Time.deltaTime * effectSpeed);

                        yield return new WaitForEndOfFrame();
                    }

                    image.transform.position = Vector2.zero;

                    break;

                case Enums.EmoteEffect.GO_DOWN:

                    while (Vector2.Distance(image.transform.position, targetDownPos) >= 0.1f)
                    {
                        curTime += Time.deltaTime;

                        image.transform.rotation = Quaternion.identity;
                        image.transform.localScale = Vector3.one;

                        image.transform.position = Vector2.Lerp(image.transform.position, targetDownPos, Time.deltaTime * effectSpeed);

                        yield return new WaitForEndOfFrame();
                    }

                    image.transform.position = Vector2.zero;

                    break;
                     
            }
        }
        
        image.gameObject.SetActive(false);
        yield return null;
    }

}

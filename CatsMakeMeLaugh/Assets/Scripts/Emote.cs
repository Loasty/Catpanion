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
    [SerializeField] Image image;
    [SerializeField] Vector3 startingPos;
    [SerializeField] Transform topPosPlaceholder;
    [SerializeField] Transform botPosPlaceholder;

    private void Awake()
    {
        startingPos = transform.position;
    }

    private void Start()
    {
        SetEmoteImage();
        StartCoroutine(PlayEmoteEffect());
    }

    public void PlayEmote(Enums.Emotes emote, Enums.EmoteEffect effect, float duration, float repeats)
    {
        typeOfEmote = emote;
        effectOfEmote = effect;
        durationOfEffect = duration;
        numOfRepeats = repeats;
        startingPos = transform.position;

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
        Vector3 minSize = new Vector3(0.25f, 0.25f, 0);
        Vector3 maxSize = new Vector3(1.75f, 1.75f, 0);

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
                        image.transform.position = startingPos;

                        if (rotatingClockwise)
                        {
                            image.transform.rotation = Quaternion.Lerp(image.transform.rotation, rightRot, 0.015f);
                        }
                        else
                        {
                            image.transform.rotation = Quaternion.Lerp(image.transform.rotation, leftRot, 0.015f);
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

                        image.transform.position = startingPos;
                        image.transform.rotation = Quaternion.identity;

                        if (!increasing)
                            image.transform.localScale = Vector3.Lerp(image.transform.localScale, minSize, 0.015f);
                        else
                            image.transform.localScale = Vector3.Lerp(image.transform.localScale, maxSize, 0.015f);

                        if(Vector2.Distance(image.transform.localScale, minSize) <= 0.01f || Vector2.Distance(image.transform.localScale, maxSize) <= 0.01f)
                            increasing = !increasing;

                        yield return new WaitForEndOfFrame();
                    }

                    break;

                case Enums.EmoteEffect.GO_UP:
                    
                    while (Vector2.Distance(image.transform.position, targetUpPos) >= 0.1f)
                    {
                        curTime += Time.deltaTime;

                        image.transform.rotation = Quaternion.identity;
                        image.transform.localScale = Vector3.one;

                        image.transform.position = Vector2.Lerp(image.transform.position, targetUpPos, curTime / durationOfEffect / 2);

                        yield return new WaitForEndOfFrame();
                    }

                    break;

                case Enums.EmoteEffect.GO_DOWN:

                    while (Vector2.Distance(image.transform.position, targetDownPos) >= 0.1f)
                    {
                        curTime += Time.deltaTime;

                        image.transform.rotation = Quaternion.identity;
                        image.transform.localScale = Vector3.one;

                        image.transform.position = Vector2.Lerp(image.transform.position, targetDownPos, curTime / durationOfEffect / 2);

                        yield return new WaitForEndOfFrame();
                    }
                    break;
                     
            }
        }
        
        image.gameObject.SetActive(false);
        yield return null;
    }

}

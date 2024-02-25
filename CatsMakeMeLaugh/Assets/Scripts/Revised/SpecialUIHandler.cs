using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private Image imageComponent;
    private Transform gameObj;
    private TextMeshProUGUI textComponent;
    public bool fireFadeIn = false;
    public bool fireFadeOut = false;
    public bool fireSlideInRight = false;
    public bool fireSlideOutRight = false;
    public bool fireSlideInLeft = false;
    public bool fireSlideOutLeft = false;

    void Start()
    {
        gameObj = this.transform;
        textComponent = GetComponent<TextMeshProUGUI>();
        imageComponent = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireFadeIn)
        {
            fireFadeIn = false;
            FadeInImage();
        }
        if (fireFadeOut)
        {
            fireFadeOut = false;
            FadeOutImage();
        }

        if (fireSlideInRight)
        {
            fireSlideInRight = false;
            SlideInRight();
        }
        if (fireSlideOutRight)
        {
            fireSlideOutRight = false;
            SlideOutRight();
        }

        if (fireSlideInLeft)
        {
            fireSlideInLeft = false;
            SlideInLeft();
        }
        if (fireSlideOutLeft)
        {
            fireSlideOutLeft = false;
            SlideOutLeft();
        }

    }

    //Fade
    public void FadeIn()
    {
        if (imageComponent != null)
        {
            FadeInImage();
        }
        if (textComponent != null)
        {
            FadeInText();
        }

    }

    public void FadeOut()
    {
        if (imageComponent != null)
        {
            FadeOutImage();
        }
        if (textComponent != null)
        {
            FadeOutText();
        }
        
    }

    private void FadeInImage()
    {
        StartCoroutine(Fade(true, imageComponent, 0.1f, 1, 0.1f));
    }
    private void FadeOutImage()
    {
        StartCoroutine(Fade(false, imageComponent, -0.1f, 0, 0.1f));
    }

    private void FadeInText()
    {
        StartCoroutine(Fade(true, textComponent, 0.1f, 1, 0.1f));
    }
    private void FadeOutText()
    {
        StartCoroutine(Fade(false, textComponent, -0.1f, 0, 0.1f));
    }


    IEnumerator Fade(bool fadeIn, Image img, float changeVal, int goalVal, float speed)
    {
        Color color = img.color;
        if (fadeIn)
        {
            while (color.a < goalVal)
            {
                color.a += changeVal;
                img.color = color;
                yield return new WaitForSeconds(speed);
            }
        }
        else
        {
            while (color.a > goalVal)
            {
                color.a += changeVal;
                img.color = color;
                yield return new WaitForSeconds(speed);
            }
        }
        color.a = goalVal;

    }
    IEnumerator Fade(bool fadeIn, TextMeshProUGUI text, float changeVal, int goalVal, float speed)
    {
        Color color = text.color;
        if (fadeIn)
        {
            while (color.a < goalVal)
            {
                color.a += changeVal;
                text.color = color;
                yield return new WaitForSeconds(speed);
            }
        }
        else
        {
            while (color.a > goalVal)
            {
                color.a += changeVal;
                text.color = color;
                yield return new WaitForSeconds(speed);
            }
        }
        color.a = goalVal;

    }

    //Slide
    public void SlideInRight()
    {
        float max = imageComponent.rectTransform.rect.width;
        StartCoroutine(Slide(true, gameObj, 5, 0, 0.01f));
    }
    public void SlideOutRight()
    {
        float max = imageComponent.rectTransform.rect.width;
        StartCoroutine(Slide(true, gameObj, 5, max, 0.01f));
    }

    public void SlideInLeft()
    {
        float max = imageComponent.rectTransform.rect.width;
        StartCoroutine(Slide(false, gameObj, -5, 0, 0.01f));
    }
    public void SlideOutLeft()
    {
        float max = imageComponent.rectTransform.rect.width;
        StartCoroutine(Slide(false, gameObj, -5, -max, 0.01f));
    }


    IEnumerator Slide(bool right, Transform rect, float changeVal, float goalVal, float speed)
    {
        if (right)
        {

            while (rect.localPosition.x < goalVal)
            {
                Debug.Log(rect.localPosition.x);
                rect.localPosition += new Vector3(changeVal, 0, 0);
                Debug.Log(rect.localPosition.x);
                yield return new WaitForSeconds(speed);

            }

        }
        else
        {

            while (rect.localPosition.x > goalVal)
            {
                Debug.Log(rect.localPosition.x);
                rect.localPosition += new Vector3(changeVal, 0, 0);
                Debug.Log(rect.localPosition.x);
                yield return new WaitForSeconds(speed);

            }

        }
        rect.localPosition = new Vector3(goalVal, rect.localPosition.y, rect.localPosition.z);
    }


}
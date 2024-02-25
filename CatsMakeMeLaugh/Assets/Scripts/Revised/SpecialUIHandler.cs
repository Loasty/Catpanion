using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Image test;
    public bool fireFadeIn = false;
    public bool fireFadeOut = false;
    public bool fireSlideIn = false;
    public bool fireSlideOut = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireFadeIn)
        {
            fireFadeIn = false;
            FadeIn();
        }
        if (fireFadeOut)
        {
            fireFadeOut = false;
            FadeOut();
        }
        if (fireSlideIn)
        {
            fireSlideIn = false;
            SlideIn();
        }
        if (fireSlideOut)
        {
            fireSlideOut = false;
            SlideOut();
        }
    }
    public void FadeIn()
    {
        StartCoroutine(Fade(true, test, 0.1f, 1, 0.1f));
    }
    public void FadeOut()
    {
      
        StartCoroutine(Fade(false, test, -0.1f, 0, 0.1f));
    }
    public void SlideIn()
    {
        float max = test.rectTransform.rect.width;
        StartCoroutine(Slide(true, test.transform, 0.1f, max, 0.01f));
        
        
        
    }
    public void SlideOut()
    {
        float max = test.rectTransform.rect.width;
        StartCoroutine(Slide(false, test.rectTransform, 1, 0, 0.01f));
    }

    IEnumerator Slide(bool slideIn, Transform rect, float changeVal, float goalVal, float speed)
    {
        if (slideIn)
        {
            while (rect.position.x < goalVal)
            {
                Debug.Log(rect.position.x);
                rect.position += new Vector3(changeVal, 0, 0);
                
                yield return new WaitForSeconds(speed);

            }
        }
        else
        {
            while (rect.position.x > goalVal)
            {
                Debug.Log(rect.position.x);
                rect.position += new Vector3(changeVal, 0, 0);

                yield return new WaitForSeconds(speed);
            }
           
        }
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
    
}

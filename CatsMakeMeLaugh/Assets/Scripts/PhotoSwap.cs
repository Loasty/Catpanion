using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSwap : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;

    void Start()
    {
        StartCoroutine(SwapPhotos());
    }

    IEnumerator SwapPhotos()
    {
        while(true)
        {
            img.sprite = (img.sprite == sprite1) ? sprite2 : sprite1;
            yield return new WaitForSeconds(.6f);
        }
    }
}

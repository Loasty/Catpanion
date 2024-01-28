using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSwap : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;
    [SerializeField] float speed = 1.0f;
    [SerializeField] bool isMainMenuJoe;

    void Start()
    {
        if (isMainMenuJoe)
        {
            GameData.justForMainMenuJoe += Swap;
        }
        else
        {

            StartCoroutine(SwapPhotos());
        }
    }

    IEnumerator SwapPhotos()
    {
        while(true)
        {
            Swap();
            yield return new WaitForSeconds(speed);
        }
    }

    private void Swap()
    {
        img.sprite = (img.sprite == sprite1) ? sprite2 : sprite1;
    }
}

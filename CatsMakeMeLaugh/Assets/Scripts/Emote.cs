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
    [SerializeField] List<Sprite> sprites;

    private void Awake()
    {
        
    }

    public void SetEmoteImage()
    {

    }

    public IEnumerator PlayEmoteEffect()
    {
        switch (effectOfEmote)
        {
            
        }

        yield return null;
    }

}

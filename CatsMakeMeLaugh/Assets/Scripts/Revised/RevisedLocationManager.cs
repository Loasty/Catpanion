using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class LocationClass
{
    public Sprite locationSprite;
    public Enums.Locations locationEnum = Enums.Locations.NONE;
}
public class RevisedLocationManager : MonoBehaviour
{
    public List<LocationClass> locationsData = new List<LocationClass>();
    public Image backgroundImage;
    public SpecialUIHandler backgroundImageHandler;
    Dictionary<Enums.Locations, Sprite> Locations = new Dictionary<Enums.Locations, Sprite>();
    public Enum currentLocation = Enums.Locations.NONE;

    static private RevisedLocationManager instance;
    static public RevisedLocationManager Instance { get { return instance; } }

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        ProcessLocations();
    }
    public void ProcessLocations()
    {
        for (int i = 0; i < locationsData.Count; i++)
        {
            try
            {
                Locations.Add(locationsData[i].locationEnum, locationsData[i].locationSprite);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public void ChangeLocation(Enums.Locations locationEnum)
    {
        Sprite sprite;
        Locations.TryGetValue(locationEnum, out sprite);
        if (sprite != null)
        {
            backgroundImage.sprite = sprite;
        }
    }
    
    

    
}

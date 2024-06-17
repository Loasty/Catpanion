using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Locations
{
    public Sprite sprite;
    public Enums.Locations location;
}
public class LocationManager : MonoBehaviour
{
    public List<Locations> locations;
    public Dictionary<Enums.Locations, Locations> locationDict = new Dictionary<Enums.Locations, Locations>();
    public Enums.Locations currentLocation;

    public Image backgroundImg; 

    static private LocationManager instance;
    static public LocationManager Instance { get { return instance; } }

    



    // Start is called before the first frame update
    void Start()
    {
        SetInstance();
        SortLocations();
        
    }
    public void SortLocations()
    {

        if (locations.Count != locationDict.Count)
        {
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    try
                    {
                        locationDict.Add(locations[i].location, locations[i]);
                    }
                    catch
                    {
                        Debug.LogError("Something went wrong. Make sure that there is no multiple of location type. There should be one of each.");
                    }
                }
            }
        }
    }
    public void SetInstance()
    {
        if (instance == null) { instance = this; };
        SortLocations();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLocation(Enums.Locations newLocation)
    {
        currentLocation = newLocation;
        Locations location;
        locationDict.TryGetValue(newLocation, out location);
        backgroundImg.sprite = location.sprite;
    }
}

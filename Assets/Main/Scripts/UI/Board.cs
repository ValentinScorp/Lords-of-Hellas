using System;
using UnityEngine;

[System.Serializable]
public class Board : MonoBehaviour
{
  // public TextAsset boardStructureFile;
    public TextAsset seaTrailsFile;

    // Map Regions
    [System.Serializable]
    public class Region
    {
        public string name;
        public string color;
        public int ps;
        public bool city;
        public bool shrine;
        public bool port;
        public bool monument;
        public string [] neighbor_regions;
    }

    [System.Serializable]
    public class RegionList
    {
        public Region[] regions;
    }
    public RegionList mapRegions = new RegionList();

    // Sea Trails
    [System.Serializable]
    public class SeaTrail
    {
        public string region1;
        public string region2;
    }
    [System.Serializable]
    public class SeaTrailList
    {
        public SeaTrail[] sea_trails;
    }
    public SeaTrailList seaTrails = new SeaTrailList();

    void Awake() {
        try {
       //     mapRegions = JsonUtility.FromJson<RegionList>(boardStructureFile.text);
       //     seaTrails = JsonUtility.FromJson<SeaTrailList>(seaTrailsFile.text);
        } catch (Exception ex) {
            string message = $"Error on {gameObject.name}: {ex.Message}";
            Debug.LogError(message, gameObject);
        }
    }
    private void Start()
    {
        
    }
}

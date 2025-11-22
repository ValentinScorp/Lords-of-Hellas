using System;
using System.IO;
using UnityEngine;

public class RegionStaticDataLoader
{
    private RegionJsonWrapper RegionJsonWrapper { get; set; } = new();

    public bool LoadMapData() {
        return LoadFromFile("MapStructure.json");
    }
    private bool LoadFromFile(string filePath) {
        string fullPath = Application.dataPath + "/Main/Resources/" + filePath;
        if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath)) {
            Debug.LogError($"File path is invalid or file does not exist: {fullPath}");
            return false;
        }
        try {
            string jsonText = File.ReadAllText(fullPath);
            RegionJsonWrapper = JsonUtility.FromJson<RegionJsonWrapper>(jsonText);
        } catch (Exception ex) {
            string message = $"Error on load MapStructure.json: {ex.Message}";
            Debug.LogError(message);
            return false;
        }
        foreach (var jsonRegion in RegionJsonWrapper.regions) {
            RegionStaticData regionData = new();
            CopyRegionData(jsonRegion, regionData);
           
            GameData.Instance.RegionStaticData.Add(regionData);            
        }
        
        return true;
    }
    private void CopyRegionData(RegionJson source, RegionStaticData dest) {
        dest.CopyData(source);
    }
}

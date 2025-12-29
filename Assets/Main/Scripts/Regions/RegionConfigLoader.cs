using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RegionConfigLoader
{
    private RegionJsonWrapper RegionJsonWrapper { get; set; } = new();

    public bool TryLoadRegions(out List<RegionConfig> regions) {
        return TryLoadFromFile("MapStructure.json", out regions);
    }
    private bool TryLoadFromFile(string filePath, out List<RegionConfig> regions) {
        regions = new List<RegionConfig>();

        string fullPath = Application.dataPath + "/Main/Resources/" + filePath;
        if (!File.Exists(fullPath)) {
            Debug.LogError($"File does not exist: {fullPath}");
            return false;
        }
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
            RegionConfig regionData = new();
            CopyRegionData(jsonRegion, regionData);
            regions.Add(regionData);
        }        
        return true;
    }
    private void CopyRegionData(RegionJson source, RegionConfig dest) {
        dest.CopyData(source);
    }
}

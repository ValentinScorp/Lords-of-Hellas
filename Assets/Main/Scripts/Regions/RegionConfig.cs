using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionConfig
{
    [Header("Static Properties")]
    [SerializeField] private string _regionName;
    [SerializeField] private string _landColor;
    [SerializeField] private int _populationStrength;
    [SerializeField] private bool _hasShrine;
    [SerializeField] private bool _hasCity;
    [SerializeField] private bool _hasPort;
    [SerializeField] private bool _hasMonument;
    [SerializeField] private string _monumentGodName;
    [SerializeField] private RegionJson _sourceData;

    [System.NonSerialized] public List<RegionConnection> RegionConnections;

    public string RegionName => _regionName;
    public string LandColor => _landColor;
    public int PopulationStrength => _populationStrength;
    public bool HasShrine => _hasShrine;
    public bool HasCity => _hasCity;
    public bool HasPort => _hasPort;
    public bool HasMonument => _hasMonument;
    public string MonumentGodName => _monumentGodName;
    public RegionJson SourceData => _sourceData;

    public void CopyData(RegionJson source) {
        _regionName = source.name;
        _landColor = source.color;
        _populationStrength = source.ps;
        _hasShrine = source.shrine;
        _hasCity = source.city;
        _hasPort = source.port;
        _hasMonument = source.monument;
        _monumentGodName = "";
        _sourceData = source;
    }
}

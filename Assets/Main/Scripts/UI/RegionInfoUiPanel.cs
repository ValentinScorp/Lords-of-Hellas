using TMPro;
using UnityEngine;

public class RegionInfoUiPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _regionNameText;
    [SerializeField] private TextMeshProUGUI _populationStrengthText;
    [SerializeField] private TextMeshProUGUI _ownerColorText;
    [SerializeField] private TextMeshProUGUI _hoplitesCount;

    public void ShowRegionInfo(RegionStatus regionRuntimeData) {
        _regionNameText.text = $"Region Name: {regionRuntimeData.RegionStaticData.RegionName}";
        _populationStrengthText.text = $"Population Strength: {regionRuntimeData.RegionStaticData.PopulationStrength}";
        _ownerColorText.text = $"Owned By: {regionRuntimeData.OwnedBy.ToString()}";
        _hoplitesCount.text = $"Hoplites count: {regionRuntimeData.GetHopliteCount(regionRuntimeData.OwnedBy)}";
        //_hoplitesCount.text = $"Hoplites Count: {regionRuntimeData.Hoplites.Count}";
        //CityToggle.isOn = data.SourceData.city;
        //ShrineToggle.isOn = data.SourceData.shrine;
        //PortToggle.isOn = data.SourceData.port;
        //MonumentToggle.isOn = data.SourceData.monument;
        gameObject.SetActive(true);
    }
    public void HidePanel() {
        gameObject.SetActive(false);
    }
}

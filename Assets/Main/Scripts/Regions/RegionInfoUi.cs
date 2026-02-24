using TMPro;
using UnityEngine;

public class RegionInfoUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _regionNameText;
    [SerializeField] private TextMeshProUGUI _populationStrengthText;
    [SerializeField] private TextMeshProUGUI _ownerColorText;
    [SerializeField] private TextMeshProUGUI _hoplitesCount;

    private RegionInfoUiCtlr _regionInfoUiController;

    private void Awake()
    {
        _regionInfoUiController = new RegionInfoUiCtlr();
        _regionInfoUiController.RegisterUi(this);
        ServiceLocator.Register(_regionInfoUiController);
    }

    public void ShowRegionInfo(RegionModel regionData)
    {
        _regionNameText.text = $"Region Name: {regionData.RegionConfig.RegionName}";
        _populationStrengthText.text = $"Population Strength: {regionData.RegionConfig.PopulationStrength}";
        _ownerColorText.text = $"Owned By: {regionData.OwnedBy.ToString()}";
        _hoplitesCount.text = $"Hoplites count: {regionData.GetHopliteCount(regionData.OwnedBy)}";
        //_hoplitesCount.text = $"Hoplites Count: {regionRuntimeData.Hoplites.Count}";
        //CityToggle.isOn = data.SourceData.city;
        //ShrineToggle.isOn = data.SourceData.shrine;
        //PortToggle.isOn = data.SourceData.port;
        //MonumentToggle.isOn = data.SourceData.monument;
        gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}

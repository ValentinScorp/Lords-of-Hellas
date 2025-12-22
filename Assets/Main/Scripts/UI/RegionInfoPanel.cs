using TMPro;
using UnityEngine;

public class RegionInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _regionNameText;
    [SerializeField] private TextMeshProUGUI _populationStrengthText;
    [SerializeField] private TextMeshProUGUI _ownerColorText;
    [SerializeField] private TextMeshProUGUI _hoplitesCount;

    RegionInfoUiController _regionInfoUiController;

    private void Awake() {
        _regionInfoUiController = new RegionInfoUiController();
        _regionInfoUiController.RegisterPanel(this);
        ServiceLocator.Register(_regionInfoUiController);
    }
    private void Start() {
        ServiceLocator.Get<SelectMgr>().RegisterSelctionListener(_regionInfoUiController);
    }
    public void ShowRegionInfo(RegionData regionRuntimeData) {
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

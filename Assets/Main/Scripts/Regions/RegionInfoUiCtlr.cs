using System;

public class RegionInfoUiCtlr
{
    private RegionAreaView _selectedRegion;
    private RegionInfoUi _regionInfoUi;
    public event Action<RegionContext> OnRegionSelected;

    public void RegisterUi(RegionInfoUi infoUi)
    {
        _regionInfoUi = infoUi;
    }
    public void Select(RegionAreaView newRegion)
    {
        if (_selectedRegion != null && _selectedRegion != newRegion) {
            _selectedRegion.Deactivate();
        }

        _selectedRegion = newRegion;
        _selectedRegion.Activate();
        var regionStatus = GameContext.Instance.RegionDataRegistry.GetRegionContext(_selectedRegion.Id);

        _regionInfoUi?.ShowRegionInfo(regionStatus);
    }
    public void Deactivate()
    {
        _selectedRegion?.Deactivate();
        _selectedRegion = null;

        _regionInfoUi?.HidePanel();
    }
}

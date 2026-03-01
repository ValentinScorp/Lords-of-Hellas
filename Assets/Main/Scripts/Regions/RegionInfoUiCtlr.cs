using System;

internal class RegionInfoUiCtlr
{
    private RegionAreaView _selectedRegion;
    private RegionInfoUi _regionInfoUi;

    internal void RegisterUi(RegionInfoUi infoUi)
    {
        _regionInfoUi = infoUi;
    }
    internal void Select(RegionAreaView newRegion)
    {
        if (_selectedRegion != null && _selectedRegion != newRegion) {
            _selectedRegion.Deactivate();
        }

        _selectedRegion = newRegion;
        _selectedRegion.Activate();
        var regionStatus = GameContext.Instance.RegionRegistry.GetRegionContext(_selectedRegion.Id);

        _regionInfoUi?.ShowRegionInfo(regionStatus);
    }
    internal void Deactivate()
    {
        _selectedRegion?.Deactivate();
        _selectedRegion = null;

        _regionInfoUi?.HidePanel();
    }
}

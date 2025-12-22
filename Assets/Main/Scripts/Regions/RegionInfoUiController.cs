public class RegionInfoUiController
{
    private RegionAreaView _selected;
    private RegionInfoPanel _regionInfoUiPanel;

    public void RegisterPanel(RegionInfoPanel panel)
    {
        _regionInfoUiPanel = panel;
    }
    public void Select(RegionAreaView newTarget)
    {
        if (_selected != null && _selected != newTarget) {
            _selected.Deactivate();
        }

        _selected = newTarget;
        _selected.Activate();
        var regionStatus = ServiceLocator.Get<RegionDataRegistry>().GetRegionData(_selected.RegionId);

        _regionInfoUiPanel?.ShowRegionInfo(regionStatus);
    }

    private void Deselect()
    {
        if (_selected != null) {
            _selected.Deactivate();
            _regionInfoUiPanel?.HidePanel();
            _selected = null;
        }
    }
}

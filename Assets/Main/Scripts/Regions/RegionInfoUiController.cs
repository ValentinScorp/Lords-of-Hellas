public class RegionInfoUiController
{
    private RegionAreaView _selected;
    private RegionInfoUiPanel _regionInfoUiPanel;

    public void RegisterPanel(RegionInfoUiPanel panel)
    {
        _regionInfoUiPanel = panel;
    }
    private void Select(RegionAreaView newTarget)
    {
        if (_selected != null && _selected != newTarget) {
            _selected.Deactivate();
        }

        _selected = newTarget;
        _selected.Activate();
        var regionStatus = ServiceLocator.Get<RegionStatusRegistry>().GetRegionData(_selected.RegionId);

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

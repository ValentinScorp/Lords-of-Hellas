using UnityEngine;

public class TokenPlacer
{
    private RegionViewManager _regionVisuals;
    private RegionDataManager _regionManager;
    private TokenVisualChanger _tokenVisualChanger;
    public TokenPlacer(RegionViewManager regionManagerVisuals, RegionDataManager regionManager, TokenMaterialPalette colorPalette) {
        _regionVisuals = regionManagerVisuals;
        _regionManager = regionManager;
        _tokenVisualChanger = new(colorPalette);
    }
    public void PlaceTokenAt(RegionId regionId, TokenType tokenType, PlayerColor color) {

    }
}

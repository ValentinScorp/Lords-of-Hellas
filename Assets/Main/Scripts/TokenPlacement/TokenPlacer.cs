using UnityEngine;

public class TokenPlacer
{
    private RegionViewManager _regionVisuals;
    private RegionStatusRegistry _regionManager;
    private TokenVisualChanger _tokenVisualChanger;
    public TokenPlacer(RegionViewManager regionManagerVisuals, RegionStatusRegistry regionManager, TokenMaterialPalette colorPalette) {
        _regionVisuals = regionManagerVisuals;
        _regionManager = regionManager;
        _tokenVisualChanger = new(colorPalette);
    }
    public void PlaceTokenAt(RegionId regionId, TokenType tokenType, PlayerColor color) {

    }
}

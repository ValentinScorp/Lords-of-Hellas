using UnityEngine;

public class TokenPlacer
{
    private RegionsView _regionVisuals;
    private RegionDataRegistry _regionManager;
    private TokenVisualChanger _tokenVisualChanger;
    public TokenPlacer(RegionsView regionManagerVisuals, RegionDataRegistry regionManager, TokenMaterialPalette colorPalette) {
        _regionVisuals = regionManagerVisuals;
        _regionManager = regionManager;
        _tokenVisualChanger = new(colorPalette);
    }
    public void PlaceTokenAt(RegionId regionId, TokenType tokenType, PlayerColor color) {

    }
}

using UnityEngine;

public class TokenVisualChanger
{
    private readonly TokenMaterialChanger _tokenMaterialChanger;

    public TokenVisualChanger(TokenMaterialPalette colorPalette) {
        _tokenMaterialChanger = new TokenMaterialChanger(colorPalette);
    }
    public void SetGhostMaterial(GameObject gameObject) {
        _tokenMaterialChanger.SetGhostMaterial(gameObject);
    }
    public void SetTokenMaterialGhostState(GameObject gameObject, TokenPlacementTerrainValidator.GhostState state) {
        _tokenMaterialChanger.SetStateColor(gameObject, state);
    }
    public void PrepareTokenPlacement(GameObject gameObject, PlayerColor color) {
        var tokenView = gameObject.GetComponent<TokenView>();
        if (tokenView != null) {
            tokenView.SetVisualLayer("HoplonToken");
            tokenView.SetVisualTag("PlacedToken");
            tokenView.PlayerColor = color;
        } else {
            Debug.Log($"Unable to get TokenPrefabVisual component from {gameObject.name}");
        }
        _tokenMaterialChanger.SetPlayerMaterial(gameObject, color);        

        if (gameObject.TryGetComponent<Rigidbody>(out var rb)) {
            Object.Destroy(rb);
        } else {
            Debug.LogError($"No Rigidbody found in: {gameObject.name}");
        }
    }
    public void SetGhostState(GameObject tokenObject, TokenPlacementTerrainValidator.GhostState state) {
        _tokenMaterialChanger.SetStateColor(tokenObject, state);
    }
    public void ApplyPlacementVisuals(GameObject tokenObject, PlayerColor playerColor) {
        _tokenMaterialChanger.SetPlayerMaterial(tokenObject, playerColor);

        var visual = tokenObject.GetComponent<TokenView>();
        visual?.SetVisualLayer("HoplonToken");
        visual?.SetVisualTag("PlacedToken");

        if (tokenObject.TryGetComponent<Rigidbody>(out var rb)) {
            Object.Destroy(rb);
        } else {
            Debug.LogError($"No Rigidbody found in: {tokenObject.name}");
        }
    }
}

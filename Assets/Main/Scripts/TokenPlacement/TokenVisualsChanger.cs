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
    public void SetTokenMaterialGhostState(GameObject gameObject, TerrainValidator.GhostState state) {
        _tokenMaterialChanger.SetStateColor(gameObject, state);
    }
    public void PrepareTokenPlacement(TokenView token, PlayerColor color) {
        // Debug.Log($"Preparing token placement visuals for player color: {color}");
        token.SetLayer("HoplonToken");
        token.SetTag("PlacedToken");
        token.PlayerColor = color;

        _tokenMaterialChanger.SetPlayerMaterial(token, color);        

        TryDestroyRigidbody(token.gameObject);
    }
    public void SetGhostState(GameObject tokenObject, TerrainValidator.GhostState state) {
        _tokenMaterialChanger.SetStateColor(tokenObject, state);
    }
    public void ApplyPlacementVisuals(TokenView token, PlayerColor playerColor) {
        _tokenMaterialChanger.SetPlayerMaterial(token, playerColor);

        token?.SetLayer("HoplonToken");
        token?.SetTag("PlacedToken");

        TryDestroyRigidbody(token.gameObject);
    }
    private bool TryDestroyRigidbody(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Rigidbody>(out var rb)) {
            Object.Destroy(rb);
            return true;
        } else {
            Debug.LogWarning($"No Rigidbody found in: {gameObject.name}");
        }
        return false;
    }
}

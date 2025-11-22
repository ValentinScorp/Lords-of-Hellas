using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TokenMaterialChanger
{
    private TokenMaterialPalette _colorPallete;
    public TokenMaterialChanger(TokenMaterialPalette colorPalette) {
        _colorPallete = colorPalette;
    }
    public void SetGhostMaterial(GameObject gameObject) {
        GetRenderer(gameObject).material = _colorPallete.ghostMaterial;
    }

    public void SetStateColor(GameObject gameObject, TokenPlacementTerrainValidator.GhostState state) {
        Debug.Log(state.ToString());
        switch (state) {
            case TokenPlacementTerrainValidator.GhostState.Neutral:
                SetColor(gameObject, _colorPallete.ghostColorInit);
                break;
            case TokenPlacementTerrainValidator.GhostState.Allowed:
                SetColor(gameObject, _colorPallete.ghostColorOk);
                break;
            case TokenPlacementTerrainValidator.GhostState.Forbidden:
                SetColor(gameObject, _colorPallete.ghostColorError);
                break;
            default:
                SetColor(gameObject, _colorPallete.ghostColorInit);
                break;
        }
    }

    public void SetPlayerMaterial(GameObject gameObject, PlayerColor playerColor) {
        switch (playerColor) {
            case PlayerColor.Red:
                SetMaterial(gameObject, _colorPallete.redTokenMaterial);
                break;
            case PlayerColor.Blue:
                SetMaterial(gameObject, _colorPallete.blueTokenMaterial);
                break;
            case PlayerColor.Green:
                SetMaterial(gameObject, _colorPallete.greenTokenMaterial);
                break;
            case PlayerColor.Yellow:
                SetMaterial(gameObject, _colorPallete.yellowTokenMaterial);
                break;
            case PlayerColor.Purple:
                SetMaterial(gameObject, _colorPallete.purpleTokenMaterial);
                break;
            case PlayerColor.Brown:
                SetMaterial(gameObject, _colorPallete.brownTokenMaterial);
                break;
            default:
                SetMaterial(gameObject, _colorPallete.grayTokenMaterial);         
                break;
        }
    }

    private Renderer GetRenderer(GameObject gameObject) {
        var renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null) {
            Debug.LogWarning("No renderer found!");
        }
        return renderer;
    }

    private void SetMaterial(GameObject gameObject, Material material) {
        GetRenderer(gameObject).material = material;
    }

    private void SetColor(GameObject gameObject, Color color) {
        Debug.Log("setting color " +color.ToString());
        GetRenderer(gameObject).material.SetColor("_FresnelTint", color);
    }
}

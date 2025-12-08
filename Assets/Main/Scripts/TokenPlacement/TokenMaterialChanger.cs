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

    public void SetStateColor(GameObject gameObject, TerrainValidator.GhostState state) {
        Debug.Log(state.ToString());
        switch (state) {
            case TerrainValidator.GhostState.Neutral:
                SetColor(gameObject, _colorPallete.ghostColorInit);
                break;
            case TerrainValidator.GhostState.Allowed:
                SetColor(gameObject, _colorPallete.ghostColorOk);
                break;
            case TerrainValidator.GhostState.Forbidden:
                SetColor(gameObject, _colorPallete.ghostColorError);
                break;
            default:
                SetColor(gameObject, _colorPallete.ghostColorInit);
                break;
        }
    }

    public void SetPlayerMaterial(TokenView token, PlayerColor playerColor) {

        SetMaterial(token.gameObject, _colorPallete.GetPlayerMaterial(playerColor));
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

using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Colors/TokenMaterialPalette")]
public class TokenMaterialPalette : ScriptableObject
{
    public Color ghostColorInit;
    public Color ghostColorError;
    public Color ghostColorOk;

    public Material ghostMaterial;

    public Material grayTokenMaterial;
    public Material greenTokenMaterial;
    public Material blueTokenMaterial;
    public Material redTokenMaterial;
    public Material yellowTokenMaterial;
    public Material purpleTokenMaterial;
    public Material brownTokenMaterial;

    public Material GetPlayerMaterial(PlayerColor playerColor)
    {
        switch (playerColor)
        {
            case PlayerColor.Red:    return redTokenMaterial;
            case PlayerColor.Blue:   return blueTokenMaterial;
            case PlayerColor.Green:  return greenTokenMaterial;
            case PlayerColor.Yellow: return yellowTokenMaterial;
            case PlayerColor.Purple: return purpleTokenMaterial;
            case PlayerColor.Brown:  return brownTokenMaterial;
            default:                 return grayTokenMaterial;
        }
    }
}

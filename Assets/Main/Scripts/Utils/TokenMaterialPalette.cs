using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Colors/GeneralColorPalette")]
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
}

using System;
using UnityEngine;
using UnityEngine.UI;

internal class LandTokenIcon : MonoBehaviour
{
    [SerializeField] private Image _image;

    internal void SetColorLand(LandId color)
    {
        SetColor(GameContent.Instance.GetLandColor(color));
    }
    internal void SetColorHtml(string htmlColor)
    {
        if (ColorUtility.TryParseHtmlString(htmlColor, out var color)) {
            _image.color = color;
        } else {
            Debug.LogWarning("Unable to parse htmsColor in GloyrTokenIcon!");
        }
    }
    internal void Greyout()
    {
        SetColor(GameContent.PlayerColorPalette.Grey);
    }
    internal void SetColor(Color color)
    {
        _image.color = color;
    }
}

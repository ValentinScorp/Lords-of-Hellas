using UnityEngine;
using UnityEngine.UI;

public class HopliteIconView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private PlayerInfoPanelSpriteCatalog _spriteCatalog;

    private void Start()
    {
        _image.sprite = _spriteCatalog.HopliteUnavailable;
    }

}

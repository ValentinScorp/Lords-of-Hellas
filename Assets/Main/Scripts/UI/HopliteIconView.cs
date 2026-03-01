using UnityEngine;
using UnityEngine.UI;

public class HopliteIconView : MonoBehaviour
{
    [SerializeField] private Image _helmetImage;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;

    private void Awake()
    {
        SetHopliteOffBoard(false);
    }
    internal void SetHopliteOffBoard(bool isActive)
    {
        _helmetImage.sprite = isActive ? _activeSprite : _inactiveSprite;
    }
}

using UnityEngine;
using UnityEngine.UI;
public class PriestIconView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _offBoardSprite;
    [SerializeField] private Sprite _onBoardSprite;
    [SerializeField] private Sprite _inPoolSprite;

    internal enum Placement { OffBoard, InPool, OnBoard };    
    internal void SetInPool() => SetPlacement(PriestModel.Placement.InPool);

    private void Awake()
    {
        SetPlacement(PriestModel.Placement.OffBoard);
    }
    internal void SetPlacement(PriestModel.Placement placement)
    {
        if (_iconImage == null) return;

        _iconImage.sprite = placement switch
        {
            PriestModel.Placement.OffBoard => _offBoardSprite,
            PriestModel.Placement.InPool => _inPoolSprite,
            PriestModel.Placement.OnBoard => _onBoardSprite,
            _ => throw new System.ArgumentOutOfRangeException(nameof(placement), placement, null)
        };
    }

}

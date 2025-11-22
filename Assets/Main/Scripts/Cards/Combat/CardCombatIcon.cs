using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardCombatIcon : CardIcon
{
    [SerializeField] private Image _image;
    [SerializeField] private CardCombatIconDatabase _iconDatabase;
    [SerializeField] private CardCombatView _cardViewPrefab;
    
    private void ChangeImage(CardCombat.Type type) {
        _image.sprite = type switch {
            CardCombat.Type.Sword => _iconDatabase.Sword,
            CardCombat.Type.Bow => _iconDatabase.Bow,
            CardCombat.Type.Spear => _iconDatabase.Spear,
            CardCombat.Type.Shield => _iconDatabase.Shield,
            CardCombat.Type.Axe => _iconDatabase.Axe,
            CardCombat.Type.Sickle => _iconDatabase.Sickle,
            CardCombat.Type.Mace => _iconDatabase.Mace,
            CardCombat.Type.Torch => _iconDatabase.Torch,
            _ => null 
        };

        _image.enabled = _image.sprite != null;
    }

    protected override void OnInit(CardData card) {
        if (card is CardCombat combatCard) {
            ChangeImage(combatCard.WeaponType);
        }
    }
    protected override CardView OnCreateCardPreview() {
        if (_cardViewPrefab == null) {
            Debug.LogWarning("No preview prefab assigned!");
            return null;
        }

        var cardView = Instantiate(_cardViewPrefab, transform);
        return cardView;
    }
}

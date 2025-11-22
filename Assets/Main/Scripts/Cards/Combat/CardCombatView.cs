using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardCombatView : CardView
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descripionText;

    protected override void OnInit(CardData card) {
        if (card is CardCombat combat) {
            _titleText.text = combat.TacticName;
            _descripionText.text = combat.Effect;
        }
    }
}

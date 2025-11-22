using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMonsterView : CardView
{
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _regionText;
    [SerializeField] TextMeshProUGUI _evolveText;
    public CardData CardData;
    public string Title => _titleText.text;

    public void SetTitle(string title) {
        _titleText.SetText(title);
    }
    protected override void OnInit(CardData card) {
        if (card is CardMonster monster) {
            _titleText.text = monster.Title;
            _regionText.text = monster.Region;
            _evolveText.text = monster.evolveText;
        }
    }
}

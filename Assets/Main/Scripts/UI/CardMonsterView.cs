using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

internal class CardMonsterView : CardView
{
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _regionText;
    [SerializeField] TextMeshProUGUI _evolveText;
    internal CardData CardData;
    internal string Title => _titleText.text;

    internal void SetTitle(string title) {
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

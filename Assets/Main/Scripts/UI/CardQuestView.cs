using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardQuestView : CardView
{
    [SerializeField]
    private TMP_Text _titleText;

    [SerializeField]
    private TMP_Text _regionText;

    [SerializeField]
    private TMP_Text _rewardText;

    protected override void OnInit(CardData card) {
        if (card is CardQuest quest) {
            _titleText.text = quest.Title;
            _regionText.text = quest.Region;
            _rewardText.text = quest.reward;
        }
    }
}

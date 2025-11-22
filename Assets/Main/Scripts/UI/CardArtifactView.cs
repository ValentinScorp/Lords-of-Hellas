using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArtifactView : CardView
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descripionText;

    protected override void OnInit(CardData card) {
        if (card is CardArtifact artifactCard) {
            _titleText.text = artifactCard.Title;
            _descripionText.text = artifactCard.Description;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardArtifactIcon : CardIcon
{
    [SerializeField] private Image _image;
    [SerializeField] private CardArtifactIconDatabase _iconDatabase;
    [SerializeField] private CardArtifactView _cardViewPrefab;
    
    private void ChangeImage(CardArtifact.Type type) {
        Debug.Log("Changing artifact icon!" + type);
        _image.sprite = type switch {
            CardArtifact.Type.God => _iconDatabase.God,
            CardArtifact.Type.Neutral => _iconDatabase.Neutral,
            CardArtifact.Type.Monster => _iconDatabase.Monster,
            _ => null 
        };

        _image.enabled = _image.sprite != null;
    }

    protected override void OnInit(CardData card) {
        if (card is CardArtifact artifactCard) {
            ChangeImage(artifactCard.ArtifactType);
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

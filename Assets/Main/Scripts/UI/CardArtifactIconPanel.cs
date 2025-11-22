using System.Collections.Generic;
using UnityEngine;

public class CardArtifactIconPanel : MonoBehaviour
{
    [SerializeField] private CardArtifactIcon _iconPrefab;

    private List<CardArtifactIcon> _icons = new();

    public void AddCardIcon(CardArtifact card) {
        Debug.Log("Adding card icon");
        var icon = Instantiate(_iconPrefab, transform);
        icon.Init(card);
        _icons.Add(icon);
    }
    public void ClearPanel() {
        foreach (var icon in _icons)
            Destroy(icon.gameObject);

        _icons.Clear();
    }
}

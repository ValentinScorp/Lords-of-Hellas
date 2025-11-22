using System.Collections.Generic;
using UnityEngine;

public class CardCombatIconPanel : MonoBehaviour
{
    [SerializeField] private CardCombatIcon _iconPrefab;

    private List<CardCombatIcon> _icons = new();

    public void AddCardIcon(CardCombat card) {
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

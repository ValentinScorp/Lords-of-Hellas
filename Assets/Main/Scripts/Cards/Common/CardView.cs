using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CardView : MonoBehaviour, IPointerClickHandler
{
    protected event Action<CardView> _onSelected;
    protected CardData _data;

    public CardData Data => _data;

    public void Init(CardData card, System.Action<CardView> onSelect = null) {
        _data = card;
        if (onSelect != null) {
            _onSelected = onSelect;
        }

        OnInit(card); 
    }
    public void OnPointerClick(PointerEventData eventData) {
        _onSelected?.Invoke(this);
    }
    protected abstract void OnInit(CardData card);
}
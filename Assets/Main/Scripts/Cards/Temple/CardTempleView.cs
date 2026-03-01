using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class CardTempleView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private CardTempleDatabase _cardDatabase;

    private CardTemple _cardData;
    private Action<CardTemple> _onClick;

    internal void Init(CardTemple data, Action<CardTemple> OnClick = null) {
        _cardData = data;
        _onClick = OnClick;
        _backgroundImage.sprite = data.Image;

    }
    public void OnPointerClick(PointerEventData eventData) {
        _onClick?.Invoke(_cardData);
    }
    public void OnPointerEnter(PointerEventData eventData) {
    }
    public void OnPointerExit(PointerEventData eventData) { 
    }
    
}

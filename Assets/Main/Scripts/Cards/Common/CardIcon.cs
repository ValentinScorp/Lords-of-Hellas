using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class CardIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardView _cardView;
    private CardData _cardData;

    public void OnPointerEnter(PointerEventData eventData) {
        ShowCardPreview();
    }
    public void OnPointerExit(PointerEventData eventData) {
        HideCardPreview();
    }
    private void Update() {
        SetCardPreviewPosition();
    }
    public void Init(CardData card) {
        _cardData = card;
        OnInit(card);
    }
    private void ShowCardPreview() {       
        _cardView = OnCreateCardPreview();
        _cardView.Init(_cardData);
        SetCardPreviewPosition();
    }
    private void HideCardPreview() {
        if (_cardView != null) {
            Destroy(_cardView.gameObject);
            _cardView = null;
        }
    }
    private void SetCardPreviewPosition() {
        if (_cardView != null) {
            var rt = _cardView.GetComponent<RectTransform>();
            rt.pivot = Vector2.zero;
            var pointerPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;
            rt.position = new Vector3(pointerPosition.x, pointerPosition.y, 0) + new Vector3(15, 15);
        }
    }
    protected abstract void OnInit(CardData card);
    protected abstract CardView OnCreateCardPreview();
}

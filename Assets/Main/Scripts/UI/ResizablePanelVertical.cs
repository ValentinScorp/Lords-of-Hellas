using UnityEngine;
using UnityEngine.EventSystems;

public class ResizablePanelVertiacal : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform panel;
    public float minHeight = 20f;
    public float maxHeight = 600f;

    private Vector2 _startMousePos;
    private float _startHeight;

    public void OnPointerDown(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out _startMousePos);
        _startHeight = panel.sizeDelta.y;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 currentMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out currentMousePos);
        float deltaY = currentMousePos.y - _startMousePos.y;

        float newHeight = Mathf.Clamp(_startHeight + deltaY, minHeight, maxHeight);
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, newHeight);
    }
}

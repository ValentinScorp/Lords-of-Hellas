using UnityEngine;
using UnityEngine.EventSystems;

public class ResizablePanelHorizontal : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform panel;
    public float minWidth = 100f;
    public float maxWidth = 600f;

    private Vector2 _startMousePos;
    private float _startWidth;

    public void OnPointerDown(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out _startMousePos);
        _startWidth = panel.sizeDelta.x;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 currentMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out currentMousePos);
        float deltaX = currentMousePos.x - _startMousePos.x;

        float newWidth = Mathf.Clamp(_startWidth + deltaX, minWidth, maxWidth);
        panel.sizeDelta = new Vector2(newWidth, panel.sizeDelta.y);
    }
}

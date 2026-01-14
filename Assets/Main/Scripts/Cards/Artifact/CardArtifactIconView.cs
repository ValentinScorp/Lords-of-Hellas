using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardArtifactIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardArtifactView _cardPreviewPrefab;

    private GameObject _currentPreview;

    public void OnPointerEnter(PointerEventData eventData) {
        ShowPreview();
    }
    public void OnPointerExit(PointerEventData eventData) {
        HidePreview();
    }
    private void Update() {
        if (_currentPreview != null) {
            Vector2 offset = new Vector2(10f, 10f);
            var pointerPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;
            _currentPreview.transform.position = new Vector3(pointerPosition.x, pointerPosition.y, 0) + (Vector3)offset;
        }
    }
    private void ShowPreview() {
        if (_cardPreviewPrefab == null) {
            Debug.LogWarning("No preview prefab assigned!");
            return;
        }

        if (_currentPreview == null) {
            // ��������� ������� �����
            _currentPreview = Instantiate(_cardPreviewPrefab.gameObject, transform.root);
            var rectTransform = _currentPreview.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0f, 0f);

            var pointerPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;
            _currentPreview.transform.position = new Vector3(pointerPosition.x, pointerPosition.y, 0);

            rectTransform.anchoredPosition += new Vector2(10f, 10f);
        }
    }
    private void HidePreview() {
        if (_currentPreview != null) {
            Destroy(_currentPreview);
            _currentPreview = null;
        }
    }       
}

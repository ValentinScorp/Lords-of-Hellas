using UnityEngine;
using UnityEngine.EventSystems;

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
            _currentPreview.transform.position = Input.mousePosition + (Vector3)offset;
        }
    }
    private void ShowPreview() {
        if (_cardPreviewPrefab == null) {
            Debug.LogWarning("No preview prefab assigned!");
            return;
        }

        if (_currentPreview == null) {
            // Створюємо інстанс прев’ю
            _currentPreview = Instantiate(_cardPreviewPrefab.gameObject, transform.root);
            var rectTransform = _currentPreview.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0f, 0f);

            _currentPreview.transform.position = Input.mousePosition;

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

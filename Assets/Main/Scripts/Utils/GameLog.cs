using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameLog : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _logEntryPrefab;

    public void AddLogEntry(string message) {
        GameObject entry = Instantiate(_logEntryPrefab, _contentParent);
        TextMeshProUGUI text = entry.GetComponent<TextMeshProUGUI>();
        text.text = message;

        Canvas.ForceUpdateCanvases(); 
        _scrollRect.verticalNormalizedPosition = 0f; 
    }
}


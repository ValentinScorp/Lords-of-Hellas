using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _logEntryPrefab;

    private void Start() {
        GameLogger.Instance.OnEventTrigger += AddLogEntry;
    }

    public void AddLogEntry(string message) {
        GameObject entry = Instantiate(_logEntryPrefab, _contentParent);
        TextMeshProUGUI text = entry.GetComponent<TextMeshProUGUI>();
        text.text = message;

        Canvas.ForceUpdateCanvases();
        
        GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }
}

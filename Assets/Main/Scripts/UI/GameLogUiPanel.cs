using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogUiPanel : UiPanel
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _logEntryPrefab;
    protected override void Awake()
    {
        base.Awake();
        Show(false);
    }
    private void Start() {
        GameLogger.Instance.OnEventTrigger += AddLogEntry;
    }
    protected override void OnDestroy()
    {
        GameLogger.Instance.OnEventTrigger -= AddLogEntry;
        base.OnDestroy();
    }

    public void AddLogEntry(string message) {
        GameObject entry = Instantiate(_logEntryPrefab, _contentParent);
        TextMeshProUGUI text = entry.GetComponent<TextMeshProUGUI>();
        text.text = message;

        Canvas.ForceUpdateCanvases();
        
        GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class MonumentManager : MonoBehaviour
{
    [SerializeField] RectTransform _zeus;
    [SerializeField] RectTransform _hermes;
    [SerializeField] RectTransform _athena;
    private Dictionary<MonumentModel.GodType, (MonumentModel monument, RectTransform rectTransform)> _monuments;

    private void Awake() {
        _monuments = new Dictionary<MonumentModel.GodType, (MonumentModel, RectTransform)>
        {
            { MonumentModel.GodType.Zeus, (new MonumentModel(MonumentModel.GodType.Zeus), _zeus) },
            { MonumentModel.GodType.Hermes, (new MonumentModel(MonumentModel.GodType.Hermes), _hermes) },
            { MonumentModel.GodType.Athena, (new MonumentModel(MonumentModel.GodType.Athena), _athena) }
        };
    }
    internal int GetMonumentLevel(MonumentModel.GodType type) {
        if (_monuments.TryGetValue(type, out var data))
            return data.monument.Level;

        Debug.LogError($"Can't get level of monument type: {type}");
        return 0;
    }

    internal void IncreaseMonumentLevel(MonumentModel.GodType type) {
        if (_monuments.TryGetValue(type, out var data)) {
            data.monument.IncreaseLevel();
            data.rectTransform.GetComponent<TMP_Text>().text = data.monument.Level.ToString();
        } else {
            Debug.LogError($"Can't increase monument level or set text for: {type}");
        }
    }

    internal void ClickButton() {
        IncreaseMonumentLevel(MonumentModel.GodType.Zeus);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonumentManager : MonoBehaviour
{
    [SerializeField] RectTransform _zeus;
    [SerializeField] RectTransform _hermes;
    [SerializeField] RectTransform _athena;
    private Dictionary<MonumentType, (Monument monument, RectTransform rectTransform)> _monuments;

    private void Awake() {
        _monuments = new Dictionary<MonumentType, (Monument, RectTransform)>
        {
            { MonumentType.Zeus, (new Monument(MonumentType.Zeus), _zeus) },
            { MonumentType.Hermes, (new Monument(MonumentType.Hermes), _hermes) },
            { MonumentType.Athena, (new Monument(MonumentType.Athena), _athena) }
        };
    }
    public int GetMonumentLevel(MonumentType type) {
        if (_monuments.TryGetValue(type, out var data))
            return data.monument.Level;

        Debug.LogError($"Can't get level of monument type: {type}");
        return 0;
    }

    public void IncreaseMonumentLevel(MonumentType type) {
        if (_monuments.TryGetValue(type, out var data)) {
            data.monument.IncreaseLevel();
            data.rectTransform.GetComponent<TMP_Text>().text = data.monument.Level.ToString();
        } else {
            Debug.LogError($"Can't increase monument level or set text for: {type}");
        }
    }

    public void ClickButton() {
        IncreaseMonumentLevel(MonumentType.Zeus);
    }
}

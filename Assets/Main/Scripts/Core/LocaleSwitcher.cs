using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSwitcher : MonoBehaviour
{
    [SerializeField] Button _ukButton;
    [SerializeField] Button _enButton;

    private const string LocaleKey = "locale";

    private void Awake()
    {
        _ukButton.onClick.AddListener(SetUkrainian);
        _enButton.onClick.AddListener(SetEnglish);

        StartCoroutine(LoadSavedLocale());
    }
    private void Oestroy()
    {
        _ukButton.onClick.RemoveListener(SetUkrainian);
        _enButton.onClick.RemoveListener(SetEnglish);
    }
    public void SetEnglish() => StartCoroutine(SetLocale("en"));
    public void SetUkrainian() => StartCoroutine(SetLocale("uk"));
    private IEnumerator LoadSavedLocale()
    {
        yield return LocalizationSettings.InitializationOperation;

        string savedCode = PlayerPrefs.GetString(LocaleKey, "en");
        var locale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == savedCode);

        if (locale != null) {
            LocalizationSettings.SelectedLocale = locale;
        }
    }
    private IEnumerator SetLocale(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        var locale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == code);

        if (locale != null) {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString("locale", code);
            PlayerPrefs.Save();
        } else {
            Debug.LogWarning($"Locale not found: {code}");
        }
    }
}

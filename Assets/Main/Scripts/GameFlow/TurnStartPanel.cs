using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnStartPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] Button _okButton;

    private Action _Completed;
    private void Awake()
    {
        Show(false);
        SceneUiRegistry.Register(this);
        _okButton.onClick.AddListener(HandleOkButton);
    }
    private void OnDestroy()
    {
        _okButton.onClick.RemoveListener(HandleOkButton);
        SceneUiRegistry.Unregister<TurnStartPanel>();
    }
    public void Launch(Player player, Action Completed)
    {
        _Completed = Completed;
        Color color = GameContent.Instance.GetPlayerColor(player.Color);
        var hexColor = ColorUtility.ToHtmlStringRGB(color); 
        _title.text = $"Хід <color=#{hexColor}>колір</color> гравця!";
        Show(true);
    }
    public void HandleOkButton()
    {
        Show(false);
        _Completed.Invoke();
        _Completed = null;
    }
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
}

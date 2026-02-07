using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionControlPanel : MonoBehaviour
{
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private Button _buttonUndo;
    [SerializeField] private Button _buttonDone;
    private Action _onCancel;
    private Action _onUndo;
    private Action _onDone;

    private void Awake()
    {
        SetInteractable(false);
        Show(false);
        SceneUIRegistry.Register(this);     
    }
    private void OnDestroy()
    {
        SceneUIRegistry.Unregister<ActionControlPanel>();
    }
    
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
    public void Bind(Action onDone, Action onUndo, Action onCancel)
    {
        Unbind();

        _onDone = onDone;
        _onUndo = onUndo;
        _onCancel = onCancel;

        if (_onDone != null) _buttonDone.onClick.AddListener(OnDoneClicked);
        if (_onUndo != null) _buttonUndo.onClick.AddListener(OnUndoClicked);
        if (_onCancel != null) _buttonCancel.onClick.AddListener(OnCancelClicked);

        SetInteractable(true);
    }
    public void Unbind()
    {
        _buttonDone.onClick.RemoveListener(OnDoneClicked);
        _buttonUndo.onClick.RemoveListener(OnUndoClicked);
        _buttonCancel.onClick.RemoveListener(OnCancelClicked);

        _onDone = null;
        _onUndo = null;
        _onCancel = null;

        SetInteractable(false);
    }

    public void SetUndoInteractable(bool value) => _buttonUndo.interactable = value;

    private void SetInteractable(bool interactable)
    {
        _buttonCancel.interactable = interactable;
        _buttonUndo.interactable = interactable;
        _buttonDone.interactable = interactable;
    }

    private void OnDoneClicked() => _onDone?.Invoke();
    private void OnUndoClicked() => _onUndo?.Invoke();
    private void OnCancelClicked() => _onCancel?.Invoke();

}

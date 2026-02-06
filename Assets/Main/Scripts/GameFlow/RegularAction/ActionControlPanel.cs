using UnityEngine;
using UnityEngine.UI;

public class ActionControlPanel : MonoBehaviour
{
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private Button _buttonUndo;
    [SerializeField] private Button _buttonDone;

    private ActionControlPanelController _panelController;

    private void Awake()
    {
        SetInteractable(false);
        _panelController = new ActionControlPanelController(this);
        ServiceLocator.Register(_panelController);

        _buttonCancel.onClick.AddListener(_panelController.OnCancel);
        _buttonUndo.onClick.AddListener(_panelController.OnUndo);
        _buttonDone.onClick.AddListener(_panelController.OnDone);        
    }
    private void OnDestroy()
    {
        if (_panelController is not null) {
            _buttonCancel.onClick.RemoveListener(_panelController.OnCancel);
            _buttonUndo.onClick.RemoveListener(_panelController.OnUndo);
            _buttonDone.onClick.RemoveListener(_panelController.OnDone);     
        }   
    }
    public void Show(bool show)
    {
        gameObject.SetActive(show);
        SetInteractable(show);
    }
    private void SetInteractable(bool interactable)
    {
        _buttonCancel.interactable = interactable;
        _buttonUndo.interactable = interactable;
        _buttonDone.interactable = interactable;
    }

}

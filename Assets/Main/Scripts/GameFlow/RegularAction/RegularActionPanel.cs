using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RegularActionPanel : MonoBehaviour
{
    [SerializeField] private Button _heroMovementButton;
    [SerializeField] private Button _hopliteMovementButton;
    [SerializeField] private Button _usingArtifactButton;
    [SerializeField] private Button _prayerButton;
    [SerializeField] private Button _endActionButton;

    private RegularActionManager _manager;

    private void Awake()
    {
        _heroMovementButton.interactable = false;
        _hopliteMovementButton.interactable = false;
        _usingArtifactButton.interactable = false;
        _prayerButton.interactable = false;
        _endActionButton.interactable = false;
    }
    private void Start()
    {
        var _manager = ServiceLocator.Get<RegularActionManager>();
        if (_manager is not null) {
            _manager.Bind(this);
            _heroMovementButton.onClick.AddListener(_manager.HeroMoveStart);
            _hopliteMovementButton.onClick.AddListener(_manager.HopliteMoveStart);
        }
        Show(false);
    }
    private void OnDestroy()
    {
        if (_manager is not null) {
            _heroMovementButton.onClick.RemoveListener(_manager.HeroMoveStart);
            _hopliteMovementButton.onClick.RemoveListener(_manager.HopliteMoveStart);
        }
    }    

    public void SetHeroMoveButtonInteractable(bool interactable)
    {
        _heroMovementButton.interactable = interactable;
    }
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
}

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ActionRegularPanel : EventListener
{
    [SerializeField] private Button _heroMovementButton;
    [SerializeField] private Button _hopliteMovementButton;
    [SerializeField] private Button _usingArtifactButton;
    [SerializeField] private Button _prayerButton;
    [SerializeField] private Button _endActionButton;

    private ActionRegular _regularAction;
    private Player _player;

    protected override Type EventType => typeof(ActionRegular);

    protected override void Awake() {
        base.Awake();
        HidePanel();
    }

    protected override void HandleEvent(IGameEvent evt) {
        if (evt is not ActionRegular regularAction) return;
        _regularAction = regularAction;
        _player = regularAction.Player;
        ShowPanel();
        UpdateButtonInteractability();
    }
    private void UpdateButtonInteractability() {
        _heroMovementButton.interactable = _regularAction.HeroSteps > 0;
        _hopliteMovementButton.interactable = _regularAction.HoplitesMoveLeft() > 0;
        _usingArtifactButton.interactable = _regularAction.ChargedArtifacts().Any();
        _prayerButton.interactable = _regularAction.Player.PriestsInPool > 0;
    }
    private void ShowPanel() => gameObject.SetActive(true);
    private void HidePanel() => gameObject.SetActive(false);
}

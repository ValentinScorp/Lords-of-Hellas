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

    public void UpdateButtonInteractability(RegularAction regularAction) {
        _heroMovementButton.interactable = regularAction.HeroSteps > 0;
        _hopliteMovementButton.interactable = regularAction.HoplitesMoveLeft() > 0;
        _usingArtifactButton.interactable = regularAction.ChargedArtifacts().Any();
        _prayerButton.interactable = regularAction.Player.PriestsInPool > 0;
    }
    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);
    
}

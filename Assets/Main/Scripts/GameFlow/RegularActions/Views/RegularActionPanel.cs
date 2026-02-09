
using UnityEngine;
using UnityEngine.UI;

public class RegularActionPanel : MonoBehaviour
{
    [SerializeField] private Button _heroMovementButton;
    [SerializeField] private Button _hopliteMovementButton;
    [SerializeField] private Button _usingArtifactButton;
    [SerializeField] private Button _prayerButton;
    [SerializeField] private Button _endActionButton;

    private void Awake()
    {
        SceneUiRegistry.Register(this);

        _heroMovementButton.interactable = false;
        _hopliteMovementButton.interactable = false;
        _usingArtifactButton.interactable = false;
        _prayerButton.interactable = false;
        _endActionButton.interactable = true;
    }
    private void Start()
    {
        Show(false);
    }
    private void OnDestroy()
    {
        SceneUiRegistry.Unregister<RegularActionPanel>();
    }
    public void Bind(RegularActionController controller)
    {
        if (controller is not null) {
            _heroMovementButton.onClick.AddListener(controller.HeroMoveStart);
            _hopliteMovementButton.onClick.AddListener(controller.HoplitesMoveStart);
            _usingArtifactButton.onClick.AddListener(controller.ArtifactsUseAction);
            _prayerButton.onClick.AddListener(controller.PrayerAction);
            _endActionButton.onClick.AddListener(controller.OnCompleteAction);
        }
    }
    public void Undbind(RegularActionController controller)
    {
        if (controller is not null) {
            _heroMovementButton.onClick.RemoveListener(controller.HeroMoveStart);
            _hopliteMovementButton.onClick.RemoveListener(controller.HoplitesMoveStart);
            _usingArtifactButton.onClick.RemoveListener(controller.ArtifactsUseAction);
            _prayerButton.onClick.RemoveListener(controller.PrayerAction);
            _endActionButton.onClick.RemoveListener(controller.OnCompleteAction);
        }
    }
    public void SetHeroMoveButtonInteractable(bool interactable)
    {
        _heroMovementButton.interactable = interactable;
    }
    public void SetHoplitesMoveButtonInteractable(bool interactable)
    {
        _hopliteMovementButton.interactable = interactable;
    }
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
}

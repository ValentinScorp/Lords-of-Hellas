using UnityEngine;
using UnityEngine.UI;

public class PreparePanel : MonoBehaviour
{
    [SerializeField] private Button _healInjury;
    [SerializeField] private Button _drawCombatCard;
    [SerializeField] private Button _recruitHoplite;
    private void Awake()
    {
        SceneUiRegistry.Register(this);
        Show(false);
    }
    private void OnDestroy()
    {
        SceneUiRegistry.Unregister<PreparePanel>();
    }
    public void Bind(SpecialActionPrepareController controller)
    {
        if (controller is null) return;

        _healInjury?.onClick.AddListener(controller.OnHealInjuryPressed);
        _drawCombatCard?.onClick.AddListener(controller.OnDrawCombatCardPressed);
        _recruitHoplite?.onClick.AddListener(controller.OnRecruitHoplitePressed);
    }
    public void Unbind(SpecialActionPrepareController controller)
    {
        if (controller is null) return;

        _healInjury?.onClick.RemoveListener(controller.OnHealInjuryPressed);
        _drawCombatCard?.onClick.RemoveListener(controller.OnDrawCombatCardPressed);
        _recruitHoplite?.onClick.RemoveListener(controller.OnRecruitHoplitePressed);
    }
    public void Show(bool value)
    {
        gameObject.SetActive(value);
    }
}

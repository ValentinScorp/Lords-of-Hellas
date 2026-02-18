using UnityEngine;
using UnityEngine.UI;

public class SpecialActionsPanel : UiPanel
{
    [SerializeField] private Button _prepare;
    [SerializeField] private Button _hunt;
    [SerializeField] private Button _usurp;
    [SerializeField] private Button _buildTemple;
    [SerializeField] private Button _recruit;
    [SerializeField] private Button _march;
    [SerializeField] private Button _buildMonument;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        Show(false);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void Bind(SpecialActionsController controller)
    {
        if (controller is not null) {
            _prepare.onClick.AddListener(controller.OnPreparePressed);
            _hunt.onClick.AddListener(controller.OnHuntPressed);
            _usurp.onClick.AddListener(controller.OnUsurpPressed);
            _buildTemple.onClick.AddListener(controller.OnBuildTemplePressed);
            _recruit.onClick.AddListener(controller.OnRecruitPressed);
            _march.onClick.AddListener(controller.OnMarchPressed);
            _buildMonument.onClick.AddListener(controller.OnBuildMonumentPressed);
        }
    }
    public void Undbind(SpecialActionsController controller)
    {
        if (controller is not null) {
            _prepare.onClick.RemoveListener(controller.OnPreparePressed);
            _hunt.onClick.RemoveListener(controller.OnHuntPressed);
            _usurp.onClick.RemoveListener(controller.OnUsurpPressed);
            _buildTemple.onClick.RemoveListener(controller.OnBuildTemplePressed);
            _recruit.onClick.RemoveListener(controller.OnRecruitPressed);
            _march.onClick.RemoveListener(controller.OnMarchPressed);
            _buildMonument.onClick.RemoveListener(controller.OnBuildMonumentPressed);
        }
    }
}

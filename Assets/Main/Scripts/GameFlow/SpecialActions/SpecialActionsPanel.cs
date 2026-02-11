using UnityEngine;
using UnityEngine.UI;

public class SpecialActionsPanel : MonoBehaviour
{
    [SerializeField] private Button _prepare;
    [SerializeField] private Button _hunt;
    [SerializeField] private Button _usurp;
    [SerializeField] private Button _buildTemple;
    [SerializeField] private Button _recruit;
    [SerializeField] private Button _march;
    [SerializeField] private Button _buildMonument;

    private void Awake()
    {
        SceneUiRegistry.Register(this);
    }
    private void Start()
    {
        Show(false);
    }
    private void Oestroy()
    {
        SceneUiRegistry.Unregister<SpecialActionsPanel>();
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
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
}

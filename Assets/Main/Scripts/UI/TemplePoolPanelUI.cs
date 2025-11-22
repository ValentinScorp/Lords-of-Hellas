using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemplePoolPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cardInfoText;
    [SerializeField] private TextMeshProUGUI _draftInfoText;
    [SerializeField] private Button _randomCardButton;
    [SerializeField] private Button _nextStepButton;

    private TemplePool _templePool;

    private void Awake() {
        //GameContext.Instance.Initialize();
        //_templePool = GameContext.Instance.TemplePool;
    }

    private void OnEnable() {
        if (_templePool == null) {
            return;
        }
        _templePool.OnTemplePoolChanged += UpdateUI;
        _nextStepButton.onClick.AddListener(_templePool.AdvanceStep);

        UpdateUI();
    }

    private void OnDisable() {
        if (_templePool == null) {
            return;
        }
        _templePool.OnTemplePoolChanged -= UpdateUI;
        _nextStepButton.onClick.RemoveListener(_templePool.AdvanceStep);
    }

    private void UpdateUI() {
        if (_templePool == null) {
            return;
        }
        _cardInfoText.text = $"Oracle Blessing: {_templePool.CurrentCard.OracleBonus.Description()}";
        _draftInfoText.text = _templePool.CurrentStepText();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelView : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] Button _startPlacement_Button;
    [SerializeField] Button _testPlacement_Button;
    [SerializeField] Button _templeCardToggle_Button;

    public void ShowPanel(bool show) {
        gameObject.SetActive(show);
    }

}

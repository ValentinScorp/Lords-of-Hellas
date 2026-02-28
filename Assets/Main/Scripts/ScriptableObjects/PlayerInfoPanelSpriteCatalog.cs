using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerInfoPanel", 
    menuName = "Game/Databases/SpriteCatalog"
)]
public class PlayerInfoPanelSpriteCatalog : ScriptableObject
{
    [SerializeField] private Sprite _hopliteAvailable;
    public Sprite HopliteAvailable => _hopliteAvailable;
    [SerializeField] public Sprite _hopliteUnavailable;
    public Sprite HopliteUnavailable => _hopliteUnavailable;

}

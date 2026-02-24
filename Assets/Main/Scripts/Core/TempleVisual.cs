using UnityEngine;

public class TempleModel : TokenModel
{
    public TempleModel() : base(TokenType.Temple, PlayerColor.Gray)
    {
    }
}
public class TempleVisual : MonoBehaviour, IPlaceableVisual
{
    private PlayerColorable _playerColorable;
    private TempleModel _model = new TempleModel();
    public TokenModel Model => _model;

    private void Awake()
    {
        _playerColorable = GetComponent<PlayerColorable>();
        if (_playerColorable is null) {
            Debug.LogWarning($"Unable get PlayerColorabel component from TempleView!");
        }        
    }
    internal void SetOwnerColor(PlayerColor ownedBy)
    {
        _playerColorable.SetPlayerColor(ownedBy);
    }
}

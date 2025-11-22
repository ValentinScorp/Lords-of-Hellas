using UnityEngine;

public class TokenPlacementInfo
{
    private PlayerColor _playerColor;
    private RegionId _regionId;
    //private TokenEntity _tokenEntity;
    private GameObject _gameObject;
    private TokenType _tokenType;

    public GameObject TokenObject => _gameObject;
    public PlayerColor PlayerColor => _playerColor;
    public RegionId RegionId => _regionId;
    public TokenType TokenType => _tokenType;
    public TokenPlacementInfo(GameObject gameObject, TokenType type, PlayerColor playerColor, RegionId regionId) {
        _gameObject = gameObject;
        _tokenType = type;
        _playerColor = playerColor;
        _regionId = regionId;
    }
}

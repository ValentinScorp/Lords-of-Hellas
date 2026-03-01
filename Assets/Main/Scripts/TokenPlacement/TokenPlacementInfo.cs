using UnityEngine;

internal class TokenPlacementInfo
{
    private PlayerColor _playerColor;
    private RegionId _regionId;
    //private TokenEntity _tokenEntity;
    private GameObject _gameObject;
    private TokenType _tokenType;

    internal GameObject TokenObject => _gameObject;
    internal PlayerColor PlayerColor => _playerColor;
    internal RegionId RegionId => _regionId;
    internal TokenType TokenType => _tokenType;
    internal TokenPlacementInfo(GameObject gameObject, TokenType type, PlayerColor playerColor, RegionId regionId) {
        _gameObject = gameObject;
        _tokenType = type;
        _playerColor = playerColor;
        _regionId = regionId;
    }
}

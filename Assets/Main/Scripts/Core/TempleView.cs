using UnityEngine;

public class TempleView : MonoBehaviour, IPlaceableView
{
    private PlayerColorable _playerColorable;    

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

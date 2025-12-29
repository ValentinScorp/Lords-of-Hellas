using UnityEngine;
public class TokenHolder
{
    private TokenView _tokenView = null;
    public TokenView TokenView => _tokenView;
    public TokenModel TokenModel => _tokenView.ViewModel.Model;
    public TokenHolder() {
    }
    public bool HasObject() {
        if (_tokenView == null) {
            return false;
        }
        return true;
    }
    public void AttachToken(TokenView token) {
        _tokenView = token;
    }
    public void SetGameObjectPosition(Vector3? newPosition) {
        if (newPosition.HasValue) {
            _tokenView.transform.position = newPosition.Value;
        } else {
            Debug.LogError("Can't set token new position, position is null!");
        }
    }
    public Vector3? GetGameObjectPosition() {
        if (HasObject()) {
            return _tokenView.transform.position;
        }
        Debug.LogWarning("TokenHolder: Token or GameObject is missing.");
        return null;
    }
    public TokenView UnattachTokenAt(Vector3? position) {
        SetGameObjectPosition(position);
        return UnattachToken();
    }
    public TokenView UnattachToken() {
        if (HasObject()) {
            var token = _tokenView;
            _tokenView = null;
            return token;
        } else {
            //Debug.LogWarning("Unable to place empty object!");
        }
        return null;
    }  
    public void DestroyTokenView() {
        if (HasObject()) {
            Object.Destroy(_tokenView.gameObject);
            _tokenView = null;
        }
    }
}

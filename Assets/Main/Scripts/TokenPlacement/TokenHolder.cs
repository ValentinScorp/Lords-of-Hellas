using UnityEngine;
internal class TokenHolder
{
    private TokenView _tokenView = null;
    internal TokenView TokenView => _tokenView;
    internal TokenModel TokenModel => _tokenView.Model;
    internal TokenHolder() {
    }
    internal bool HasObject() {
        if (_tokenView == null) {
            return false;
        }
        return true;
    }
    internal void AttachToken(TokenView token) {
        _tokenView = token;
    }
    internal void SetGameObjectPosition(Vector3? newPosition) {
        if (newPosition.HasValue) {
            _tokenView.transform.position = newPosition.Value;
        } else {
            Debug.LogError("Can't set token new position, position is null!");
        }
    }
    internal Vector3? GetGameObjectPosition() {
        if (HasObject()) {
            return _tokenView.transform.position;
        }
        Debug.LogWarning("TokenHolder: Token or GameObject is missing.");
        return null;
    }
    internal TokenView UnattachTokenAt(Vector3? position) {
        SetGameObjectPosition(position);
        return UnattachToken();
    }
    internal TokenView UnattachToken() {
        if (HasObject()) {
            var token = _tokenView;
            _tokenView = null;
            return token;
        } else {
            //Debug.LogWarning("Unable to place empty object!");
        }
        return null;
    }  
    internal void DestroyTokenView() {
        if (HasObject()) {
            Object.Destroy(_tokenView.gameObject);
            _tokenView = null;
        }
    }
}

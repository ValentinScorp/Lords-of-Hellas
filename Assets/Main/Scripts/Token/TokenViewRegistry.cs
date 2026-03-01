using System.Collections.Generic;
using UnityEngine;

internal class TokenViewRegistry
{
    private readonly Dictionary<TokenModel, TokenView> _views = new();

    internal void Register(TokenModel m, TokenView view)
    {
        if (m == null || view == null) return;
        _views[m] = view;
    }

    internal void Unregister(TokenModel m, TokenView view)
    {
        if (m == null || view == null) return;
        if (_views.TryGetValue(m, out var current) && current == view) {
            _views.Remove(m);
        }
    }

    internal bool TryDestroy(TokenModel m)
    {
        if (m != null && _views.TryGetValue(m, out var view)) {
            Object.Destroy(view.gameObject);
            _views.Remove(m);
            return true;
        }
        return false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TokenViewRegistry
{
    private readonly Dictionary<TokenViewModel, TokenView> _views = new();

    public void Register(TokenViewModel vm, TokenView view)
    {
        if (vm == null || view == null) return;
        _views[vm] = view;
    }

    public void Unregister(TokenViewModel vm, TokenView view)
    {
        if (vm == null || view == null) return;
        if (_views.TryGetValue(vm, out var current) && current == view) {
            _views.Remove(vm);
        }
    }

    public bool TryDestroy(TokenViewModel vm)
    {
        if (vm != null && _views.TryGetValue(vm, out var view)) {
            Object.Destroy(view.gameObject);
            _views.Remove(vm);
            return true;
        }
        return false;
    }
}

using System;
using UnityEngine;

public class TokenViewModel : IDisposable
{
    public SpawnPoint SpawnPoint { get; private set; }
    public TokenModel Model { get; private set; }
    public TokenView View {get; private set;}

    public RegionId RegionId => Model != null ? Model.RegionId : RegionId.Unknown;

    public TokenViewModel(TokenModel model, TokenView view)
    {
        if (model is null)
            Debug.LogWarning("Setting Model at TokenViewModel as null!");
        
        if (view is null)        
            Debug.LogWarning("Setting View at TokenViewModel as null!");

        SpawnPoint = new();
        Model = model;
        View = view;
    }
    public virtual void Dispose() { }
}

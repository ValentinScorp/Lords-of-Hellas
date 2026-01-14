using System;
using UnityEngine;

public class TokenViewModel : IDisposable
{
    public SpawnPoint SpawnPoint { get; private set; }
    public TokenModel Model { get; private set; }

    public RegionId RegionId => Model != null ? Model.RegionId : RegionId.Unknown;

    public TokenViewModel(TokenModel model)
    {
        if (model is null)
            Debug.LogWarning("Setting Model at TokenViewModel as null!");        

        SpawnPoint = new();
        Model = model;
    }
    public virtual void Dispose() { }
}

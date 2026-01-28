using System;
using UnityEngine;

public class TokenViewModel : IDisposable
{
    public Vector3 WorldPosition { get; private set; }
    public TokenNest TokenNest { get; private set; }
    public TokenModel Model { get; private set; }

    public enum VisualState
    {
        Ghost,
        Placed
    } 
    public VisualState GhostState { get; private set; }

    public event Action<Vector3> WorldPositionChanged;
    public event Action<VisualState, PlayerColor> VisualStateChanged;

    public RegionId RegionId => Model != null ? Model.RegionId : RegionId.Unknown;

    public TokenViewModel(TokenModel model)
    {
        if (model is null)
            Debug.LogWarning("Setting Model at TokenViewModel as null!");        

        GhostState = VisualState.Ghost;
        TokenNest = new();
        Model = model;
    }
    public void Place(TokenNest nest)
    {
        TokenNest = nest;
        nest.Occupy();
        SetWorldPosition(nest.Position);
        SetPlacedVisual();
    }
    public void SetWorldPosition(Vector3 position)
    {
        WorldPosition = position;
        WorldPositionChanged?.Invoke(position);
    }
    public void SetPlacedVisual()
    {
        GhostState = VisualState.Placed;
        VisualStateChanged?.Invoke(GhostState, Model.PlayerColor);
    }
    public virtual void Dispose() { }
}

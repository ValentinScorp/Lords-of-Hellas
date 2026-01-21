using System;
using UnityEngine;

public class TokenViewModel : IDisposable
{
    public Vector3 WorldPosition { get; private set; }
    public SpawnPoint SpawnPoint { get; private set; }
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
        SpawnPoint = new();
        Model = model;
    }
    public void Place(SpawnPoint spawnPoint)
    {
        SpawnPoint = spawnPoint;
        spawnPoint.Occupy();
        SetWorldPosition(spawnPoint.Position);
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
        if (Model is IPlayerOwned playerOwnedModel)
            VisualStateChanged?.Invoke(GhostState, playerOwnedModel.PlayerColor);
        else
            VisualStateChanged?.Invoke(GhostState, PlayerColor.Gray);
    }
    public virtual void Dispose() { }
}

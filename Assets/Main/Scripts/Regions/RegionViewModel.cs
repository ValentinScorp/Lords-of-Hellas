using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RegionViewModel : IDisposable
{
    public RegionId Id { get; }
    private List<TokenNest> _nests;

    private List<TokenViewModel> _tokens = new();
    private RegionContext _regionModel;
    private TokenNest _cashedNest;
    private TokenViewModel _cashedViewModel;

    public Action <TokenViewModel> TokenRegistered;

    public RegionViewModel(RegionId id)
    {
        Id = id;
        if (GameContext.Instance.RegionDataRegistry.TryFindRegion(id, out var region)) {
            _regionModel = region;
            _regionModel.TokenPlaced += HandleTokenPlaced;
        }
    }
    public void Dispose()
    {
        if (_regionModel is not null) {
            _regionModel.TokenPlaced -= HandleTokenPlaced;
        }
    }
    private void HandleTokenPlaced(TokenModel token, TokenNest nest)
    {
        var tokenVm = ServiceLocator.Get<TokenFactory>().CreateGhostToken(token);
        Place(tokenVm, nest);
    }
    private void Place(TokenViewModel token, TokenNest nest)
    {
        if (nest is null) {
            nest = GetCenteredUnoccupied();
        } 
        token.Place(nest);
        _tokens.Add(token);
    }
    public void SetNests(List<TokenNest> nests)
    {
        _nests = nests;
    }
    public bool TryGetFreeNest(Vector3 hitPoint, out TokenNest nest)
    {
        nest = GetNearestUnoccupied(hitPoint);
        if (nest != null) {
            return true;
        }
        return false;
    }
    private bool TryFindHopliteStack(PlayerColor playerColor, out HopliteStackViewModel hopliteStack)
    {
        foreach (var token in _tokens) {
            if (token is HopliteStackViewModel stackVm && stackVm.Model is HopliteStackModel stackModel ) {
                if (stackModel.PlayerColor == playerColor) {
                    hopliteStack = stackVm;
                    return true;
                }
            }
        }

        hopliteStack = null;
        return false;
    }
    public TokenNest GetCenteredUnoccupied()
    {
        var average = CalcAveragePoint();
        return GetNearestUnoccupied(average);
    }    
    public TokenNest GetNearestUnoccupied(Vector3 point)
    {
        TokenNest nearest = null;
        float minDistance = float.MaxValue;

        foreach (var spawnPoint in _nests) {
            if (!spawnPoint.IsOccupied) {
                float distance = Vector3.Distance(point, spawnPoint.Position);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearest = spawnPoint;
                }
            }
        }
        if (nearest == null) {
            Debug.LogError("No free SpawnPoints left!");
        }
        return nearest;
    }
    private Vector3 CalcAveragePoint()
    {
        Vector3 averageNest = Vector3.zero;
        foreach (var nest in _nests) {
            averageNest += nest.Position;
        }
        averageNest /= _nests.Count;
        return averageNest;
    }
}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RegionViewModel : IDisposable
{
    public RegionId Id { get; }
    private List<TokenNest> _nests;

    private List<TokenViewModel> _tokens = new();
    private RegionContext _regionModel;
    public Action <TokenViewModel> TokenRegistered;
    public Action <PlayerColor> OwnerChanged;

    public RegionViewModel(RegionId id)
    {
        Id = id;
        if (GameContext.Instance.RegionDataRegistry.TryFindRegion(id, out var region)) {
            _regionModel = region;
            _regionModel.TokenPlaced += HandleTokenPlaced;
            _regionModel.TokenMoved += HandleTokenMoved;

            _regionModel.TokenRemoved += HandleTokenRemoved;
            _regionModel.OwnerChanged += HandleOwnerChanged;
        }
    }
    public void Dispose()
    {
        if (_regionModel is not null) {
            _regionModel.TokenPlaced -= HandleTokenPlaced;
            _regionModel.TokenMoved -= HandleTokenMoved;
            _regionModel.TokenRemoved -= HandleTokenRemoved;
            _regionModel.OwnerChanged -= HandleOwnerChanged;
        }
    }
    private void HandleTokenPlaced(TokenModel token, TokenNest nest)
    {
        var tokenVm = ServiceLocator.Get<TokenFactory>().CreateGhostToken(token);
        Place(tokenVm, nest);
    }
    private void HandleTokenMoved(TokenModel token, RegionId from, TokenNest nest)
    {
        // Debug.Log($"Registering token at {_regionModel.RegionId}");
        // Debug.Log($"Token from {token.RegionId}");
        if (ServiceLocator.Get<RegionsViewModel>().TryGetRegion(from, out var region)) {
            if (region.TryFindToken(token, out var tokenVm)) {
                tokenVm.SetWorldPosition(nest.Position); 
                Place(tokenVm, nest);
            }
        }        
    }
    private void HandleTokenRemoved(TokenModel token)
    {
        foreach (var t in _tokens) {
            if (t.Model == token) {                
                var viewRegistry = ServiceLocator.Get<TokenViewRegistry>();
                if (viewRegistry is null) {
                    Debug.LogWarning("Unable to get TokenViewRegistry!");
                } else {
                    
                    if(!viewRegistry.TryDestroy(t)) {
                        Debug.LogWarning("Unable to destroy ghost TokenView in TokenDragger!");
                    } else {
                        _tokens.Remove(t);
                        return;
                    }
                }
            }
        }
    }
    private void HandleOwnerChanged(PlayerColor color)
    {
        OwnerChanged?.Invoke(color);
    }
    private void Place(TokenViewModel token, TokenNest nest)
    {
        if (nest is null) {
            nest = GetCenteredUnoccupied();
        } 
        token.Place(nest);
        _tokens.Add(token);
    }
    public bool TryFindToken(TokenModel model, out TokenViewModel tokenVm)
    {
        foreach(var t in _tokens) {
            if (t.Model == model) {
                tokenVm = t;
                return true;
            }
        }
        tokenVm = null;
        return false;
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

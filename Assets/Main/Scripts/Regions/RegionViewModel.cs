using System;
using System.Collections.Generic;
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
        if (GameContext.Instance.RegionDataRegistry.TryGetRegion(id, out var region)) {
            _regionModel = region;
            _regionModel.TokenAdded += HandleTokenAdded;
        }
    }
    public void Dispose()
    {
        if (_regionModel is not null) {
            _regionModel.TokenAdded -= HandleTokenAdded;
        }
    }
    public bool TryRegisterToken(TokenViewModel token, TokenNest nest)
    {
        if (nest.RegionId != Id) {
            Debug.LogWarning($"Unable register {token.Model.Type} in region {Id}. RegionId not equal!");
            return false;
        }
        _cashedNest = nest;
        _cashedViewModel = token;
        if (_regionModel.TryRegisterToken(token.Model)) {            
            return true;
        }
        return false;
    }
    private void HandleTokenAdded(TokenModel token)
    {
        Debug.Log("HandelTokenAdded!");
        TokenViewModel viewModel = null;
        if (token is HopliteModel hoplite) {
            foreach(var tokenVm in _tokens) {
                if (tokenVm is HopliteStackViewModel hopliteStackVm) {
                    if (hopliteStackVm.Model is HopliteStackModel hopliteStackModel) {
                        if (hopliteStackModel.PlayerColor == hoplite.PlayerColor) {
                            hopliteStackModel.AddHoplite(hoplite);
                            viewModel = hopliteStackVm;
                            break;
                        }
                    }
                }
            }
        }
        if (viewModel is null) {
            if (_cashedNest is null) {
                Debug.Log("_cashedNest is null!");
            }
            
            if (_cashedViewModel is null) {
                Debug.Log("_cashedViewModel is null!");
            }
            viewModel = _cashedViewModel;
            viewModel.Place(_cashedNest);
            _tokens.Add(viewModel);
        } 
        _cashedViewModel = null;
        _cashedNest = null;
        TokenRegistered?.Invoke(viewModel);
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

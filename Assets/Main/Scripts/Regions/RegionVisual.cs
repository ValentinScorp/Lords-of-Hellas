using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;

public class RegionVisual : MonoBehaviour
{
    private RegionId _id = RegionId.Unknown;
    private RegionModel _region;
    private RegionAreaView _areaView;
    private RegionBorderView _borderView;
    private RegionNestsView _nests;
    private List<IPlaceableVisual> _placeables = new();

    private void Awake()
    {
        _id = RegionIdParser.Parse(gameObject.name);

        if (_id == RegionId.Unknown) {
            Debug.LogWarning($"RegionView '{name}' has unknown RegionId");
        }
        Transform areaTransform = transform.Find("Area");
        if (areaTransform == null) {
            Debug.LogError($"RegionView '{name}' could not find cild Area!");
        } else {
            if (!areaTransform.TryGetComponent(out _areaView)) {
                Debug.LogError($"RegionView '{name}' could not get RegionAreaView component from child Area!");
            }
            if (!areaTransform.TryGetComponent(out _nests)) {
                Debug.LogError($"RegionView '{name}' could not get {_nests.GetType().Name} component from child Area!");                
            }
        }
        Transform borderTransform = transform.Find("Border");
        if (borderTransform == null || !borderTransform.TryGetComponent(out _borderView)) {
            Debug.LogError($"RegionView '{name}' could not find RegionBorderView under child 'Border'");
        }

        if (_areaView is not null) {
            _areaView.Id = _id;
        }
        
    }
    private void Start()
    {
        _region = GameContext.Instance.RegionRegistry.GetRegionContext(_id);
        if (_region is null) {
            Debug.LogWarning($"Can't get region context of : {_id}!");
        } else {
            _region.TemplePlaced += OnTemplePlaced;
            _region.TokenPlaced += OnTokenPlaced;
            _region.TokenRemoved += OnTokenRemoved;

            if (_areaView is not null) {
                _region.OwnerChanged += _areaView.OnOwnerChanged;
            }            
        }
    }

    private void OnDestroy()
    {
        if (_region is not null) {
            _region.TemplePlaced -= OnTemplePlaced;
            _region.TokenPlaced -= OnTokenPlaced;
            _region.TokenRemoved -= OnTokenRemoved;
        }

        if (_region is not null && _areaView is not null) {
            _region.OwnerChanged -= _areaView.OnOwnerChanged;            
        }
    }
    private void OnTemplePlaced(RegionModel region)
    {
        if (region == null || region.RegionId != _id) {
            Debug.LogWarning($"TemplePlaced wrong region for {name}");
            return;
        }
        foreach (var p in _placeables) {
            if (p is TempleVisual) {
                Debug.LogWarning($"Unable to place second temple in {_id}");
                return;
            }
        }
        if (GameContent.PrefabCatalog == null || GameContent.PrefabCatalog.TemplePrefab == null) {
            Debug.LogError("PrefabCatalog or TemplePrefab is not configured.");
            return;
        }
        if (_nests is not null) {
            var templeNest = _nests.GetCenteredUnoccupied();
            if (templeNest == null) {
                Debug.LogWarning($"RegionView '{name}' has no temple nest assigned.");
                return;
            }

            TempleVisual templeView = Instantiate(
                GameContent.PrefabCatalog.TemplePrefab, templeNest.Position, Quaternion.identity, transform);

            templeView.SetOwnerColor(region.OwnedBy);
            _placeables.Add(templeView);
        }        
    }
    private void OnTokenPlaced(TokenModel token, int nestId)
    {
        Debug.Log($"Token placed at {_id} nestId {nestId}");
        RegionNest nest;
        if (nestId == -1)
            nest = _nests.GetCenteredUnoccupied();
        else
            nest = _nests.GetNest(nestId);

        if (nest is null) {
            Debug.LogWarning($"Error getting nest in region {_id}");
            return;
        }
        nest.Occupy();
        var tokenVisual = ServiceLocator.Get<TokenFactory>().CreateTokenPrefab(token);
        tokenVisual.SetParent(transform);
        _placeables.Add(tokenVisual);
        token.SetBoardLocation(_id, nestId);
    }
    private void OnTokenRemoved(TokenModel token)
    {
        if (token == null) {
            Debug.LogWarning("OnTokenRemoved called with null token.");
            return;
        }

        for (int i = _placeables.Count - 1; i >= 0; i--) {
            var placeable = _placeables[i];

            if (!ReferenceEquals(placeable.Model, token)) {
                continue;
            }

            if (placeable is TokenView tokenView && tokenView.Nest != null) {
                tokenView.Nest.Release();
            }

            _placeables.RemoveAt(i);

            if (placeable is MonoBehaviour mb) {
                Destroy(mb.gameObject);
            } else {
                Debug.LogWarning($"Placeable visual for token {token.Type} is not a MonoBehaviour.");
            }

            return;
        }

        Debug.LogWarning($"Token visual not found in region {_id} for removed token {token.Type}.");
    }


}

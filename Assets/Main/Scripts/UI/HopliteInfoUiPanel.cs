using System;
using System.Collections.Generic;
using UnityEngine;

public class HopliteInfoUiPanel : UiPanel
{
    [SerializeField] private HopliteIconView _iconPrefab;
    private const int _MaxIcons = 15;
    private readonly List<HopliteIconView> _icons = new();
    private HopliteManager _manager;
    protected override void Awake()
    {
        base.Awake();
        InstantiateIcons(HopliteManager.MaxHoplites);
    }    
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    internal void Bind(HopliteManager manager) {
        if (manager is not null) {
            _manager = manager;
            _manager.HopliteChangedRegion += OnHopliteChangedRegion;
            _manager.RefreshStatus();
        }
    }    
    internal void Unbind()
    {
        if (_manager is not null) {
            _manager.HopliteChangedRegion -= OnHopliteChangedRegion;
            _manager = null;
        }        
    }
    internal void OnHopliteChangedRegion(HopliteManager manager, TokenModel token)
    {        
        if (manager is null) return;
        SetHoplitesOffBoard(manager.HoplitesOffBoard());
    }
    internal void SetHoplitesOffBoard(int count)
    {
        if (count > _MaxIcons) {
            count = _MaxIcons;
        }
        if (count < 0) {
            count = 0;
        }
        for (int i = 0; i < _MaxIcons; i++) {            
            _icons[i].SetHopliteOffBoard(i < count);
        }
    } 
    private void InstantiateIcons(int count)
    {
        for (int i = 0; i < count; i++) {
            var icon = Instantiate(_iconPrefab, gameObject.transform, false);
            icon.SetHopliteOffBoard(true);
            _icons.Add(icon);
        }
    }
}
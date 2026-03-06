using System;
using System.Collections.Generic;
using UnityEngine;

public class PriestInfoUiPanel : MonoBehaviour
{
    [SerializeField] private PriestIconView _iconPrefab;
    private readonly List<PriestIconView> _icons = new();
    private PriestManager _manager;
    private void Awake()
    {
        InstantiateIcons(PriestManager.MaxPriests);
    }    
    internal void Bind(PriestManager manager) {
        if (manager is not null) {
            _manager = manager;
            _manager.PriestChangedStatus += OnPriestChangedStatus;
            _manager.RefreshStatus();
        }
    }    
    internal void Unbind()
    {
        if (_manager is not null) {
            _manager.PriestChangedStatus -= OnPriestChangedStatus;
            _manager = null;
        }        
    }
    internal void OnPriestChangedStatus(PriestManager manager, PriestModel priest)
    {        
        if (manager == null || manager != _manager) return;
        SetPriestsStatuses(manager.Priests);
    }
    internal void SetPriestsStatuses(List<PriestModel> priests)
    {
        if (priests is null || priests.Count == 0) return;
        if (priests.Count != _icons.Count) return;

        for (int i = 0; i < PriestManager.MaxPriests; i++) {
            _icons[i].SetPlacement(priests[i].PlacedAt);            
        }
    } 
    private void InstantiateIcons(int count)
    {
        for (int i = 0; i < count; i++) {
            var icon = Instantiate(_iconPrefab, gameObject.transform, false);
            icon.SetPlacement(PriestModel.Placement.OffBoard);
            _icons.Add(icon);
        }
    }

    internal void Show(bool activate)
    {
        gameObject.SetActive(activate);
    }
}

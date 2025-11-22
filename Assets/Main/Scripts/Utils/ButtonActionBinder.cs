using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ButtonActionBinder
{
    private List<(Button button, Action action)> _bindings = new List<(Button, Action)>();

    public void Bind(Button button, Action action) {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
        _bindings.Add((button, action));
    }
    public void InteractableButtonByAction(Action action, bool interactable) {
        //Debug.Log($"InteractableButtonByAction called for action {action?.Method.Name}, bindings count: {_bindings.Count}");
        foreach (var binding in _bindings) {
            //Debug.Log($"Checking binding for action {binding.action?.Method.Name}");
            if (binding.action == action) {
                binding.button.interactable = interactable;
                //Debug.Log($"Binder set interactable for action {action?.Method.Name} to {interactable}");
            }
        }
        if (!_bindings.Any(b => b.action == action)) {
            Debug.LogWarning($"No binding found for action {action?.Method.Name}");
        }
    }
    public void InteractableButtonByButtonName(string buttonName, bool interactable) {
        bool found = false;
        foreach (var binding in _bindings) {
            if (binding.button.name == buttonName) {
                binding.button.interactable = interactable;
                found = true;
            }
        }
        if (!found) {
            Debug.LogWarning($"No button found with name {buttonName}");
        }
    }

    public void Unbind(Button button) {
        foreach (var binding in _bindings) {
            if (binding.button == button) {
                button.onClick.RemoveListener(() => binding.action());
            }
        }
        _bindings.RemoveAll(b => b.button == button);
    }

    public void UnbindAll() {
        foreach (var binding in _bindings) {
            binding.button.onClick.RemoveListener(() => binding.action());
        }
        _bindings.Clear();
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UserInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private UserInput _inputActions;
    public event Action<InputAction.CallbackContext> OnMouseClick;

    private void Awake() {
        _inputActions = new UserInput();
    }

    private void OnEnable() {
        _inputActions.Enable();
        _inputActions.Player.Click.performed += OnClick;
    }

    private void OnClick(InputAction.CallbackContext context) {
        if (!IsPointerOverUINoTransparent()) {
            OnMouseClick?.Invoke(context);
        }
    }

    public T GetRaycastTarget<T>() where T : Component {
        if (IsPointerOverUINoTransparent())
            return null;

        Vector2 screenPosition = _inputActions.Player.ScreenPoint.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return hit.collider.GetComponent<T>();
        }

        return null;
    }

    private bool IsPointerOverUIInternalTag(bool ignoreTransparent) {
        if (EventSystem.current == null)
            return false;

        if (EventSystem.current.currentInputModule is InputSystemUIInputModule) {
            var pointerEventData = new PointerEventData(EventSystem.current) {
                position = Mouse.current?.position.ReadValue() ?? Vector2.zero
            };
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            return results.Exists(result =>
                result.gameObject.GetComponent<RectTransform>() != null &&                                                                       
                (!ignoreTransparent || !result.gameObject.CompareTag("RaycastHitTransparent")));
        }

        return EventSystem.current.IsPointerOverGameObject();
    }
    private bool IsPointerOverUIInternal(bool ignoreTransparent) {
        if (EventSystem.current == null)
            return false;

        if (EventSystem.current.currentInputModule is InputSystemUIInputModule) {
            var pointerEventData = new PointerEventData(EventSystem.current) {
                position = Mouse.current?.position.ReadValue() ?? Vector2.zero
            };
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            return results.Exists(result =>
                result.gameObject.GetComponent<RectTransform>() != null &&
                (!ignoreTransparent || result.gameObject.layer != LayerMask.NameToLayer("RaycastHitTransparent")));
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsPointerOverUI() => IsPointerOverUIInternal(false);
    public bool IsPointerOverUINoTransparent() => IsPointerOverUIInternal(true);

    private void OnDisable() {
        _inputActions.Player.Click.performed -= OnClick;
        _inputActions.Disable();
    }
}
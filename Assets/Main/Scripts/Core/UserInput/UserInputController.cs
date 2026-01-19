using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private SelectMgr _clickMgr;
    private UserInput _inputActions;
    public event Action<Vector2> MouseClick;
    public event Action<Vector2> MouseMoved;
    public event Action<Vector2> MouseDelta;

    private void Awake()
    {
        _inputActions = new UserInput();        
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Click.performed += OnClick;
        _inputActions.Player.MousePosition.performed += OnMouseMoved;
        _inputActions.Player.MouseDelta.performed += OnMouseDelta;
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        MouseDelta?.Invoke(pos);
    }

    private void OnMouseMoved(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        MouseMoved?.Invoke(pos);
    }
    public void SetClickMgr(SelectMgr clickMgr)
    {
        _clickMgr = clickMgr;
    }   
    private void OnClick(InputAction.CallbackContext context)
    {
        var pointer = context.control.device as Pointer;
        Vector2 pos = pointer != null
            ? pointer.position.ReadValue()
            : (Vector2?)Touchscreen.current?.primaryTouch.position.ReadValue()
            ?? Vector2.zero;
        _clickMgr?.HandleHits(pos);
        MouseClick?.Invoke(pos);
    }
    public T GetRaycastTarget<T>() where T : Component
    {
        Vector2 screenPosition = _inputActions.Player.MousePosition.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return hit.collider.GetComponent<T>();
        }

        return null;
    }
    private void OnDisable()
    {
        _inputActions.Player.Click.performed -= OnClick;
        _inputActions.Player.MousePosition.performed -= OnMouseMoved;
        _inputActions.Player.MouseDelta.performed -= OnMouseDelta;
        _inputActions.Disable();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private ClickMgr _clickMgr;
    private UserInput _inputActions;
    // public event Action<Vector2> OnMouseClick;

    private void Awake()
    {
        _inputActions = new UserInput();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Click.performed += OnClick;
    }
    public void SetClickMgr(ClickMgr clickMgr)
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
        _clickMgr?.HandleClick(pos);
        // OnMouseClick?.Invoke(pos);
    }
    public T GetRaycastTarget<T>() where T : Component
    {
        Vector2 screenPosition = _inputActions.Player.ScreenPoint.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return hit.collider.GetComponent<T>();
        }

        return null;
    }
    private void OnDisable()
    {
        _inputActions.Player.Click.performed -= OnClick;
        _inputActions.Disable();
    }
}
using UnityEngine.InputSystem;
using UnityEngine;
public class MouseTracker : MonoBehaviour
{
    [SerializeField] private InputAction mouseDelta; // Vector2
    [SerializeField] private RaycastIntersector _raycast; // т

    private void OnEnable()
    {
        mouseDelta.Enable();
        mouseDelta.performed += OnMouseMoved;
        mouseDelta.canceled += OnMouseStopped;
    }

    private void OnDisable()
    {
        mouseDelta.performed -= OnMouseMoved;
        mouseDelta.canceled -= OnMouseStopped;
        mouseDelta.Disable();
    }

    private void OnMouseMoved(InputAction.CallbackContext ctx)
    {
        Vector2 delta = ctx.ReadValue<Vector2>();
    }

    private void OnMouseStopped(InputAction.CallbackContext ctx)
    {

    }    
}

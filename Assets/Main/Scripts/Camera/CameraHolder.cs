using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraHolder : MonoBehaviour
{
    const float panSpeed = 0.003f;
    const float zoomSpeed = 0.4f;

    public float panSpeedChange = 0.003f;
    public float minZoom = 3f;
    public float maxZoom = 18f;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    private PointerEventData _pointerEventData;

    public Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //Debug.Log("Start: ");
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnPan(InputAction.CallbackContext context) {
        //Debug.Log(transform.position.)
        Vector2 direction = context.ReadValue<Vector2>() * panSpeedChange;
        mainCamera.transform.position = mainCamera.transform.position + new Vector3(-direction.x, 0, -direction.y);
    }

    public void OnZoom(InputAction.CallbackContext context) {
        var zoom = context.ReadValue<Vector2>();
        if (!IsPointerOverUI()) {
            float newZoom = mainCamera.orthographicSize - Mathf.Sign(zoom.y) * zoomSpeed;
            if (newZoom > minZoom && newZoom < maxZoom && (zoom.y != 0)) {
                mainCamera.orthographicSize = newZoom;
                panSpeedChange = (minZoom + maxZoom * (newZoom - minZoom) / (maxZoom - minZoom)) / 1000f;
            }
        }
    }
    public bool IsPointerOverUI() {
        _pointerEventData = new PointerEventData(eventSystem);
        _pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(_pointerEventData, results);
        return results.Count > 0;
    }
}

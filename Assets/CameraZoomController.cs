using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private InputActionAsset playerInput;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoomRangeFOV;
    [SerializeField] private float maxZoomRangeFOV;

    private InputAction zoomInputAction
    {
        get
        {
            return playerInput.FindActionMap("Player").FindAction("Zoom"); ;
        }
        set
        {
            zoomInputAction = value;
        }
    }

    private void Update()
    {
        ApplyZoomMotion();
    }

    private void ApplyZoomMotion()
    {
        cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Clamp(cinemachineFreeLook.m_Lens.FieldOfView - zoomInputAction.ReadValue<float>() * zoomSpeed, minZoomRangeFOV, maxZoomRangeFOV);
    }
}

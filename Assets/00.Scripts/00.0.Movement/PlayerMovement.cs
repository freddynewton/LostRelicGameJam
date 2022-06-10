using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float roationSpeed;

    [Header("Assigns")]
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private Rigidbody rb;

    private InputAction moveInputAction;
    private float turnRotationVelocity;
    private Camera mainCamera;

    private Vector3 inputMovementVector
    {
        get
        {
            Vector2 movementInput = moveInputAction.ReadValue<Vector2>();
            return new Vector3(movementInput.x, 0, movementInput.y);
        }
        set
        {
            inputMovementVector = value;
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        ApplyInputMovementVector();
    }

    private void ApplyInputMovementVector()
    {
        if (inputMovementVector.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(inputMovementVector.x, inputMovementVector.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnRotationVelocity, roationSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection.y = 0;
            rb.velocity = moveDirection.normalized * Time.deltaTime * movementSpeed;
        }
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        moveInputAction = playerInput.FindActionMap("Player").FindAction("Move");
    }
}

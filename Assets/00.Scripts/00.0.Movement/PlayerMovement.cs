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

    private Vector3 inputMovementVector
    {
        get
        {
            Vector2 movementInput = playerInput.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>();
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
        rb.velocity = inputMovementVector * Time.deltaTime * movementSpeed;
    }
}

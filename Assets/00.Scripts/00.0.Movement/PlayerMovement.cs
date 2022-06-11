using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float roationSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float groundedOffset = -0.1f;

    [Header("Assigns")]
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RopeController ropeController;
    [SerializeField] private Collider collider;

    private InputAction moveInputAction;
    private float turnRotationVelocity;
    private Camera mainCamera;

    public bool canMove { get; private set; } = true;

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


    private void FixedUpdate()
    {
        if (canMove)
        {
            ApplyInputMovementVector();
        }
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + groundedOffset);
    }

    private void ApplyInputMovementVector()
    {
        if (inputMovementVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputMovementVector.x, inputMovementVector.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnRotationVelocity, roationSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection.y = 0;
            rb.MovePosition(transform.position + moveDirection.normalized * Time.deltaTime * movementSpeed);
        }
    }

    private async void OnPulledBack()
    {
        canMove = false;
        await Wait(ropeController.ropePullBackMovementDisableTimeAmountMS);
        canMove = true;
    }

    private async Task Wait(int time)
    {
        await Task.Delay(time);
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        moveInputAction = playerInput.FindActionMap("Player").FindAction("Move");
        playerInput.FindActionMap("Player").FindAction("Jump").started += PlayerMovementJump_started;
    }

    private void PlayerMovementJump_started(InputAction.CallbackContext obj)
    {
        Debug.Log(CheckIfGrounded());

        if (canMove && CheckIfGrounded())
        {
            rb.AddForce(transform.up.normalized * Time.deltaTime * jumpStrength, ForceMode.Impulse);
        }
    }

    private void OnEnable()
    {
        ropeController.onPullBackEvent += OnPulledBack;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        ropeController.onPullBackEvent -= OnPulledBack;
        playerInput.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * groundedOffset);
    }
}

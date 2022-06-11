using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeController : MonoBehaviour
{
    [Header("Settings")]
    public int ropePullBackMovementDisableTimeAmountMS;
    [SerializeField] private float ropeRange;

    [Header("Stettings - pullback")]
    [SerializeField] private float ropeRangePullBackOffset;
    [SerializeField] private float pullBackForceStrength;
    [SerializeField] private Vector3 pullBackOffset;

    [Header("Settings - pull")]
    [SerializeField] private float pullStrength;
    [SerializeField] private float ropePullStrenghtStaminaCost = 1f;
    public float ropePullStrengthStaminaMax = 100f;
    [HideInInspector] public float ropePullStrengthStaminaCurrent = 100f;

    [Header("Assigns")]
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private GameObject ropeTarget;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private UnitController lizardController;
    [SerializeField] private PlayerMovement playerMovement;

    public event Action onPullBackEvent;

    private InputAction inputActionFire;

    private void Awake()
    {
        ropePullStrengthStaminaCurrent = ropePullStrengthStaminaMax;
        inputActionFire = playerInput.FindActionMap("Player").FindAction("Fire");


        if (ropeTarget == null)
        {
            ropeTarget = GameObject.FindGameObjectWithTag("Lizard");
        }
    }

    private void LateUpdate()
    {
        if (CheckPullBackRange() || ropePullStrengthStaminaCurrent <= 0 && playerMovement.canMove)
        {
            onPullBackEvent?.Invoke();
            ApplyPullBackForce();
        }

        if (inputActionFire.IsPressed() && ropePullStrengthStaminaCurrent > 0)
        {
            ropePullStrengthStaminaCurrent = Mathf.Clamp(ropePullStrengthStaminaCurrent - Time.deltaTime * ropePullStrenghtStaminaCost, 0, ropePullStrengthStaminaMax);
            lizardController.ApplyPullForce(transform, pullStrength);
        } else
        {
            ropePullStrengthStaminaCurrent = Mathf.Clamp(ropePullStrengthStaminaCurrent + Time.deltaTime * ropePullStrenghtStaminaCost, 0, ropePullStrengthStaminaMax);
        }
    }

    private void ApplyPullBackForce()
    {
        playerRigidbody.velocity = Vector3.zero;
        Vector3 direction = (ropeTarget.transform.position + pullBackOffset) - playerRigidbody.position;
        playerRigidbody.AddForce(direction * pullBackForceStrength * Time.deltaTime, ForceMode.Impulse);
    }

    private bool CheckPullBackRange()
    {
        return Vector3.Distance(ropeTarget.transform.position, playerRigidbody.position) > ropeRange + ropeRangePullBackOffset;
    }

    private void OnDrawGizmos()
    {
        if (ropeTarget == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ropeTarget.transform.position, ropeRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ropeTarget.transform.position, ropeRange + ropeRangePullBackOffset);
    }
}

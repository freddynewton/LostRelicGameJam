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

    [Header("Assigns")]
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private GameObject ropeTarget;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private UnitController lizardController;

    public event Action onPullBackEvent;

    private InputAction inputActionFire;

    private void Awake()
    {
        inputActionFire = playerInput.FindActionMap("Player").FindAction("Fire");


        if (ropeTarget == null)
        {
            ropeTarget = GameObject.FindGameObjectWithTag("Lizard");
        }
    }

    private void LateUpdate()
    {
        if (CheckPullBackRange())
        {
            onPullBackEvent?.Invoke();
            ApplyPullBackForce();
        }

        if (inputActionFire.IsPressed())
        {
            lizardController.ApplyPullForce(transform, pullStrength);
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

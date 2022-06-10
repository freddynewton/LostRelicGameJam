using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [Header("Settings")]
    public int ropePullBackMovementDisableTimeAmountMS;
    [SerializeField] private float ropeRange;
    [SerializeField] private float ropeRangePullBackOffset;

    [SerializeField] private float pullBackForceStrength;
    [SerializeField] private Vector3 pullBackOffset;

    [Header("Assigns")]
    [SerializeField] private GameObject ropeTarget;
    [SerializeField] private Rigidbody playerRigidbody;

    public event Action onPullBackEvent;

    private void Awake()
    {
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

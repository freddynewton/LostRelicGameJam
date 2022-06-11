using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public void ApplyPullForce(Transform player, float pullForceStregth)
    {
        rb.MovePosition(transform.position + (player.transform.position - transform.position).normalized * pullForceStregth);
        //rb.AddForce((player.transform.position - transform.position).normalized * pullForceStregth, ForceMode.Force);
    }
}

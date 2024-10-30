using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCapsule : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] private Rigidbody rigidBody;
    [SerializeField, Self] private CapsuleCollider capsuleCollider;

    [Header("Settings")]
    [SerializeField] private float desiredFloatHeight = 1.25f;
    [SerializeField] private float rayLength = 1.5f;
    [SerializeField] private float springStrength = 50f;
    [SerializeField] private float springDamp = 5f;

    [field: SerializeField] public bool IsGrounded { get; private set; }

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Update()
    {
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        HandleFloatingCapsule();
    }

    private void HandleFloatingCapsule()
    {
        Vector3 rayCastDir = -transform.up;
        RaycastHit groundHit;
        Physics.Raycast(capsuleCollider.bounds.center, rayCastDir, out groundHit, rayLength, LayerMask.GetMask("Ground"));
        /*Debug.DrawLine(capsuleCollider.bounds.center, capsuleCollider.bounds.center + rayLength * rayCastDir, Color.white);
        Debug.DrawLine(capsuleCollider.bounds.center + Vector3.right, capsuleCollider.bounds.center + Vector3.right + desiredFloatHeight * rayCastDir, Color.green);
*/
        if(groundHit.collider == null)
        {
            return;
        }
        Debug.DrawLine(capsuleCollider.bounds.center, groundHit.point, Color.red);

        float floatHeightGroundDifference =  groundHit.distance - desiredFloatHeight;

        float rayCastDirVel = Vector3.Dot(rayCastDir, rigidBody.velocity);

        float springForce = floatHeightGroundDifference * springStrength -  springDamp * rayCastDirVel;

        //Debug.Log($"Diff: {floatHeightGroundDifference}, Force: {springForce}, Dir: {rayCastDir * springForce}");
        rigidBody.AddForce(rayCastDir * springForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(capsuleCollider.bounds.center - desiredFloatHeight * transform.up + (0.9f) * capsuleCollider.radius * transform.up, capsuleCollider.radius);
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics.CheckSphere(capsuleCollider.bounds.center - desiredFloatHeight * transform.up + (0.9f) * capsuleCollider.radius * transform.up, capsuleCollider.radius, LayerMask.GetMask("Ground"));
    }
}

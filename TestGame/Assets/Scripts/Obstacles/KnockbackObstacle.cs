using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackObstacle : MonoBehaviour
{
    [SerializeField]
    private float knockbackForceMagnitude = 50f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if there is at least one contact point
        if (collision.contacts.Length > 0)
        {
            // Get the first contact point
            ContactPoint contactPoint = collision.contacts[0];

            // Apply force at the contact point in the direction of the contact normal
            ApplyForceAtContactPoint(contactPoint, collision.transform.GetComponent<Rigidbody>());
        }
    }

    void ApplyForceAtContactPoint(ContactPoint contactPoint, Rigidbody body)
    {
        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.AddForceAtPosition(-knockbackForceMagnitude*contactPoint.normal,contactPoint.point, ForceMode.Impulse);
        }
    }
}

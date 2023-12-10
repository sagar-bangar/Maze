using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPole : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float forceMagnitude = 10f;

    void Update()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime,Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        // Check if there is at least one contact point
        if (collision.contacts.Length > 0)
        {
            // Get the first contact point
            ContactPoint contactPoint = collision.contacts[0];

            // Apply force at the contact point in the direction of the contact normal
            ApplyForceAtContactPoint(contactPoint,collision.transform.GetComponent<Rigidbody>());
        }
    }

    void ApplyForceAtContactPoint(ContactPoint contactPoint,Rigidbody body)
    {
        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.AddExplosionForce(forceMagnitude, contactPoint.point, 2f);
        }
    }
}

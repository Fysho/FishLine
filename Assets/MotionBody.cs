using System;
using UnityEngine;

public class MotionBody : MonoBehaviour
{
    // Air Base drag
    public static float BaseAirDrag = 2;
    [Min(1), Tooltip("Apply any additional drag to the body.")]
    public float additionalDrag = 1;
    private Vector2 velocity;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Apply base air drag
        velocity /= BaseAirDrag;
        // Apply any additional drag afterwards.
        velocity /= additionalDrag;
        
        ApplyVelocity();
    }

    // Set the velocities of every IBodyController.
    private void ApplyVelocity()
    {
        IBodyController[] controllers = GetComponents<IBodyController>();

        foreach (var controller in controllers)
        {
            controller.ExternalVelocity = velocity;
        }
    }

    public void AddMotion(Vector2 motion)
    {
        // A guard in case GameObject does not have a RigidBody2D, it should have one though.
        // Or in the case that the object has no mass.
        if (rb && rb.mass > 0)
        {
            // Mock Impulse Force by adding instant velocity similar to the formula Acceleration = Force / Mass.
            velocity += motion / rb.mass;
        }
        else
        {
            // Assume body is weightless.
            velocity += motion;
        }
       
    }
}
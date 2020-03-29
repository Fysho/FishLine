using System;
using UnityEngine;

public class GrappleHookController : MonoBehaviour
{
    [HideInInspector]
    public float grappleSwingSpeed;
    [HideInInspector]
    public float grappleSpeed;

    private Vector2 initialDirection;

    private Collider2D collider;
    private Rigidbody2D rb;
    private float currentPoint;
    private bool stopped;

    // Called when hook is being setup.
    public void SetupGrapple(float grappleSpeed, float grappleSwingSpeed, Vector2 initialDirection)
    {
        this.grappleSpeed = grappleSpeed;
        this.grappleSwingSpeed = grappleSwingSpeed;
        this.initialDirection = initialDirection;
        
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        Debug.Log($"Spawning grapple hook with speed {grappleSpeed}");
    }
    
    // Called when the hook is currently travelling.
    public void OnHookTravelling()
    {
        // float angle = transform.rotation.eulerAngles.z;
        // Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        // Debug.Log($"Angled at {angle} at direction {direction}");
        // rb.AddForce(initialDirection * grappleSpeed);
        
        if (!stopped)
        {
            Vector2 addedVelocity = initialDirection * (grappleSpeed / 10f);
            rb.MovePosition(transform.position + (Vector3) addedVelocity);
            TryLandHook();
        }
    }

    // Called when hook is suppose to land, i.e when player releases the hook button.
    public void StopHook()
    {
        stopped = true;
        
        // Check if the hook can land on anything.
        if (!TryLandHook())
        {
            // make the hook fall down or disappear or something.
        }
    }

    private bool TryLandHook()
    {
        // Make an exception for player and non-hookables.
        int mask = ~((1 << 9) | (1 << 11));

        Debug.Log(mask);

        // Try to see if we can land the hook onto anything!
        RaycastHit2D hit = Physics2D.Raycast(transform.position, initialDirection, 0.5f, mask);

        if (hit.collider != null)
        {
            stopped = true;
            Debug.Log($"Landing grapple hook to {hit.transform.name}");
            
            // We have successfully landed on a surface.
            
            return true;
        }

        return false;
    }
}
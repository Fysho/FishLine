using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    bool wInput = false;
    bool aInput = false;
    bool sInput = false;
    bool dInput = false;
    bool shiftInput = false;

    public float speed = 1;
    public float jumpStrength = 1;
    public CaveGenerator caveGenerator;
    [Tooltip("Amount of double jumps")]
    public int doubleJumpsAmount;

    [Header("Physics"), Tooltip("The collider that determines if the player is touching the ground")] 
    public Collider2D groundCollider;

    private int doubleJumps;
    private bool isGrounded;
    private float jumpCooldown;
    private Rigidbody2D rigidBody;
    private float groundDetectPoint;
    private bool canDoubleJump = true;
    private bool isJumpPressed;
    private bool jumpLock;
    private Collider2D collider;
        
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        StartCoroutine(ExplodeAfterSeconds());
    }

    
    // TODO think about this temporary code.
    private IEnumerator ExplodeAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        caveGenerator.Explode(transform.position.x, transform.position.y, 12);
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGround();
        // Debug.Log($"Grounded is {isGrounded}");
        
        float horizontalVelocity = Input.GetAxis("Horizontal") * speed;
        HandleJumpInput();

        // Horizontal Movement
        rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
    }

    private void CheckGround()
    {
        isGrounded = false;

        List<Collider2D> overlaps = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(~(1 << 9));

        if (groundCollider.OverlapCollider(filter, overlaps) > 0)
        {
            isGrounded = true;
        }
    }

    private void HandleJumpInput()
    {
        float jumpInput = Mathf.Max(0, Input.GetAxis("Jump"));

        // Check if player should jump.
        if (jumpInput > 0 && !jumpLock && (isGrounded || doubleJumps > 0))
        {
            isJumpPressed = true;
            jumpLock = true;
            // Subtract doubleJumps if player is double jumping.
            doubleJumps = isGrounded ? doubleJumps : Mathf.Max(0, doubleJumps - 1);
            // Debug.Log($"{(!isGrounded ? "Double Jumping" : "Jumping")} doubleJumps {doubleJumps} input {jumpInput}");
        }
        
        // Remove lock once jump is no longer being pressed.
        if (jumpInput == 0 && jumpLock)
        {
            // Debug.Log("Unlock jump");
            jumpLock = false;
        }

        // Process constant jump input and apply it to force.
        if (isJumpPressed)
        {
            rigidBody.AddForce(new Vector2(0, jumpStrength * jumpInput), ForceMode2D.Impulse);
        }
        
        // If the Jump button is on longer pressed or has been pressed all the way, we are no longer taking any jump input.
        if (jumpInput == 1 || jumpInput == 0)
        {
            isJumpPressed = false;
        }
        
        // Reset doubleJumps when touching the ground.
        doubleJumps = isGrounded ? doubleJumpsAmount : doubleJumps;
    }
}

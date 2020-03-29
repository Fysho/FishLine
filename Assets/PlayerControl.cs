using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerControl : MonoBehaviour, IBodyController
{
    bool wInput = false;
    bool aInput = false;
    bool sInput = false;
    bool dInput = false;
    bool shiftInput = false;

    public float speed = 1;
    public CaveGenerator caveGenerator;

    [Header("Jumping")]
    public float jumpStrength = 1;
    [Tooltip("Amount of double jumps")]
    public int doubleJumpsAmount;
    public AnimationCurve jumpInputCurve;
    public float coyoteTimeAmount;

    [Header("Physics"), Tooltip("The collider that determines if the player is touching the ground")]
    public Collider2D groundCollider;
    
    [Header("Extra")]
    public GameObject jumpPuff;
    [Tooltip("Offset the puff spawn location")]
    public Vector2 jumpPuffOffset;

    [Header("Grapple Hook")] 
    public GameObject hook;
    public float maxGrappleInAir = 1;
    [Min(1)]
    public float grappleHookSpeed = 1;
    public float grappleSwingSpeed;
    public float grappleSpawnDistance = 0.6f;

    private int doubleJumps;
    private bool isGrounded;
    private float jumpCooldown;
    private Rigidbody2D rigidBody;
    private float groundDetectPoint;
    private bool canDoubleJump = true;
    private bool isJumpPressed;
    private bool jumpLock;
    private bool isJumping;
    private Collider2D collider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool groundJumpLock;
    private float coyoteTime;
    private float grappleCount;
    private bool grappleLock;
    private bool isGrappling;
    private GrappleHookController currentHook;

    // Required ExternalVelocity from IBodyController
    public Vector2 ExternalVelocity { get; set; } = Vector2.zero;
    private bool CanNormalJump => coyoteTime > 0 && !groundJumpLock && !isJumping;
    private bool CanDoubleJump => doubleJumps > 0;


    public AudioClip jumpNoise;
    public AudioClip throwNoise;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine(ExplodeAfterSeconds());
    }


    // TODO think about this temporary code.
    private IEnumerator ExplodeAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        caveGenerator.Explode(transform.position.x, transform.position.y, 12);
    }


    private void Update()
    {
        CheckGround();
        // Debug.Log($"Grounded is {isGrounded}");

        ApplyCoyoteTime();

        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalVelocity = horizontalInput * speed;
        
        HandleJumpInput();
        HandleGrapple();

        // Animations
        animator.SetBool("IsAirborne", !isGrounded);
        animator.SetBool("Walking", Mathf.Abs(horizontalInput) > 0.1f);
        animator.SetBool("UpVelocity", isJumping);

        if (horizontalInput < -0.2f)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0.2f)
        {
            spriteRenderer.flipX = false;
        }

        // Horizontal Movement and external velocity
        rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y) + ExternalVelocity;
    }

    private void ApplyCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTime = coyoteTimeAmount;
        }
        else
        {
            coyoteTime = Mathf.Max(0, coyoteTime - Time.deltaTime);
        }
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

    private void HandleGrapple()
    {
        bool isHoldingGrappleInput = Input.GetAxisRaw("Fire2") > 0;
        
        if (isGrounded)
        {
            grappleCount = maxGrappleInAir;
        }
        
        if (isHoldingGrappleInput && !isGrappling && grappleCount > 0)
        {
            // grappleLock = true;
            isGrappling = true;
            grappleCount--;
            
            // Shoot grapple here.
            // Set grappleSpeed and grappleHookSpeed so we can adjust the hook speed and stu

            Vector2 aimDirection = GetAimDirection();

            Quaternion hookRotation = Quaternion.LookRotation(Vector3.forward, aimDirection);
            Vector3 hookPosition = transform.position + (Vector3) (aimDirection * grappleSpawnDistance);
            GameObject hookObject = Instantiate(hook, hookPosition, hookRotation);
            currentHook = hookObject.GetComponent<GrappleHookController>();
            currentHook.SetupGrapple(grappleHookSpeed, grappleSwingSpeed, aimDirection);
        }

        // Currently holding grapple button.
        if (isGrappling)
        {
            Debug.DrawLine(transform.position, currentHook.transform.position, Color.blue);
            currentHook.OnHookTravelling();
        }

        // Released grapple button.
        if (!isHoldingGrappleInput && isGrappling)
        {
            currentHook.StopHook();
            currentHook = null;
            isGrappling = false;
            // grappleLock = false;
        }
    }

    private void HandleJumpInput()
    {
        float jumpInput = Mathf.Max(0, Input.GetAxis("Jump"));

        // Check if player should jump.
        if (jumpInput > 0 && !jumpLock && (CanNormalJump || CanDoubleJump))
        {
            // Check if double jumping.
            if (!CanNormalJump)
            {
                // Negate vertical velocity to get that double jump feeling.
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

                doubleJumps = Mathf.Max(0, doubleJumps - 1);
                CreateJumpPuff();
            }
            isJumping = true;
            groundJumpLock = true;
            isJumpPressed = true;
            jumpLock = true;
        }

        // Remove lock once jump is no longer being pressed.
        if (jumpInput == 0 && jumpLock)
        {
            jumpLock = false;
        }

        // Process constant jump input and apply it to force.
        if (isJumpPressed && jumpInput > 0)
        {
            AudioSource.PlayClipAtPoint(jumpNoise, transform.position);

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength * jumpInputCurve.Evaluate(jumpInput));
            // rigidBody.AddForce(new Vector2(0, jumpStrength * jumpInputCurve.Evaluate(jumpInput)), ForceMode2D.Impulse);
        }

        // If the Jump button is on longer pressed or has been pressed all the way, we are no longer taking any jump input.
        if (jumpInput == 1 || jumpInput == 0)
        {
            isJumpPressed = false;
        }

        // Release ground lock when no longer touching the ground.
        if (!isGrounded)
        {
            groundJumpLock = false;
        }

        // When touching the ground again after any jump, reset double jump and player is no longer jumping.
        if (isGrounded && !groundJumpLock)
        {
            isJumping = false;
            doubleJumps = doubleJumpsAmount;
        }
    }

    private void CreateJumpPuff()
    {
        if (jumpPuff)
        {
            Vector3 puffLocation = transform.position;
            puffLocation.y -= collider.bounds.size.y / 2f;
            puffLocation += (Vector3) jumpPuffOffset;
            Instantiate(jumpPuff, puffLocation, Quaternion.identity);
        }
    }
    
    private Vector2 GetAimDirection()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}

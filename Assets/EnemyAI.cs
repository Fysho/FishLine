using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


enum States
{
    IDLE = 0,
    WALK = 1,
    CHASE = 2
}
  


public class EnemyAI : MonoBehaviour, IBodyController
{

    int state = 0;
    bool left = false;
    bool jumped = false;
    float stateTime;
    float jumpTime;
    float damageTime;

    float idleTime = 2;
    float walkTime = 1;

    System.Random rng;
    Rigidbody2D rb;
    static int count = 0;
    GameObject player;
    public Collider2D groundCollider;
    public AudioClip die;
    public AudioClip jump;
    // Required ExternalVelocity from IBodyController
    public Vector2 ExternalVelocity { get; set; } = Vector2.zero;
    
    private void Start()
    {
        player = GameObject.Find("Player");

        count++;
        rng = new System.Random(count);
        stateTime = (int) States.IDLE;
        rb = gameObject.GetComponent<Rigidbody2D>();
        idleTime =(float) rng.NextDouble() * 3;
    }

    public void PlayDeathNoise()
    {
        AudioSource.PlayClipAtPoint(die, transform.position);

    }

    private void Update()
    {
        stateTime += Time.deltaTime;
        if (Time.frameCount % 60 == 0)
        {
            PlayerCheck();  //check slow
        }
        if (state == (int)States.IDLE)
        {
            if(stateTime > idleTime)
            {
                StartWalking();
            }
        }
        if(state == (int)States.WALK)
        {
            float xSpeed = left ? -3 : 3;
            if(rb.velocity.magnitude < 0.1 && stateTime > 0.1f && jumped == false)
            {
                jumped = true;
                rb.AddForce(new Vector3(0, 10, 0), ForceMode2D.Impulse);
            }
            Vector3 newVel = new Vector3(xSpeed, rb.velocity.y, 0);
            rb.velocity = newVel;

            if (stateTime > walkTime)
            {
                StartIdling();
            }
        }
        if (state == (int)States.CHASE)
        {
            left = player.transform.position.x < transform.position.x;
            float xSpeed = left ? -4 : 4;
            jumpTime -= Time.deltaTime;
            damageTime -= Time.deltaTime;

            //if(damageTime < 0)
            //{
            //    player.transform.position - transform.position).magniude    
            //}
            if (player.transform.position.y - transform.position.y > 3 && jumpTime < 0)
            {
                List<Collider2D> overlaps = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(~(1 << 9));
                if (groundCollider.OverlapCollider(filter, overlaps) > 0)
                {
                    rb.AddForce(new Vector3(0, 10, 0), ForceMode2D.Impulse);
                    jumpTime = 2;
                }

            }
            Vector3 newVel = new Vector3(xSpeed, rb.velocity.y, 0);
            // Set velocity with added external velocity
            rb.velocity = newVel + (Vector3) ExternalVelocity;

            if (stateTime > walkTime)
            {
                StartIdling();
            }
        }
    }

    void PlayerCheck()
    {
        if(state != (int)States.CHASE)
        {
            if ((transform.position - player.transform.position).magnitude < 10)
            {
                StartChasing();
            }
        }
        if(state == (int)States.CHASE)
        {
            if ((transform.position - player.transform.position).magnitude > 15)
            {
                StartIdling();
            }
        }
       
    }

    void StartChasing()
    {
        jumpTime = 0;
        damageTime = 0;

        stateTime = 0;
        state = (int)States.CHASE;
    }

    void StartWalking()
    {
        stateTime = 0;
        state = (int)States.WALK;
        left = rng.NextDouble() < 0.5;
        jumped = false;
        walkTime = (float)rng.NextDouble() * 3;

    }

    void StartIdling()
    {
        stateTime = 0;
        state = (int)States.IDLE;
        idleTime = (float)rng.NextDouble() * 3;


    }

    //Tilemap tilemap;
    //float updateStart;
    //System.Random random;
    //float updateTime;
    //bool idleing = true;
    //float targetX;
    //float targetY;
    //float distanceToJump;
    //int dir = 0; //1left //2right
    //Rigidbody2D rigidBody;
    //float startX;
    //bool jumped;
    //float walkdistance;
    //void Start()
    //{
    //    tilemap = GameObject.Find("Collidable").GetComponent<Tilemap>();
    //    updateStart = Time.time;
    //    random = new System.Random();
    //    updateTime = 1;// (float) random.NextDouble() * 3 + 3;
    //    idleing = true;
    //    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (idleing)
    //    {
    //        if (Time.time - updateStart > updateTime)
    //        {
    //            startWalk();
    //        }
    //    }
    //    if (!idleing)
    //    {
    //        Vector3 setVel = new Vector3(0, 0, 0);
    //        float dt = Time.deltaTime;
    //        if (dir == 1) setVel = setVel + new Vector3(-500 * dt, 0, 0);
    //        if (dir == 2) setVel = setVel + new Vector3(500 * dt, 0, 0);
    //        setVel.y = rigidBody.velocity.y;
    //        rigidBody.velocity = setVel;
    //        float f = Mathf.Abs(transform.position.x - startX);
    //        if (Mathf.Abs(transform.position.x - startX) >= walkdistance)
    //        {
    //            idleing = true;
    //            updateStart = Time.time;
    //            updateTime = 1;// (float)random.NextDouble() * 3 + 3;
    //            Vector3 setVel2 = new Vector3(0, 0, 0);

    //            setVel2.y = rigidBody.velocity.y;
    //            rigidBody.velocity = setVel2;

    //        }
    //    }
    //}

    //void startWalk()
    //{
    //    idleing = false;
    //    targetX = 0;
    //    targetY = 0;
    //    distanceToJump = 0;
    //    startX = transform.position.x;
    //    jumped = false;
    //    walkdistance = 1;
    //    dir = (int) (random.NextDouble() * 2) + 1;
    //    Debug.Log(dir);

    //}
}

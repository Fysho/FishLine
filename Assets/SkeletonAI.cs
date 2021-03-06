﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SkeletonAI : MonoBehaviour
{

    int state = 0;
    bool left = false;
    bool jumped = false;
    float stateTime;
    float jumpTime;
    float damageTime;
    float shootTime;
    float shootSeconds = 1;
    float idleTime = 2;
    float walkTime = 1;

    System.Random rng;
    Rigidbody2D rb;
    static int count = 0;
    GameObject player;
    public Collider2D groundCollider;
    public GameObject arrow;

    public AudioClip die;
    public AudioClip jump;
    public AudioClip shoot;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        count++;
        rng = new System.Random(count);
        stateTime = (int)States.IDLE;
        rb = gameObject.GetComponent<Rigidbody2D>();
        idleTime = (float)rng.NextDouble() * 10;
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
            if (stateTime > idleTime)
            {
                StartWalking();
            }
        }
        if (state == (int)States.WALK)
        {
            float xSpeed = left ? -1.5f : 1.5f;
            if (rb.velocity.magnitude < 0.1 && stateTime > 0.1f && jumped == false)
            {
                jumped = true;
                rb.AddForce(new Vector3(0, 7, 0), ForceMode2D.Impulse);

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
            float xSpeed = left ? -3 : 3;
            jumpTime -= Time.deltaTime;
            damageTime -= Time.deltaTime;
            shootTime -= Time.deltaTime;
            if(shootTime < 0)
            {
                AudioSource.PlayClipAtPoint(shoot, transform.position);

                Vector3 direction = (player.transform.position - transform.position).normalized;
                //float mag = (player.transform.position - transform.position).magnitude;
                GameObject a = Instantiate(arrow, new Vector3(transform.position.x -0.5f, transform.position.y + 0.5f, 0), Quaternion.identity);
                a.GetComponent<ArrowAI>().Init(direction * 20.0f );
                shootTime = shootSeconds;
            }
            if (player.transform.position.y - transform.position.y > 3 && jumpTime < 0)
            {
                List<Collider2D> overlaps = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(~(1 << 9));
                if (groundCollider.OverlapCollider(filter, overlaps) > 0)
                {
                    rb.AddForce(new Vector3(0, 7, 0), ForceMode2D.Impulse);
                    jumpTime = 2;
                }

            }
            Vector3 newVel = new Vector3(xSpeed, rb.velocity.y, 0);
            rb.velocity = newVel;

            if (stateTime > walkTime)
            {
                StartIdling();
            }
        }
    }

    void PlayerCheck()
    {
        if (state != (int)States.CHASE)
        {
            if ((transform.position - player.transform.position).magnitude < 10)
            {
                StartChasing();
            }
        }
        if (state == (int)States.CHASE)
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
        shootTime = shootSeconds;
        stateTime = 0;
        state = (int)States.CHASE;
    }

    void StartWalking()
    {
        stateTime = 0;
        state = (int)States.WALK;
        left = rng.NextDouble() < 0.5;
        jumped = false;
        walkTime = (float)rng.NextDouble() * 2;

    }

    void StartIdling()
    {
        stateTime = 0;
        state = (int)States.IDLE;
        idleTime = (float)rng.NextDouble() * 10;


    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAI : MonoBehaviour
{
    public float arrowDamage = 6;
    public GameObject sprite;

    Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 vel)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = vel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.name == "Player")  //layers not working
        // {
            // GameObject.Find("Player").GetComponent<PlayerControl>().Damage(10);
            
        // }
        
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            
        if (playerHealth)
        {
            playerHealth.TakeDamage(arrowDamage);
            // Probably need to destroy the arrow here.
        }
    }


    // Update is called once per frame
    void Update()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        sprite.transform.localRotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
}

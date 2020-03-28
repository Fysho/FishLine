using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAI : MonoBehaviour
{

    public GameObject sprite;

    Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Init(Vector3 vel)
    {
        rb.velocity = vel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")  //layers not working
        {
            GameObject.Find("Player").GetComponent<PlayerControl>().Damage(10);

        }
    }


    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        sprite.transform.rotation.SetEulerAngles(0, 0, angle);
    }
}

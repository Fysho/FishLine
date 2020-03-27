using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{

    public GameObject player;
    Rigidbody2D rb;
    bool fallen;
    float startTime;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        fallen = false;
    }

    void Update()
    {
        if (!fallen)
        {
            float distance = (gameObject.transform.position - player.transform.position).magnitude;
            if(distance < 5.0f && gameObject.transform.position.y > player.transform.position.y)
            {
                rb.gravityScale = 1.0f;
                fallen = true;
                startTime = Time.time;
            }
        }
        if (fallen)
        {
            if(Time.time-startTime > 2.0f)
            {
                Debug.Log("ded");
                GameObject.Destroy(gameObject);
            }
        }
    }
}

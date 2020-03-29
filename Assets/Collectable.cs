using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    bool began = false;
    float onTime = 0;
    GameObject player;
    public int type;
    GameObject inventory;
    static int count;
    System.Random rng;

    public AudioClip collectNoise;

    void Start()
    {
        count++;
        rng = new System.Random(count);
        
        inventory = GameObject.Find("Inventory");
        player = GameObject.Find("Player");
        began = true; //bad lol
        onTime = 10.0f;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3((float) rng.NextDouble() * 10, (float)rng.NextDouble() * 10, 0);
    }

   
    void Update()
    {
        if (began)
        {
            if((player.transform.position- transform.position).magnitude < 1.5)
            {
                AudioSource.PlayClipAtPoint(collectNoise, transform.position);

                inventory.GetComponent<Colliection>().AddType(type, 1);
                Destroy(gameObject);

            }
            onTime -= Time.deltaTime;
            if(onTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

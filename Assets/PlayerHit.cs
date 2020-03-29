using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public float damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.gameObject.name == "Player")  //layers not working
        // {
        // GameObject.Find("Player").GetComponent<PlayerControl>().Damage(10);

        // }
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        
        if (playerHealth)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}

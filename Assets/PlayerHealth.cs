using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    private GameObject healthBar;
    private Camera Camera => Camera.current;
    private float shakeTime;

    private void Start()
    {
        healthBar = GameObject.Find("HealthBar");
        healthBar.GetComponent<Slider>().value = 1;
    }

    private void Update()
    {
        CheckShake();
    }

    public override void TakeDamage(float amount)
    {
        // base.TakeDamage(amount);
        currentHealth = currentHealth - amount;
        Debug.Log("take damage");
        healthBar.GetComponent<Slider>().value = currentHealth / 100.0f;
        shakeTime = 0.3f;

        if (currentHealth <= 0)
        {
            // Kill player here
        }
    }
    
    

    System.Random rand;
    private void CheckShake()
    {
        if (rand == null) rand = new System.Random();
        if(shakeTime > 0)
        {
            float x = (float) rand.NextDouble() * shakeTime;
            float y = (float) rand.NextDouble() * shakeTime;
    
            shakeTime -= Time.deltaTime;
            if(shakeTime < 0)
            {
                Camera.transform.localPosition = new Vector3(0,0,-10);

            }
        }
    }
}
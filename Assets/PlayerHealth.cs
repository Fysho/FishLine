using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    private GameObject healthBar;
    private Camera Camera => Camera.current;
    public GameObject cam;
    private float shakeTime;
    float iframes;
    float maxIframe;
    private void Start()
    {
        healthBar = GameObject.Find("HealthBar");
        healthBar.GetComponent<Slider>().value = 1;
        maxIframe = 1;
        iframes = 0;
        currentHealth = 100;
    }

    private void Update()
    {
        CheckShake();
        if(iframes > 0)
        {
            iframes -= Time.deltaTime;
        }
    }

    public override void TakeDamage(float amount)
    {
        // base.TakeDamage(amount);
        Debug.Log($"Health at {currentHealth}");

        if(iframes <= 0)
        {

       
        currentHealth = currentHealth - amount;
        Debug.Log("take damage");
        healthBar.GetComponent<Slider>().value = currentHealth / 100.0f;
        shakeTime = 0.7f;

        if (currentHealth <= 0)
        {
            // Kill player here
        }
            iframes = maxIframe;
        }
        Debug.Log(currentHealth);

    }



    System.Random rand;
    private void CheckShake()
    {
        if (rand == null) rand = new System.Random();
        if(shakeTime > 0)
        {
            float x = (float) rand.NextDouble() * shakeTime;
            float y = (float) rand.NextDouble() * shakeTime;
            cam.transform.localPosition = new Vector3(x, y, -10);

            shakeTime -= Time.deltaTime;
            if(shakeTime < 0)
            {
                cam.transform.localPosition = new Vector3(0,0,-10);

            }
        }
    }
}
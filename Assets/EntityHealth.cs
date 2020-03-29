using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour, IHealth
{
    [Min(1)]
    public float startingHealth = 100;
    [Min(1)]
    public float maxHealth = 100;
    [Min(0)]
    public float armour;
    
    protected float currentHealth;

    public float CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount / (armour + 1);
        if(currentHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
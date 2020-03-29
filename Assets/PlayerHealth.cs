using UnityEngine;

public class PlayerHealth : EntityHealth
{
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (currentHealth <= 0)
        {
            // Kill player here
        }
    }
}
using UnityEngine;
    
    
public interface IHealth
{
    float CurrentHealth { get; }
    
    void TakeDamage(float amount);

    void Heal(float amount);
}
using System;
using UnityEngine;

// A simple cleanup that destroys GameObject when particle system is done.
public class ParticleSystemCleanup : MonoBehaviour
{
    private ParticleSystem particleSystem;
    
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
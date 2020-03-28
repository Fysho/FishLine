using System;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public GameObject bombExplosion;
    private bool isDetonating;
    private float charge;
    private float timeToDetonation;
    private float age;
    private float blastRadius;
    private Rigidbody2D rbody;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDetonating)
        {
            age += Time.deltaTime;

            if (age > timeToDetonation)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        Instantiate(bombExplosion, transform.position, Quaternion.identity);
        GameObject cave = GameObject.Find("CaveGenerator");
        cave.GetComponent<CaveGenerator>().Explode(transform.position.x, transform.position.y, 12);
        int mask = 1 << 8;
        
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, blastRadius, mask);
        
        foreach (Collider collider in hitCollider)
        {
            // TODO explode code.
        }

        Destroy(gameObject);
    }

    public void SetDetonation(float charge, float timeToDetonation, float blastRadius)
    {
        this.charge = charge;
        this.timeToDetonation = timeToDetonation;
        this.blastRadius = blastRadius;
        this.isDetonating = true;
    }
}
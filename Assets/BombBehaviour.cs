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
    private float blastStrength;
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
        
        ExplodeTiles();
        ExplodeBodies();
        DamageBodies();

        Destroy(gameObject);
    }

    private void ExplodeTiles()
    {
        GameObject cave = GameObject.Find("CaveGenerator");
        cave.GetComponent<CaveGenerator>().Explode(transform.position.x, transform.position.y, blastRadius);
    }

    private void ExplodeBodies()
    {
        int mask = 1 << 8;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        
        foreach (Collider2D hitCollider in hitColliders)
        {
            Vector2 hitVector = hitCollider.transform.position - transform.position;
            float hitDistance = hitVector.magnitude;
            Vector2 hitDirection = hitVector.normalized;
            Vector2 hitForce = hitDirection * blastStrength / Mathf.Max(hitDistance, 0.001f);

            Rigidbody2D hitBody = hitCollider.GetComponent<Rigidbody2D>();
            MotionBody hitMotionBody = hitCollider.GetComponent<MotionBody>();

            if (hitMotionBody)
            {
                hitMotionBody.AddMotion(hitForce);
            }

            if (hitBody && !hitMotionBody)
            {
                hitBody.AddForce(hitForce);
            }
            
            Debug.DrawLine(transform.position, hitCollider.transform.position, Color.yellow, 5);
            
        }
        
        Debug.DrawRay(transform.position, Vector3.up * blastRadius, Color.red, 6);
        Debug.DrawRay(transform.position, Vector3.left * blastRadius, Color.red, 6);
        Debug.DrawRay(transform.position, Vector3.right * blastRadius, Color.red, 6);
        Debug.DrawRay(transform.position, Vector3.down * blastRadius, Color.red, 6);
    }

    private void DamageBodies()
    {
        int mask = 1 << 8;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 5);

        foreach (Collider2D hitCollider in hitColliders)
        {

            if (String.Compare(hitCollider.gameObject.tag, "Enemy") == 0)
            {
                hitCollider.gameObject.GetComponent<EntityHealth>().TakeDamage(40);
            }
        }
    }
    public void SetDetonation(float charge, float timeToDetonation, float blastRadius, float blastStrength)
    {
        this.charge = charge;
        this.timeToDetonation = timeToDetonation;
        this.blastRadius = blastRadius;
        this.isDetonating = true;
        this.blastStrength = blastStrength;
    }
}
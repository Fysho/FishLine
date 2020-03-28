using System;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float maxCharge = 2.5f;
    [SerializeField]
    private bool startWithNoBombs;
    public float bombStockMax = 3;
    public float bombReplenishSpeed = 1;

    [Header("Bomb Properties")]
    public GameObject bomb;
    public float bombSpawnDistance = 0.6f;
    public float bombSpeed = 35;
    [Tooltip("Time to detonation divided by the charge")]
    public AnimationCurve bombDetonationCurve;
    public float bombZPosition;
    public float bombBlastRadius;

    private float charge;
    private bool isCharging;
    private bool isFiring;
    private float bombStock;

    private void Start()
    {
        if (!startWithNoBombs)
            bombStock = bombStockMax;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Fire1") == 1)
        {
            if (!isFiring && bombStock > 1)
            {
                isCharging = true;
                charge += Time.deltaTime;

                if (charge > maxCharge)
                {
                    charge = maxCharge;
                    ReleaseBomb();
                }
            }
        }
        else if (isCharging)
        {
            ReleaseBomb();
        }
        else
        {
            isFiring = false;
        }
        bombStock = Mathf.Min(bombStock + bombReplenishSpeed * Time.deltaTime, bombStockMax);
    }

    private void ReleaseBomb()
    {
        bombStock--;
        Vector2 aimDirection = GetAimDirection();
        Vector3 bombLocation = transform.position + (Vector3) (aimDirection * bombSpawnDistance);
        bombLocation.z = bombZPosition;
        float timeToDetonation = 1 / bombDetonationCurve.Evaluate(charge / maxCharge);
        Debug.Log($"Releasing bomb with charge {charge} with time to detonation {timeToDetonation}s!");
        GameObject bombObject = Instantiate(bomb, bombLocation, Quaternion.identity);
        
        bombObject.GetComponent<BombBehaviour>()?.SetDetonation(charge, timeToDetonation, bombBlastRadius);
        bombObject.GetComponent<Rigidbody2D>()?.AddForce(aimDirection * bombSpeed, ForceMode2D.Impulse);

        isCharging = false;
        isFiring = true;
        charge = 0;
    }

    private Vector2 GetAimDirection()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    static int count;
    public GameObject ruby;
    public GameObject gold;
    public GameObject diamond;
    bool exploded;

    void Start()
    {
        exploded = false;        
    }

    void Update()
    {
        
    }

    public void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            count++;
            System.Random rng = new System.Random(count);
            int g = (int)(rng.NextDouble() * 10) + 2;
            int r = (int)(rng.NextDouble() * 5);
            int d = (int)(rng.NextDouble() * 2);

            for (int i = 0; i < g; i++)
            {
                Instantiate(gold, transform.position, Quaternion.identity);
            }
            for (int i = 0; i < g; i++)
            {
                Instantiate(ruby, transform.position, Quaternion.identity);
            }
            for (int i = 0; i < g; i++)
            {
                Instantiate(diamond, transform.position, Quaternion.identity);
            }
            gameObject.GetComponent<Animator>().SetBool("ChestOpen", true);

        }

    }
}

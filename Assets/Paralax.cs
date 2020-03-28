using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{

    public float xSens;
    public float ySens;
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(-player.transform.position.x * xSens, player.transform.position.y * ySens, 0);

    }
}

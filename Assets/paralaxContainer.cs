using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralaxContainer : MonoBehaviour
{

    int containers;
    public float xSize;
    public float speedRatio;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        containers = gameObject.transform.childCount;
    }

    void Update()
    {
        transform.position = new Vector3(-player.transform.position.x * speedRatio + player.transform.position.x, 0, 0);
    }
}

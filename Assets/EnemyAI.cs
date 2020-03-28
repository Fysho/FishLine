using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{

    int state = 0;
    float stateTime;
    private void Start()
    {
        stateTime = 0;
    }

    private void Update()
    {
        if(state == 0)
        {

        }
        if(state == 1)
        {

        }
    }

    //Tilemap tilemap;
    //float updateStart;
    //System.Random random;
    //float updateTime;
    //bool idleing = true;
    //float targetX;
    //float targetY;
    //float distanceToJump;
    //int dir = 0; //1left //2right
    //Rigidbody2D rigidBody;
    //float startX;
    //bool jumped;
    //float walkdistance;
    //void Start()
    //{
    //    tilemap = GameObject.Find("Collidable").GetComponent<Tilemap>();
    //    updateStart = Time.time;
    //    random = new System.Random();
    //    updateTime = 1;// (float) random.NextDouble() * 3 + 3;
    //    idleing = true;
    //    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (idleing)
    //    {
    //        if (Time.time - updateStart > updateTime)
    //        {
    //            startWalk();
    //        }
    //    }
    //    if (!idleing)
    //    {
    //        Vector3 setVel = new Vector3(0, 0, 0);
    //        float dt = Time.deltaTime;
    //        if (dir == 1) setVel = setVel + new Vector3(-500 * dt, 0, 0);
    //        if (dir == 2) setVel = setVel + new Vector3(500 * dt, 0, 0);
    //        setVel.y = rigidBody.velocity.y;
    //        rigidBody.velocity = setVel;
    //        float f = Mathf.Abs(transform.position.x - startX);
    //        if (Mathf.Abs(transform.position.x - startX) >= walkdistance)
    //        {
    //            idleing = true;
    //            updateStart = Time.time;
    //            updateTime = 1;// (float)random.NextDouble() * 3 + 3;
    //            Vector3 setVel2 = new Vector3(0, 0, 0);

    //            setVel2.y = rigidBody.velocity.y;
    //            rigidBody.velocity = setVel2;

    //        }
    //    }
    //}

    //void startWalk()
    //{
    //    idleing = false;
    //    targetX = 0;
    //    targetY = 0;
    //    distanceToJump = 0;
    //    startX = transform.position.x;
    //    jumped = false;
    //    walkdistance = 1;
    //    dir = (int) (random.NextDouble() * 2) + 1;
    //    Debug.Log(dir);

    //}
}

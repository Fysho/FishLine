using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update

    bool wInput = false;
    bool aInput = false;
    bool sInput = false;
    bool dInput = false;
    bool shiftInput = false;

    Rigidbody2D rigidBody;
    public CaveGenerator caveGenerator;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        
    }

    void Move()
    {
        float dt = Time.deltaTime;
        Vector3 setVel = new Vector3(0, 0, 0);
        if (aInput) setVel = setVel + new Vector3(!shiftInput ? - 500 * dt : -1250 * dt, 0, 0);
        if (dInput) setVel = setVel + new Vector3(!shiftInput ? 500 * dt : 1250 * dt, 0, 0);
        setVel.y = rigidBody.velocity.y;
        rigidBody.velocity = setVel;
        if (wInput)
        {
            rigidBody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            wInput = false;
        }
        if (sInput)
        {
            caveGenerator.Explode(transform.position.x, transform.position.y, 10);
        }
    }

    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W)){ wInput = true; }
        if (Input.GetKeyUp(KeyCode.W))  { wInput = false; }
        if (Input.GetKeyDown(KeyCode.A)){ aInput = true; }
        if (Input.GetKeyUp(KeyCode.A))  { aInput = false; }
        if (Input.GetKeyDown(KeyCode.S)){ sInput = true; }
        if (Input.GetKeyUp(KeyCode.S))  { sInput = false; }
        if (Input.GetKeyDown(KeyCode.D)){ dInput = true; }
        if (Input.GetKeyUp(KeyCode.D))  { dInput = false; }
        if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftInput = true; }
        if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftInput = false; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject player;
    public GameObject text;
    bool start;
    float time;
    void Start()
    {
        time = 0;
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            time += Time.deltaTime;
            if (time > 2.0f)
            {

                gameObject.GetComponent<Animator>().SetBool("DoorOpened", true);
                start = true;
            }
        }
        else
        {
            if ((player.transform.position - transform.position).magnitude < 5)
            {
                text.SetActive(true);
            }
            else
            {
                text.SetActive(false);
            };
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public GameObject player;
    public GameObject text;
    bool start;
    float time;
    bool exiting;
    bool exittest;
    void Start()
    {
        time = 0;
        start = false;
        exiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (exiting)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
        if (!start)
        {
            time += Time.deltaTime;
            if (time > 10.0f)
            {

                gameObject.GetComponent<Animator>().SetBool("DoorOpened", true);
                start = true;
            }
        }
        else
        {
            if ((player.transform.position - transform.position).magnitude < 2)
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return)){
                    GameObject.Find("Inventory").GetComponent<Colliection>().FinnishText();
                    exiting = true;
                }
            }
            else
            {
                text.SetActive(false);
            };
        }
        
    }
}

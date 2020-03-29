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
    float timeleft;
    void Start()
    {
        time = 0;
        start = false;
        exiting = false;
        timeleft = 190;
    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

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
            if (time > 1.0f)
            {

                gameObject.GetComponent<Animator>().SetBool("DoorOpened", true);
                start = true;
            }
        }
        else
        {
            if ((player.transform.position - transform.position).magnitude < 2 && timeleft >= 0)
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

    public void UpdateTime()
    {
        timeleft -= Time.deltaTime;
        float minutes = Mathf.Floor(timeleft / 60);
        float seconds = Mathf.RoundToInt(timeleft % 60);
        string mins = "00";
        string secs = "00";
        
        //if (minutes < 10)
        //{
        //    mins = "0" + minutes.ToString();
        //}
       // else
       // {
            mins = minutes.ToString();
       // }
        if (seconds < 10)
        {
            secs = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secs = Mathf.RoundToInt(seconds).ToString();
        }


        if(timeleft < 0)
        {
            gameObject.GetComponent<Animator>().SetBool("DoorOpened", true);
            GameObject.Find("timetext").GetComponent<UnityEngine.UI.Text>().text = "Out of time: Game Over";

        }
        else
        {
            GameObject.Find("timetext").GetComponent<UnityEngine.UI.Text>().text = "Time Left: " + mins + ":" + secs;

        }
    }
}

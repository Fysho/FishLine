using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Colliection : MonoBehaviour
{
    int gold = 0;
    int rubys = 0;
    int diamonds = 0;

    public Text goldText;
    public Text rubyText;
    public Text diamondText;

    public Text endtext;

    void Start()
    {
        
    }

    public void AddType(int type, int ammount)
    {
        if(type == 0)
        {
            gold += ammount;
        }
        else if(type == 1)
        {
            rubys += ammount;
        }
        else if (type == 2)
        {
            diamonds += ammount;
        }
        goldText.text = gold.ToString();
        rubyText.text = rubys.ToString();
        diamondText.text = diamonds.ToString();
    }

    public void FinnishText()
    {
        string test = "You finnished with " + gold + " gold, " + rubys + " rubies, and " + diamonds + " diamonds!";
        endtext.gameObject.SetActive(true);
        endtext.text = test;
    }
}

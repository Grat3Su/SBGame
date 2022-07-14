using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    PlayerBehaviour pb;
    TimeManager tm;
    public Slider[] desire = new Slider[3];//hunger clean energy
    float fSliderBarTime;


    // Start is called before the first frame update
    void Start()
    {
        pb = player.GetComponent<PlayerBehaviour>();
        tm = player.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hunger.value);
        if (desire != null)
        {
            for(int i = 0; i<3; i++)
            {
                desire[i].value = pb.getDesire(i) * 10;
            }
        }
    }


}

class Display
{
    public static int[] day(int sec)
    {
        int[] m = new int[4];
        m[0] = sec % 60;
        m[1] = (sec / 60) % 60;
        m[2] = ((sec / 60) / 60) % 24;
        m[3] = ((sec / 60) / 60) / 24;
        return m;
    }

    public static string elecTime(int sec)
    {
        return "00:00";
    }

    public static string hanTime(int sec)
    {
        return "0½Ã°£ 0 ºÐ";
    }
}
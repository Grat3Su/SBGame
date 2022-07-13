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
    public Text timeText;


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

    public void PrintTime()
    {
        if (timeText)
        {
            int printTime = tm.GetTime();
            int minute = printTime % 60;
            int hour = printTime / 60;

            timeText.text = string.Format($"{tm.GetDay()}일차 {hour}시 {minute}분");
        }
    }

}
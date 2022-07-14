using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    GameObject manager;
    PlayerManager pm;
    PlayerBehaviour pb;
    public Text timeText;

    float timer;
    public float timeSet = 10f;//게임에서의 1분
    private int printTime;//실제로 보여질 시간
    private int day;//날짜
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager");
        pm = manager.GetComponent<PlayerManager>();
        pb = GetComponent<PlayerBehaviour>();

        timer = 0;
        printTime = 0;
        day = 0;
        PrintTime();
    }

    // Update is called once per frame
    void Update()
    {        
        MoveTime();
    }

    void MoveTime()
    {
        timer += Time.deltaTime;

        if (timer > timeSet)
        {
            printTime += 1;
            timer = 0;

            int minute = printTime % 60;//10분마다 갱신
            if (minute % 10 == 0)
            {
                DayTime();
                PrintTime();
            }
        }
    }

    int curhour = 0;
    void DayTime()
    {
        int hour = printTime / 60;       

        if (curhour != hour)
        {
            pb.DecEnergy();
            curhour = hour;
        }

        if (hour >= 24)
        {
            day++;
            printTime -= 60 * 24;
        }
    }

    public int GetDay()
    {
        return day;
    }
    public int GetTime()
    {
        return printTime;
    }
    
    public void SetTime(int time)
    {
        printTime += time;
        DayTime();
    }
    public void PrintTime()
    {
        if (timeText)
        {
            int printTime = GetTime();
            int minute = printTime % 60;
            int hour = printTime / 60;

            timeText.text = string.Format($"{GetDay()}일차 {hour}시 {minute}분");
        }
    }
}

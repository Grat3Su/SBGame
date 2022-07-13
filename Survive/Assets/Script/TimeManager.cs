using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    GameObject manager;
    PlayerManager pm;
    PlayerBehaviour pb;

    float timer;
    public float timeSet = 10f;//���ӿ����� 1��
    private static int printTime;//������ ������ �ð�
    private static int day;//��¥
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager");
        pm = manager.GetComponent<PlayerManager>();
        pb = GetComponent<PlayerBehaviour>();

        timer = 0;
        printTime = 0;
        day = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveTime()
    {
        timer += Time.deltaTime;

        if (timer > timeSet)
        {
            printTime += 1;
            timer = 0;

            int minute = printTime % 60;//10�и��� ����
            if (minute % 10 == 0)
            {
                DayTime();
            }
        }
    }

    int curhour = 0;
    void DayTime()
    {
        int hour = printTime / 60;
        pm.PrintTime();

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
}

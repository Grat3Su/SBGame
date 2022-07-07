using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //플레이어 욕구
    public int[] p_desire;
    int desireIdx;

    //감소 시간
    public float dec_hunger = 1.0f;
    public float dec_energy = 1.0f;
    public float dec_clean = 1.0f;

    public float speed = 5.0f;
    public int dash_speed = 1;

    GameObject manager;
    PlayerManager pm;

    GameObject[] items;
    int itemidx;

    float timer;
    public float timeSet = 10f;//게임에서의 1분
    int printTime;//실제로 보여질 시간
    int day;//날짜

    public bool bedtime = false;

    // Start is called before the first frame update
    void Start()
    {
        p_desire = new int[3] { 10, 10, 10 };//0 : h / 1 : c / 2 : e        
        desireIdx = 3;

        manager = GameObject.Find("GameManager");
        pm = manager.GetComponent<PlayerManager>();
        items = new GameObject[10];
        itemidx = 0;

        timer = 0;
        printTime = 0;
        day = 0;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        MoveTime();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bedtime = true;
        }
    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            dash_speed = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            dash_speed = 1;
        }
        //쉽고 빠르고 간단한 움직이기
        float x = Input.GetAxis("Horizontal") * speed * dash_speed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * speed * dash_speed * Time.deltaTime;

        if (x != 0 || y != 0)
        {
            //Debug.Log(x + " " + y);
            transform.Translate(x, y, 0f);
        }
    }

    public int getDesire(int idx)
    {
        return p_desire[idx];
    }

    public void DecEnergy()//비율 수식
    {
        for (int i = 0; i < desireIdx; i++)
        {
            if (p_desire[i] > 0)
                p_desire[i]--;//시간당 한 칸씩 깎는다.
            if (p_desire[i] < 0)
                p_desire[i] = 0;
        }
    }

    public void IncEnergy(int idx, int incSize)
    {
        p_desire[idx] += incSize;
        if (p_desire[idx] > 10)
            p_desire[idx] = 10;
    }


    void MoveTime()
    {
        timer += Time.deltaTime;

        if (timer > timeSet)
        {
            printTime += 1;
            timer = 0;

            int minute = printTime % 60;//5분마다 갱신
            if (minute % 5 == 0)
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
            DecEnergy();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<item>() != null)
        {
            items[itemidx] = other.gameObject;//아이템 저장
            itemidx++;
        }
    }
}

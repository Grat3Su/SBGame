using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintUI : MonoBehaviour
{//만들어야할거 정리하고 부분 부분 구현해야할 거 구현하기.

    //자원/최대값

    //화면 전환(날짜 변경 시 하루 보고)
    Hero h;
    int people, _people;
    int food, _food;
    int water, _water;
    int day;
    
    void Start()
    {
        h = GameObject.Find("Hero").GetComponent<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setMax(int p, int f, int w)
    {
        //최댓값, 바뀔 때만 불러온다. (Lab 방문 - 최댓값 증가할 때)
        _people = p;
        _food = f;
        _water = w;
    }
    public void printHour()
    {
        //시간

    }

    public void printNextDay()//Hero에서 하루 지났을 때 부르기
	{
        //몇일차 표시
        //현재 값
        people = h.people;
        food = h.food;
        water = h.water;
        day = h.day;

        string pDay = day + "일차";
        string report = "남은 사람 : " + people + "명"
                        + "남은 음식 : " + food + "개"
                        + "남은 물 : " + water + "개";
        
    }
}

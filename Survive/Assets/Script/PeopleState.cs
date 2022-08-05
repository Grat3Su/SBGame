using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    // 생존자 행동
    // idle, move, atack..
    // 이벤트 시 taketime만큼 사라짐

    string name;//이름
    int behave;// 0 : idle / 1 : move / 2 : attack
    bool defence;//습격?
    public int takeTime;//맵에 없을 시간
    public int job = 0;// 0 : 백수 / 1 : 방위대원 / 2 : 일꾼 / 3 : 농부 / 4 : 연구원
    public Sprite[][] sp;
    GameManager h;
    
    void Start()
    {
        behave = 0;
        defence = false;
        h = GameObject.Find("GameManager").GetComponent<GameManager>();
        //job = 0;

        string[] n = {"가","나", "다", "라","마","바","사","아","자","차","카","타","파","하", "수","경","재","문"};

        name = n[Random.Range(0, n.Length)]+ n[Random.Range(0, n.Length)];
        Debug.Log(name+"구조");
        //이름 랜덤 생성
    }

    // Update is called once per frame
    void Update()
    {
        if(takeTime > 4)
            jobAction();
        if (defence)
            ;
    }

    void fight()
	{

	}
    

    void jobAction()//-> 
    {// 0 : 백수 / 1 : 방위대원 / 2 : 탐험가 / 3 : 농부 / 4 : 연구원
        //taketime ==4
        takeTime -= 4;
        if (job == 0)
		{
            //맵 배회
        }
        else if (job == 1)
        {
            //습격 시 싸우기
        }
        else if (job == 2)
        {
            //식량/물 가저오기
            int food = Random.Range(0, 1);
            int water = Random.Range(0, 1);            
            h.getItem(name, food, water);
        }
        else if (job == 3)
        {
            //식량 1 추가
            int food = Random.Range(0, 3);
            h.getItem(name, food, 0);
        }
        else if (job == 4)
        {
            //연구 포인트 1 추가
            h.labExp++;
        }
    }
}

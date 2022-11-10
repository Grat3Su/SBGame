using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState// : MonoBehaviour
{
	// 생존자 행동
	// idle, move, atack..
	// 이벤트 시 taketime만큼 사라짐

	public int[] jobLevel;
	int jobExp;
	public iPoint pos;
	public iPoint curPos;
	public iPoint nextPos;

	public string name;//이름
	public int behave;// 0 : idle / 1 : move / 2 : back / 3 : work / 4 : disease / 5: 사망	
	public float moveDt;
	int healthTime;
	public int takeTime;//맵에 없을 시간
	public int job = 0;// 0 : 백수 / 1 : 탐험가 / 2 : 일꾼 / 3 : 농부 / 4 : 연구원
	public int jobReserve;
	public Sprite[][] sp;
	public Event h;

	public PeopleState()
	{
		name = "null";

		init();
	}
		
	void init()
	{
		pos = new iPoint(0, 0);
		curPos = new iPoint(0, 0);
		nextPos = new iPoint(0, 0);
		behave = 0;
		moveDt = 0f;
		jobLevel = new int[5] { 0, 1, 1, 1, 1 };//한번 일이 끝날때마다 레벨 상승?
		jobExp = 0;
		healthTime = 3;

		//job = 0;
		jobReserve = -1;		
	}

	// Update is called once per frame
	public void update()
	{
		if (name == "null")
			return;
		if (takeTime > 3 && behave==2)
			jobAction();
		else if(behave == 4)
		{
			if (healthTime > 0)
			{
				healthTime--;
			}
			else if (healthTime == 0)
			{
				healthTime = 3;
				behave = 0;
				Debug.Log("나음");
			}
			takeTime -= 4;
		}

		if (behave == 5)
			name = "null";
	}

	void jobAction()//-> 
	{// 0 : 백수 / 1 : 방위대원 / 2 : 탐험가 / 3 : 농부 / 4 : 연구원
	 //taketime ==4
		takeTime -= 4;
		if (takeTime < 0)
			takeTime = 0;
		//Debug.Log("work");
		int bonus = jobLevel[job] * 2;

		if (job == 0)
		{
			//맵 배회
			//behave = 0;
		}
		else if (job == 1)
		{
			//탐험. 가끔 생존자 발견
			if (Random.Range(0, 100) > (80 - bonus))
			{
				h.storage.addStorage(0, 1);
				h.plusItem.people += 1;
			}
			h.storage.addStorage(4, 2 + bonus);
			h.plusItem.mapExp += 2;
		}
		else if (job == 2)
		{
			//식량/물 가저오기
			int mount = Math.random(bonus, bonus + 3);
			h.plusItem.food += mount;
			h.storage.addStorage(1, mount);
		}
		else if (job == 3)
		{
			//식량 추가
			int mount = Math.random(bonus, bonus + 2);
			h.storage.addStorage(1, mount);
			h.plusItem.food += mount;
		}
		else if (job == 4)
		{
			//연구 포인트 2 추가
			int mount = Math.random(bonus, bonus + 2);
			h.storage.addStorage(3, mount);
			h.plusItem.labExp += mount;
		}

		jobExp += 2;
		if (jobExp > jobLevel[job] * 5)
		{
			jobExp -= (jobLevel[job]-1) * 5;
			jobLevel[job]++;
		}
	}

	//직업 변경
	public void jobUpdate(int newjob)
	{
		if (newjob == job)//새 직업과 다를 때
			return;

		job = newjob;
		jobExp = 0;//경험치 초기화
	}
}
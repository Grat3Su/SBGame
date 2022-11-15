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
	float healthTime;
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

	public void update(float dt)
	{
		if (takeTime > 3 && behave!=4)
			jobAction();
		else if(behave == 4)
		{
			if (healthTime > 0)
			{
				healthTime-=dt;
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
	{// 0 : 백수 / 1 : 탐험가 / 2 : 일뀬 / 3 : 농부 / 4 : 연구원
	 //taketime ==4
		if (h == null)
		{
			h = MainCamera.mainCamera.GetComponent<Event>();
			Debug.Log(h.gameObject.name);
		}
		takeTime -= 4;
		if (takeTime < 0)
			takeTime = 0;
		//Debug.Log("work");
		int bonus = (int)(jobLevel[job] * 0.5f);

		if (job == 0)
		{
			//맵 배회
			//behave = 0;
		}
		else if (job == 1)
		{
			bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
			//탐험. 가끔 생존자 발견
			if (Random.Range(0, 100) > (80 - bonus))
			{
				h.storage.addStorage(0, 1);
				h.spawnPeople();
				h.plusItem.people += 1;
			}
			h.storage.addStorage(4, 1 + bonus);
			h.plusItem.mapExp += 2;
		}
		else if (job == 2)
		{
			//식량 가저오기
			bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
			int mount = Math.random(1, bonus + 1);
			h.plusItem.food += mount;
			h.storage.addStorage(1, mount);
		}
		else if (job == 3)
		{
			//식량 추가
			int mount = Math.random(1, bonus + 1);
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
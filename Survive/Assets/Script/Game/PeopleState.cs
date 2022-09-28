using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
	// 생존자 행동
	// idle, move, atack..
	// 이벤트 시 taketime만큼 사라짐

	public int[] jobLevel;
	int jobExp;

	string name;//이름
	public int behave;// 0 : idle / 1 : move / 2 : attack / 2 : work / 3 : disease
	bool defence;//습격?
	public int takeTime;//맵에 없을 시간
	public int job = 0;// 0 : 백수 / 1 : 탐험가 / 2 : 일꾼 / 3 : 농부 / 4 : 연구원
	public Sprite[][] sp;
	public Event h;
	public Texture jobTex;

	void Start()
	{
		behave = 0;
		defence = false;
		jobLevel = new int[5] { 0, 1, 1, 1, 1 };//한번 일이 끝날때마다 레벨 상승?
		jobExp = 0;

		string[] n = { "가", "나", "다", "라", "마", "바", "사", "아", "자", "차", "카", "타", "파", "하", "야", "샤", "수", "경", "재", "문" };
		jobTex = Resources.Load<Texture>("jobless");
		name = n[Math.random(0, n.Length)] + n[Math.random(0, n.Length)];
		//Debug.Log(name + "구조");
		gameObject.name = name;
		//이름 랜덤 생성

		job = 0;
		int newjob = Math.random(0, 5);
		gameObject.AddComponent<SpriteRenderer>().sortingLayerName = "Layer 1";
		jobUpdate(newjob);
	}

	// Update is called once per frame
	void Update()
	{
		if (takeTime > 4)
			jobAction();
	}

	//강제 복귀
	public void comeBack()
	{
		jobExp += 1;

		if (jobLevel[job] < 20)
		{
			if (Math.random(0, 80 + jobLevel[job]) > 20)
			{
				//Debug.Log("아무것도 얻지 못함");

				if (jobExp > jobLevel[job] * 2)
				{
					jobExp -= 2;
					jobLevel[job]++;
				}
			}
		}

		if (jobExp > jobLevel[job] * 2)
		{
			jobExp -= 2;
			jobLevel[job]++;
		}
		//2시간 이상이면 절반 가져옴
		if (takeTime < 2)
		{
			takeTime = 0;
			return;
		}

		if (job == 0)
		{
			//맵 배회
		}
		else if (job == 1)
		{
			//Debug.Log(name + "복귀");
			//탐험. 가끔 생존자 발견
			h.storage.addStorage(4, 1);
			h.plusItem.mapExp += 1;

			int people = Math.random(0, 100);
			if (people > 80)
			{
				h.storage.addStorage(0, 1);
				h.plusItem.people += 1;
				//Debug.Log(name + "이 생존자 발견");
			}
			//Debug.Log(name + " 맵 경험치 1 획득");
		}
		else if (job == 2)
		{
			jobTex = Resources.Load<Texture>("people");
			//Debug.Log(name + "복귀");
			//식량/물 가저오기
			int food = (int)((float)Math.random(0, 3) / 2 + 0.5f);
			h.storage.addStorage(1, food);
			//Debug.Log(name + " 식량 " + food + " 획득");
			h.plusItem.food += food;
		}
		else if (job == 3)
		{
			Debug.Log(name + "복귀");
			//식량 1 추가
			int food = (int)((float)Math.random(3, 4) / 2 + 0.5f);
			h.storage.addStorage(1, food);
			h.plusItem.food += food;
		}
		else if (job == 4)
		{
			//Debug.Log(name + "복귀");
			//연구 포인트 1 추가
			h.storage.addStorage(3, 2);
			h.plusItem.labExp += 1;
			//Debug.Log(name + " 연구 경험치 1 획득");
		}
	}

	void jobAction()//-> 
	{// 0 : 백수 / 1 : 방위대원 / 2 : 탐험가 / 3 : 농부 / 4 : 연구원
	 //taketime ==4
		takeTime -= 4;

		if (job == 0)
		{
			//맵 배회
			behave = 0;
		}
		else if (job == 1)
		{
			behave = 3;
			//탐험. 가끔 생존자 발견
			h.storage.addStorage(4, 2);
			h.plusItem.mapExp += 2;
			int people = Math.random(0, 100);
			if (people > 80)
			{
				h.storage.addStorage(0, 1);
				h.plusItem.people += 1;
				//Debug.Log(name + "이 생존자 발견");
			}
			//Debug.Log(name + " 맵 경험치 2 획득");
		}
		else if (job == 2)
		{
			behave = 3;
			//식량/물 가저오기
			int food = Math.random(0, 3);
			h.storage.addStorage(1, food);
			h.plusItem.food += food;

			//Debug.Log(name + " 식량 " + food + " 획득");
		}
		else if (job == 3)
		{
			behave = 3;
			//식량 1 추가
			int food = Math.random(3, 4);
			h.storage.addStorage(1, food);
			h.plusItem.food += food;
			//Debug.Log(name + " 식량 " + food + " 획득");
		}
		else if (job == 4)
		{
			behave = 3;
			//연구 포인트 2 추가
			h.storage.addStorage(3, 2);
			h.plusItem.labExp += 1;
			//Debug.Log(name + " 연구 경험치 2 획득");
		}
		jobExp += 2;
		if (jobExp > jobLevel[job] * 2)
		{
			jobExp -= 2;
			jobLevel[job]++;
		}
	}

	//직업 변경
	public void jobUpdate(int newjob)
	{
		if (newjob == job)//새 직업과 다를 때
			return;

		if (job == 0)
		{
		}
		else if (job == 1)
		{
			h.explorer--;
		}
		else if (job == 2)
		{
			h.workman--;
		}

		job = newjob;
		jobExp = 0;//경험치 초기화

		Sprite sp = Resources.Load<Sprite>("jobless");
		if (job == 0)
		{
			jobTex = Resources.Load<Texture>("jobless");
			sp = Resources.Load<Sprite>("jobless");
		}
		else if (job == 1)
		{
			jobTex = Resources.Load<Texture>("explorer");
			sp = Resources.Load<Sprite>("explorer");
		}
		else if (job == 2)
		{
			jobTex = Resources.Load<Texture>("worker");
			sp = Resources.Load<Sprite>("worker");
		}
		else if (job == 3)
		{
			jobTex = Resources.Load<Texture>("farmer");
			sp = Resources.Load<Sprite>("farmer");
		}
		else if (job == 4)
		{
			jobTex = Resources.Load<Texture>("researcher");
			sp = Resources.Load<Sprite>("researcher");
		}
		gameObject.GetComponent<SpriteRenderer>().sprite = sp;
	}
}

class newpState
{//해야하는 일 : 각자일
	public string pName;
	public int job;
	public int level;
	public float exp;

	public int takeTime;

	void Start()
	{
		//이름은 외부에서 지정
		job = 0;
		level = 0;
		exp = 0;
		takeTime = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (takeTime > 3)
			jobAction();
	}

	public void jobUpdate(int newJob)
	{
		job = newJob;
	}

	public void jobAction()// 0 : 백수 / 1 : 탐험가 / 2 : 일꾼 / 3 : 농부 / 4 : 연구원
	{
		takeTime -= 4;
		if (takeTime < 0)
			takeTime = 0;

		int bonus = level * 2;
		AddItem item = new AddItem(0);
		if (job == 1)//탐험가
		{
			//맵을 탐험해 맵 경험치를 늘리고 가끔 사람을 구해온다
			if (Random.Range(0, 100) > (80 - bonus))
				item.people = 1;

			item.mapExp = 2 + bonus;//레벨에 따라 얻는 량 달라짐

		}
		else if (job == 2)//일꾼
		{
			//탐험한 맵을 바탕으로 맵의 자원을 수집한다
			int mount = Random.Range(0, 5);
			item.food = mount;

		}
		else if (job == 3)//농부
		{
			// 농사를 지어 안정적으로 식량을 수급한다
			int mount = Random.Range(0, 2) + 2 * level;
			item.food = mount;
		}
		else if (job == 4)//연구원
		{
			//연구해서 연구 경험치를 올린다.
			//연구 레벨이 오르면 자원 상한량이 오른다.
			int mount = Random.Range(0, 2) + 2 * level;
			item.labExp = mount;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using STD;


public class Hero : MonoBehaviour
{
	public Text[] mount;
	PlayerState ps;
	GameData gd;
	Item[] item = new Item[10];
	public GameObject DayNight;//밤이면 켜
	PrintUI pu;

	int itemlength;
	public int day, hour;
	bool death;

	public int people, _people;//생존자. 이벤트 시 랜덤하게 획득
	public int food, _food;//하루마다 사람수만큼 차감
	public int water, _water;
	public int labExp, labLevel;

	int need;

	// Start is called before the first frame update
	void Start()
	{
		ps = new PlayerState(3, 3, 5, 10, 0, null);
		pu = GameObject.Find("GameManager").GetComponent<PrintUI>();
		itemlength = 0;

		resetGame();
		load();

		need = needLabLvup();
	}

	void resetGame()
	{
		day = 0;
		death = false;
		hour = 0;
		labLevel = 0;
		labExp = 0;
		lab();
		people = 1;//처음은 혼자 시작
		water = _water;
		food = _food;

		mount[0].text = people.ToString();
		mount[1].text = food.ToString();
		mount[2].text = water.ToString();
	}

	bool mgtGame()
	{
		if (Input.GetKeyDown(KeyCode.L))
			load();
		else if (Input.GetKeyDown(KeyCode.S))//입출력
			save();
		else if (Input.GetKeyDown(KeyCode.Escape))//리셋
			resetGame();
		else if (Input.GetKeyDown(KeyCode.W))
		{
			water -= 5;
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			food -= 5;
		}
		if (death)
		{
			if (Input.GetKeyDown(KeyCode.Space))//게임 새로 시작
			{
				if (el != null)
					el.deleteStr();
				resetGame();
			}
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update()
	{
		// cheat key;;;;
		if (mgtGame())
			return;

		// real
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			hour = 0;
			nextDay();
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))//탐색
			displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 0));
		else if (Input.GetKeyDown(KeyCode.LeftArrow))//사냥
			displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 1));
		else if (Input.GetKeyDown(KeyCode.UpArrow))//연구
			displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 4));
		else if (Input.GetKeyDown(KeyCode.Space))
			displayReal(doEvent(Random.Range(0, 40) / 10));

	}

	void save()
	{
		gd.day = day;
		gd.hour = hour;
		gd.food = food;
		gd.people = people;
		gd.water = water;
		gd.labLevel = labLevel;
		gd.labExp = labExp;
		gd._people = _people;
		gd._food = _food;
		gd._water = _water;

		byte[] bytes = FileIO.struct2bytes(gd);//바이트로 변경
		FileIO.save("GameData.sav", bytes);//저장
		Debug.Log("save");
	}

	void load()
	{
		byte[] bytes = FileIO.load("GameData.sav");
		if (bytes == null)
			return;

		Debug.Log("load");
		gd = FileIO.bytes2object<GameData>(bytes);

		day = gd.day;
		hour = gd.hour;
		food = gd.food;
		_food = gd._food;
		water = gd.water;
		_water = gd._water;
		people = gd.people;
		_people = gd._people;
		labExp = gd.labExp;
		labLevel = gd.labLevel;
	}

	public struct ETResult
	{
		public ETResult(int p, int f, int w, int t)
		{
			type = 0;
			people = p;
			food = f;
			water = w;
			takeTime = t;
			labExp = 0;
		}

		public int type;
		public int people;
		public int food;
		public int water;
		public int takeTime;
		public int labExp;
	}

	ETResult doEvent(int eventType)
	{
		ETResult result = new ETResult(0, 0, 0, 0);
		result.type = eventType;

		int p = people / 2;

		if (eventType == 0)
		{
			//탐색
			result.people = Random.Range(0, 3);
			result.water = (3 + Random.Range(0, p));//인구 수 보너스
			result.food = (Random.Range(0, p));
			result.takeTime = 4;
		}
		else if (eventType == 1)
		{
			//사냥
			result.people = Random.Range(0, 2);
			result.food = (3 + Random.Range(0, p));
			result.water = (Random.Range(0, p));//인구 수 보너스
			result.takeTime = 4;
		}
		else if (eventType == 2)
		{   // 습격		사람 사망, 물자 뺏길수 있음
			result.people = -Random.Range(0, p);
			result.food = -Random.Range(0, p);
			result.water = -Random.Range(0, p);
			result.takeTime = 6;
		}
		else if (eventType == 3)
		{
			//병
			int weakp = Random.Range(1, p);
			//아픈 사람만큼 소모
			result.water = -weakp;
			result.food = -weakp;

			if (people > 5)
				result.people = -Random.Range(0, weakp);
			result.takeTime = 5;
		}
		else if (eventType == 4)
		{
			//연구
			result.labExp = Random.Range(1, 3);// random
			result.takeTime = 4;
		}
		else
		{
			Debug.Log("아무일도 없었음");
		}

		return result;
	}

	void lab()
	{
		if (labLevel == 0)
		{
			// initialize
			_people = 3;
			_food = 5;
			_water = 5;
			pu.setMax(_people, _food, _water);
		}
		else if (labLevel % 5 == 0)
		{//보너스
			if (labLevel < 10)
			{
				water += labLevel * 5;
				food += labLevel * 5;
				return;
			}
			water += labLevel * 2;
			food += labLevel * 2;
		}
		else
		{//최댓값 증가
			_people += 5;
			_food   += 10;
			_water  += 10;
			pu.setMax(_people, _food, _water);
		}
	}

	void displayReal(ETResult result)
	{
		displayTest(result);
	}

	public EventLog el = null;
	void displayTest(ETResult result)
	{
		if (el == null)
			el = new EventLog();

		string[] str = new string[] { "탐색 ", "사냥 ", "습격 당", "병이 발생 ", "연구 " };
		el.add(str[result.type] + "했습니다.");

		labExp += result.labExp;

		while (labExp > need)
		{
			labExp -= need;
			labLevel++;
			lab();
			need = needLabLvup();
		}

		if (result.people > 0)
			el.add(result.people + "명을 구했습니다.");
		else if (result.people < 0)
			el.add(result.people + "명 사망했습니다.");

		people += result.people;
		if (people > _people)
			people = _people;
		else if (people < 0)
			people = 0;

		food += result.food;
		if (food > _food)
			food = _food;
		else if (food < 0)
			food = 0;

		water += result.water;
		if (water > _water)
			water = _water;
		else if (water < 0)
			water = 0;

		hour += result.takeTime;
		if (hour > 11)
		{
			nextDay();
			hour -= 12;
		}

		mount[0].text = people.ToString();
		mount[1].text = food.ToString();
		mount[2].text = water.ToString();
	}

	int needLabLvup()
	{
		if (labLevel < 5)
			return labLevel * 4;
		return 20 + labLevel * 2;
	}

	//
	void nextDay()
	{
		int dpeople = 0;
		//식량 소비
		if (food < people)
		{
			dpeople += people - food;
			Debug.Log(people - food + "명 사망");
			people -= (people - food);//식량이 부족한 수만큼 줄기
			food = 0;//다먹음
		}
		else
			food -= people;//사람 수만큼 줄이기

		if (water < people)
		{
			dpeople += people - water;
			Debug.Log(people - water + "명 사망");
			people -= (people - water);//물이 부족한 수만큼 줄기
			water = 0;//다먹음
		}
		else
			water -= people;//사람 수만큼 줄이기

		if (people <= 0)
		{
			people = 0;
			el.add("#####################");
			el.add("남은 생존자가 없다");
			el.add(day + "일 동안 생존");

			Debug.Log("남은 생존자가 없다");
			Debug.Log(day + "일 동안 생존");
			death = true;
		}
		else
		el.addToday(dpeople, people, food, water);

		day++;
		pu.printNextDay();//다음 날 print

		doEvent(Random.Range(0,100) < 10 ? 3 : 100);

		// 새로운날 표시day
		mount[0].text = people.ToString();
		mount[1].text = food.ToString();
		mount[2].text = water.ToString();

		Debug.LogFormat($"{day}일차 음식 {food}개, 물 {water}개, 인구 {people}명");//음식 물 자원 인구 수
	}

	void UseItem()
	{
		if (Input.GetKeyDown((KeyCode.Alpha0)))
		{
			deleteItem(9);
		}
		else
			for (int i = 1; i < 10; i++)
			{
				if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1 + i)))
				{
					deleteItem(i - 1);
				}
			}
	}

	void addItem(Item i)
	{
		itemlength++;
		if (itemlength > 10)//초과는 안됨
			itemlength = 10;

		for (int j = 0; j < itemlength; j++)
		{
			if (item[j] == null)
			{
				item[j] = i;
			}
			else if (i.name == item[j].name)//이름이 같으면 수량만 올린다
			{
				item[j].count += i.count;
				itemlength--;
				return;
			}
		}
	}

	void deleteItem(int idx)
	{
		itemlength--;
		for (int i = idx; i < itemlength; i++)
		{
			item[i] = item[i + 1];
		}
	}
}

struct GameData
{
	public int day;
	public int hour;
	public int food;
	public int _food;//하루마다 사람수만큼 차감
	public int water;
	public int _water;
	public int people;
	public int _people;
	public int labExp;
	public int labLevel;
}
class PlayerState
{
	//0 h 1 e 2 c
	public int[] desire;
	public int money;
	public Item[] item = new Item[10];

	public PlayerState(int h, int e, int c, int a, int m, Item[] i)
	{
		desire = new int[3];
		desire[0] = h;
		desire[1] = e;
		desire[2] = c;
		item = i;
		money = m;
	}
}
class Item
{
	public string name;//이름
	public int itemType;//0:기타 1:장비 2:소모품 3: 음식
	public int effect;//효과
	public int money;//판매할 때 얼마를 받나
	public int count;
	public Item(string n, int t, int c, int e, int m)
	{
		name = n;
		itemType = t;
		count = c;
		effect = e;
		money = m;
	}
}


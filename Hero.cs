using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using STD;


public class Hero : MonoBehaviour
{
	public Text[] mount;
	GameData gd;
	Item[] item = new Item[10];
	public GameObject DayNight;//���̸� ��
	PrintUI pu;
	PeopleState[] pState;//������

	int pintdex;

	int itemlength;
	public int day, hour;
	bool death;

	public int people, _people;//������. �̺�Ʈ �� �����ϰ� ȹ��
	public int food, _food;//�Ϸ縶�� �������ŭ ����
	public int water, _water;
	public int labExp, labLevel;
	
	int need;

	// Start is called before the first frame update
	void Start()
	{
		pu = GameObject.Find("GameManager").GetComponent<PrintUI>();
		itemlength = 0;
		pState = new PeopleState[100];
		pintdex = _people;
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
		people = 1;//ó���� ȥ�� ����
		water = _water;
		food = _food;
		deletePeople();

		mount[0].text = people.ToString();
		mount[1].text = food.ToString();
		mount[2].text = water.ToString();
	}

	bool mgtGame()
	{
		if (Input.GetKeyDown(KeyCode.L))
			load();
		else if (Input.GetKeyDown(KeyCode.S))//�����
			save();
		else if (Input.GetKeyDown(KeyCode.Escape))//����
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
			if (Input.GetKeyDown(KeyCode.Space))//���� ���� ����
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
		if (Input.GetKeyDown(KeyCode.RightArrow))//Ž��
			displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 0));
		else if (Input.GetKeyDown(KeyCode.LeftArrow))//���
			displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 1));
		else if (Input.GetKeyDown(KeyCode.UpArrow))//����
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

		byte[] bytes = FileIO.struct2bytes(gd);//����Ʈ�� ����
		FileIO.save("GameData.sav", bytes);//����
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

	public ETResult doEvent(int eventType)
	{
		ETResult result = new ETResult(0, 0, 0, 0);
		result.type = eventType;

		int p = people / 2;

		if (eventType == 0)
		{
			//Ž��
			result.people = Random.Range(0, 3);
			result.water = (3 + Random.Range(0, p));//�α� �� ���ʽ�
			result.food = (Random.Range(0, p));
			result.takeTime = 4;
		}
		else if (eventType == 1)
		{
			//���
			result.people = Random.Range(0, 2);
			result.food = (3 + Random.Range(0, p));
			result.water = (Random.Range(0, p));//�α� �� ���ʽ�
			result.takeTime = 4;
		}
		else if (eventType == 2)
		{   // ����		��� ���, ���� ����� ����
			result.people = -Random.Range(0, p);
			result.food = -Random.Range(0, p);
			result.water = -Random.Range(0, p);
			result.takeTime = 6;
		}
		else if (eventType == 3)
		{
			//��
			int weakp = Random.Range(1, p);
			//���� �����ŭ �Ҹ�
			result.water = -weakp;
			result.food = -weakp;

			if (people > 5)
				result.people = -Random.Range(0, weakp);
			result.takeTime = 5;
		}
		else if (eventType == 4)
		{
			//����
			result.labExp = Random.Range(1, 3);// random
			result.takeTime = 4;
		}
		else
		{
			Debug.Log("�ƹ��ϵ� ������");
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
		{//���ʽ�
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
		{//�ִ� ����
			_people += 5;
			_food   += 10;
			_water  += 10;
			pu.setMax(_people, _food, _water);
		}
	}

	public void displayReal(ETResult result)
	{
		displayTest(result);
	}

	public EventLog el = null;
	void displayTest(ETResult result)
	{
		if (el == null)
			el = new EventLog();

		string[] str = new string[] { "Ž�� ", "��� ", "���� ��", "���� �߻� ", "���� " };
		el.add(str[result.type] + "�߽��ϴ�.");

		labExp += result.labExp;

		while (labExp > need)
		{
			labExp -= need;
			labLevel++;
			lab();
			need = needLabLvup();
		}

		if (result.people > 0)
			el.add(result.people + "���� ���߽��ϴ�.");
		else if (result.people < 0)
			el.add(result.people + "�� ����߽��ϴ�.");

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
		
		//if(result.people<0)
		deletePeople();
		if (result.people > 0)
		{			
			spawnPeople();
		}
		for (int i = 0; i < people-1; i++)
			pState[i].takeTime += result.takeTime;


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

	//����
	void spawnPeople()
	{
		for (int i = 0; i < people-1; i++)
		{
			if (pState[i] == null)
			{
				GameObject go = new GameObject();

				//go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("meat");
				//go.transform.Scale = new Vector2(5, 5);

				float height = Camera.main.orthographicSize;
				float width = Camera.main.aspect * height;
				float x = Random.Range(-width, width);
				float y = Random.Range(-height, height);
				go.transform.position = new Vector3(x, y, 0);
				go.AddComponent<PeopleState>();
				go.GetComponent<PeopleState>().job = Random.Range(0,5);
				go.name = "people" + i.ToString();
				pState[i] = go.GetComponent<PeopleState>();
				//Instantiate<GameObject>(go); : ����
			}
		}
	}
	void deletePeople()
	{
		for(int i = people-1; i<100;i++)
		{
			if (pState[i] == null)
				return;
			
			Destroy(pState[i].gameObject);
			pState[i] = null;
		}	
	}

	//
	void nextDay()
	{
		int dpeople = 0;
		//�ķ� �Һ�
		if (food < people)
		{
			dpeople += people - food;
			Debug.Log(people - food + "�� ���");
			people -= (people - food);//�ķ��� ������ ����ŭ �ٱ�
			food = 0;//�ٸ���
		}
		else
			food -= people;//��� ����ŭ ���̱�

		if (water < people)
		{
			dpeople += people - water;
			Debug.Log(people - water + "�� ���");
			people -= (people - water);//���� ������ ����ŭ �ٱ�
			water = 0;//�ٸ���
		}
		else
			water -= people;//��� ����ŭ ���̱�

		if (people <= 0)
		{
			people = 0;
			el.add("#####################");
			el.add("���� �����ڰ� ����");
			el.add(day + "�� ���� ����");

			Debug.Log("���� �����ڰ� ����");
			Debug.Log(day + "�� ���� ����");
			death = true;
		}
		else
		el.addToday(dpeople, people, food, water);

		day++;
		pu.printNextDay();//���� �� print

		doEvent(Random.Range(0,100) < 10 ? 3 : 100);

		// ���ο ǥ��day
		mount[0].text = people.ToString();
		mount[1].text = food.ToString();
		mount[2].text = water.ToString();

		Debug.LogFormat($"{day}���� ���� {food}��, �� {water}��, �α� {people}��");//���� �� �ڿ� �α� ��
	}

	public void getItem(string name, int f, int w)
	{
		if(f==0&&w==0)
			el.add(name + "�� �ƹ��͵� �߰����� ���߽��ϴ�.");
		else
			el.add(name+"�� �ķ� " + f+ "�� ��" + w +"�� ȹ��");
		food  += f;
		water += w;
	}
}

struct GameData
{
	public int day;
	public int hour;
	public int food;
	public int _food;//�Ϸ縶�� �������ŭ ����
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
	public string name;//�̸�
	public int itemType;//0:��Ÿ 1:��� 2:�Ҹ�ǰ 3: ����
	public int effect;//ȿ��
	public int money;//�Ǹ��� �� �󸶸� �޳�
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


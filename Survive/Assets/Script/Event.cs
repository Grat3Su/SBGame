using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        pState = new PeopleState[100];
        initGame();
    }

    bool gameover;
    public Storage storage;
    int day;
    int hour;
    public PeopleState[] pState;
    public int workman;
    public int explorer;

    void initGame()
	{
        storage = new Storage(1, 5, 5, 0, 1,0,1);
        day = 0;
        hour = 0;
        workman = 0;
        explorer = 0;
        deletePeople();
        gameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameover)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
            doEvent((DoEvent)Random.Range(0, 3));
    }

    enum DoEvent
	{
        Adventure,
        Hunt,
        Research,
        Defense,
        Disease
	}
    //스폰
    void spawnPeople()
    {
        int people = storage.getStorage(0);
        for (int i = 0; i < people - 1; i++)
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
                go.AddComponent<SpriteRenderer>();
                go.name = "people" + i.ToString();
                pState[i] = go.GetComponent<PeopleState>();
                go.GetComponent<PeopleState>().h = this;
                //Instantiate<GameObject>(go); : 복사
            }
        }
    }

    void deletePeople()
    {
        int people = storage.getStorage(0);

        if (people < 1)
            return;
        for (int i = people - 1; i < 100; i++)
        {
            if (pState[i] == null)
                return;

            Debug.Log(pState[i].name + "사망");
            Destroy(pState[i].gameObject);
            pState[i] = null;
        }
    }

    void doEvent(DoEvent type)
	{
        string[] etype = new string[] {"탐험", "사냥", "연구"};
        AddItem item = new AddItem(0);
        if(type == DoEvent.Adventure)
		{
            item.food = Random.Range(0, 2);
            item.water = Random.Range(1, 3);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;

            item.stageExp += 4;
        }
        else if (type == DoEvent.Hunt)
		{            
            item.food = Random.Range(1, 3);
            item.water = Random.Range(0, 3);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;
        }
        else if (type == DoEvent.Research)
		{
            int labLevel = storage.getStorage(4);
            item.labExp = labLevel < 5 ? Random.Range(1,3) : Random.Range(2, 5);
            item.takeTime = 4;
        }
        Debug.Log(etype[(int)type]);

        updateEvent(item);
    }

	void doRandEvent(DoEvent type)
	{
        AddItem item = new AddItem(0);
        string[] etype = new string[] { "습격", "병" };

        if (type == DoEvent.Defense)
        {
            Debug.Log(etype[0]);
            item.food = Random.Range(0, 100) > 80 ? -Random.Range(1, 2) : 0;
            item.people = Random.Range(0, 100) > 50 ? -Random.Range(1, 2) : 0;
            item.takeTime = 8;
        }
        else if (type == DoEvent.Disease)
        {
            Debug.Log(etype[1]);
            int people = storage.getStorage(0);
            int weak = Random.Range(0, people - 1);
            item.food = -weak;
            item.water = -weak;
            item.people = Random.Range(0, 100) > 80 ? -Random.Range(0, weak) : 0;
            item.takeTime = 4;
        }
        updateEvent(item);
    }

    void updateEvent(AddItem item)
	{
        storage.addStorage(0, item.people);
        storage.addStorage(1, item.food);
        storage.addStorage(2, item.water);
        storage.addStorage(3, item.labExp);
        storage.addStorage(4, item.stageExp);

        if(item.people > 0)
        spawnPeople();
        else if(item.people < 0)
        deletePeople();

        hour+= item.takeTime;
                
        if (hour>11)//12시 땡
		{
            hour -= 12;
            day++;
            nextDay();

            //랜덤하게 일어나야하는 이벤트. 나중에 확률 조정할 것
            doRandEvent((DoEvent)(Random.Range(1, 5)));

            int people = storage.getStorage(0);
            for (int i = 0; i < people - 1; i++)
                pState[i].comeBack();
        }
        else
		{
            int people = storage.getStorage(0);
              if (people > 1)
            {
                for (int i = 0; i < people - 1; i++)
                {
                    pState[i].takeTime += item.takeTime;
                }
            }

		}
	}

    void nextDay()
	{
        Debug.Log("다음날");
        //storage.peopleTakeItem();
        int people = storage.getStorage(1);
        int food = storage.getStorage(1);
        int water = storage.getStorage(2);

        if (food - people < 0)
        {
            storage.addStorage(0, food - people);
            storage.addStorage(1, people - food);
            deletePeople();
        }
        else
            storage.addStorage(1, food - people);

        people = storage.getStorage(1);

        if (water - people < 0)
        {
            storage.addStorage(0, water - people);
            storage.addStorage(1, people - water);
            deletePeople();
        }
        else
            storage.addStorage(1, water - people);

        if (storage.getStorage(0) == 0)
        {
            gameover = true;
            Debug.Log("게임오버");
        }
    }

}
public struct AddItem
{
    public int people;
    public int food;
    public int water;
    public int takeTime;
    public int labExp;
    public int stageExp;

    public AddItem(int init)
    {
        people = init;
        food = init;
        water = init;
        takeTime = init;
        labExp = init;
        stageExp = init;
    }
}
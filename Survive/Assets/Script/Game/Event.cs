using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        pState = new PeopleState[100];
        initGame();
    }

    public AddItem getDay;
    bool gameover;
    public Storage storage;
    public int day;
    int hour;
    public PeopleState[] pState;
    public int workman;
    public int explorer;
    int[] map;

    public bool newday;

    public int specialEvent;

    void initGame()
    {
        //storage = new Storage(1, 5, 0, 1, 0, 1);
        specialEvent = 0;
        storage = new Storage(40, 15, 1, 10, 40, 10);
        getDay = new AddItem(0);//하루 지나면 초기화
        day = 0;
        hour = 0;
        workman = 0;
        explorer = 0;
        deletePeople();
        gameover = false;
        spawnPeople();
        map = new int[] {50, 100, 300 };
        newday = false;
    }

    // Update is called once per frame
    void Update()
    {
        return;

        if (newday)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                newday = false;
                getDay = new AddItem(0);//하루 지나면 초기화
            }
            return;
        }

        if (gameover)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                initGame();
                return;
            }
        if (Input.GetKeyDown(KeyCode.Space))
            doEvent((DoEvent)Math.random(0, 3));
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            hour = 12;
            storage.addStorage(0,-100);
        }
    }

    public enum DoEvent
    {
        Adventure,
        Hunt,
        Research,
        Defense,
        Disease,
        SkipDay
    }
    //스폰
    void spawnPeople()
    {
        int people = storage.getStorage(0)-1;

        if (people > 100)
            people = 100;

        for (int i = 0; i < people; i++)
        {
            if (pState[i] == null)
            {
                GameObject go = new GameObject();

                //go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("meat");
                //go.transform.Scale = new Vector2(5, 5);

                float height = Camera.main.orthographicSize;
                float width = Camera.main.aspect * height;
                float x = Math.random(-width, width);
                float y = Math.random(-height, height);
                go.transform.position = new Vector3(x, y, 0);
                go.AddComponent<PeopleState>();                
                go.name = "people" + i.ToString();
                pState[i] = go.GetComponent<PeopleState>();
                go.GetComponent<PeopleState>().h = this;
            }
        }
    }

    void deletePeople()
    {
        int people = storage.getStorage(0)-1;//플래이어는 빼야함

        if (people < 0)
            people = 0;
        for (int i = people; i < 100; i++)
        {
            if (pState[i] == null)
                return;

            Debug.Log(pState[i].name + "사망");
            Destroy(pState[i].gameObject);
            pState[i] = null;
        }
    }
    //지정해서 삭제
    void deletePeople(int idx)
    {
        Destroy(pState[idx]);
        int people = storage.getStorage(0)-1;//플래이어는 빼야함

        Debug.Log(pState[idx].name + "사망");

        for (int i = idx; i <people; i++)
        {
            pState[i] = pState[i + 1];
        }
        storage.addStorage(0, -1);
    }

    public void doEvent(DoEvent type)
    {
        string[] etype = new string[] { "탐험", "사냥", "연구" };
        AddItem item = new AddItem(0);
        if (type == DoEvent.Adventure)
        {
            item.food = Math.random(0, 2);
            item.people = Math.random(0, 100) > 50 ? Math.random(1, 2) : 0;
            item.takeTime = 4;

            item.mapExp += 4;
        }
        else if (type == DoEvent.Hunt)
        {
            item.food = Math.random(1, 3);
            item.people = Math.random(0, 100) > 50 ? Math.random(1, 2) : 0;
            item.takeTime = 4;
        }
        else if (type == DoEvent.Research)
        {
            int labLevel = storage.getStorage(4);
            item.labExp = labLevel < 5 ? Math.random(1, 3) : Math.random(2, 5);
            item.takeTime = 4;
        }
        else if(type == DoEvent.SkipDay)
		{
            skipDay();
            Debug.Log("skipday");
            return;
        }
        Debug.Log(etype[(int)type]);

        updateEvent(item);
    }

    public void doRandEvent(DoEvent type)
    {
        AddItem item = new AddItem(0);
        string[] etype = new string[] { "습격", "병" };

        if (type == DoEvent.Defense)
        {
            specialEvent = 1;
            Debug.Log(etype[0]);
            item.food = Math.random(0, 100) > 80 ? -Math.random(1, 2) : 0;
            item.people = Math.random(0, 100) > 50 ? -Math.random(1, 2) : 0;
            item.takeTime = 8;
        }
        else if (type == DoEvent.Disease)
        {
            specialEvent = 2;
            Debug.Log(etype[1]);
            int people = storage.getStorage(0);
            int weak = Math.random(0, people - 1);
            item.food = -weak;
            item.people = Math.random(0, 100) > 80 ? -Math.random(0, weak) : 0;
            item.takeTime = 4;
        }
        updateEvent(item);
    }
    void skipDay()
	{
        int people = storage.getStorage(0);
        for (int i = 0; i < people - 1; i++)
        {
            if (pState[i] != null)
                pState[i].takeTime += 12;
        }
        hour = 0;
        day++;
        nextDay();
	}

    void updateEvent(AddItem item)
    {
        storage.addStorage(0, item.people);
        storage.addStorage(1, item.food);
        storage.addStorage(3, item.labExp);
        storage.addStorage(4, item.mapExp);

        //오늘 얻은 물건 저장
        getDay.people += item.people;
        getDay.food += item.food;
        getDay.labExp += item.labExp;
        getDay.mapExp += item.mapExp;

        if (item.people > 0)
            spawnPeople();
        else if (item.people < 0)
            deletePeople();

        hour += item.takeTime;

        if (hour > 11)//12시 땡
        {
            hour -= 12;
            day++;
            nextDay();

            specialEvent = 0;//이벤트 초기화
            //랜덤하게 일어나야하는 이벤트. 나중에 확률 조정할 것
            doRandEvent((DoEvent)(Math.random(1, 5)));

            //int people = storage.getStorage(0);
            //for (int i = 0; i < people - 1; i++)
            //    if (pState[i] != null)
            //        pState[i].comeBack();
        }
        else
        {
            int people = storage.getStorage(0);
            for (int i = 0; i < people - 1; i++)
            {
                if(pState[i]!=null)
                pState[i].takeTime += item.takeTime;
            }
        }
    }

    void nextDay()
    {
        newday = true;//보고창을 닫지 않으면 게임 속행 안되게
        Debug.Log("다음날");

        int people = storage.getStorage(0);
        int food = storage.getStorage(1);

        if (food < people)
        {
            getDay.food -= food;
            getDay.people = food - people;
        }
        else
            getDay.food = -people;
        storage.peopleTakeItem();
        
        if (people != storage.getStorage(0))//자원을 사용한 후와 사람 수가 다르다
            deletePeople();

        if (storage.getStorage(0) == 0)
        {
            deletePeople();
            gameover = true;
            Debug.Log("게임오버");
        }
        
    }

}
public struct AddItem
{
    public int people;
    public int food;
    public int takeTime;
    public int labExp;
    public int mapExp;

    public AddItem(int init)
    {
        people = init;
        food = init;
        takeTime = init;
        labExp = init;
        mapExp = init;
    }
}
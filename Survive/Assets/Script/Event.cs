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

    void initGame()
    {
        //storage = new Storage(1, 5, 0, 1, 0, 1);
        storage = new Storage(40, 15, 1, 10, 0, 10);
        getDay = new AddItem(0);//�Ϸ� ������ �ʱ�ȭ
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
        if (newday)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                newday = false;
                getDay = new AddItem(0);//�Ϸ� ������ �ʱ�ȭ
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
            doEvent((DoEvent)Random.Range(0, 3));
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            hour = 12;
            storage.addStorage(0,-100);
        }
    }

    enum DoEvent
    {
        Adventure,
        Hunt,
        Research,
        Defense,
        Disease
    }
    //����
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
                float x = Random.Range(-width, width);
                float y = Random.Range(-height, height);
                go.transform.position = new Vector3(x, y, 0);
                go.AddComponent<PeopleState>();
                go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bush");
                go.name = "people" + i.ToString();
                pState[i] = go.GetComponent<PeopleState>();
                go.GetComponent<PeopleState>().h = this;
                //Instantiate<GameObject>(go); : ����

                Resources.Load<Sprite>("bush_2");
            }
        }
    }

    void deletePeople()
    {
        int people = storage.getStorage(0)-1;//�÷��̾�� ������

        if (people < 0)
            people = 0;
        for (int i = people; i < 100; i++)
        {
            if (pState[i] == null)
                return;

            Debug.Log(pState[i].name + "���");
            Destroy(pState[i].gameObject);
            pState[i] = null;
        }
    }
    //�����ؼ� ����
    void deletePeople(int idx)
    {
        Destroy(pState[idx]);
        int people = storage.getStorage(0)-1;//�÷��̾�� ������

        Debug.Log(pState[idx].name + "���");

        for (int i = idx; i <people; i++)
        {
            pState[i] = pState[i + 1];
        }
        storage.addStorage(0, -1);
    }

    void doEvent(DoEvent type)
    {
        string[] etype = new string[] { "Ž��", "���", "����" };
        AddItem item = new AddItem(0);
        if (type == DoEvent.Adventure)
        {
            item.food = Random.Range(0, 2);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;

            item.stageExp += 4;
        }
        else if (type == DoEvent.Hunt)
        {
            item.food = Random.Range(1, 3);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;
        }
        else if (type == DoEvent.Research)
        {
            int labLevel = storage.getStorage(4);
            item.labExp = labLevel < 5 ? Random.Range(1, 3) : Random.Range(2, 5);
            item.takeTime = 4;
        }
        Debug.Log(etype[(int)type]);

        updateEvent(item);
    }

    void doRandEvent(DoEvent type)
    {
        AddItem item = new AddItem(0);
        string[] etype = new string[] { "����", "��" };

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
            item.people = Random.Range(0, 100) > 80 ? -Random.Range(0, weak) : 0;
            item.takeTime = 4;
        }
        updateEvent(item);
    }

    void updateEvent(AddItem item)
    {
        storage.addStorage(0, item.people);
        storage.addStorage(1, item.food);
        storage.addStorage(3, item.labExp);
        storage.addStorage(4, item.stageExp);

        //���� ���� ���� ����
        getDay.people += item.people;
        getDay.food += item.food;
        getDay.labExp += item.labExp;
        getDay.stageExp += item.stageExp;

        if (item.people > 0)
            spawnPeople();
        else if (item.people < 0)
            deletePeople();

        hour += item.takeTime;

        if (hour > 11)//12�� ��
        {
            hour -= 12;
            day++;
            nextDay();

            //�����ϰ� �Ͼ���ϴ� �̺�Ʈ. ���߿� Ȯ�� ������ ��
            doRandEvent((DoEvent)(Random.Range(1, 5)));

            int people = storage.getStorage(0);
            for (int i = 0; i < people - 1; i++)
                if (pState[i] != null)
                    pState[i].comeBack();

            newday = true;//����â�� ���� ������ ���� ���� �ȵǰ�
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
        Debug.Log("������");

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
        
        if (people != storage.getStorage(0))//�ڿ��� ����� �Ŀ� ��� ���� �ٸ���
            deletePeople();

        if (storage.getStorage(0) == 0)
        {
            deletePeople();
            gameover = true;
            Debug.Log("���ӿ���");
        }
        
    }

}
public struct AddItem
{
    public int people;
    public int food;
    public int takeTime;
    public int labExp;
    public int stageExp;

    public AddItem(int init)
    {
        people = init;
        food = init;
        takeTime = init;
        labExp = init;
        stageExp = init;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    enum Desire
    {
        Hungry = 0, Energy, Clean,
    }

    PlayerState ps;
    Item[] item = new Item[10];
    Inventory inventory;
    public GameObject DayNight;//밤이면 켜

    int itemlength;
    int day, hour;
    bool night;//밤인가
    bool death;
    int people;
    int food;

    float delay;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        day = 0;
        death = false;
        ps = new PlayerState(3, 3, 5, 10, 10, 0, null);
        itemlength = 0;
        night = false;
        people = 1;//처음은 혼자 시작
        food = 0;
        delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool coevent = true;
        if (death)
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            delay = 0;
            coevent = false;
            nextDay();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            delay = 0;
            coevent = false;
            eventTime();
        }

        if (coevent)
        {
            delay += Time.deltaTime;
            if (delay > 60.0f)//1분동안 동작하지 않으면 강제 이벤트 실행
            {
                delay = 0;
                eventTime();
            }
        }
        // cheat key;;;;
        //
        //
        //
    }

    void UseItem()
    {
        if (Input.GetKeyDown((KeyCode.Alpha0)))
        {
            deleteItem(9);
        }
        else
            for (int i=1; i<10;i++)
            {
                if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1+i)))
                {
                    deleteItem(i-1);
                }
            }
    }

    void addDesire(int idx, int m)
    {
        if (idx == 4)//모두 올린다
            for (int i = 0; i < 3; i++)
                ps.desire[i] += m;
        else
            ps.desire[idx] += m;
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

    void eventTime()
    {
        if (!night)
        {
            night = true;
            DayNight.SetActive(night);
        }

        int type = Random.Range(0, 2);

        int takeTime = 0;

        switch (type)
        {
            case 0:
                //탐색
                Debug.Log("탐색");
                addItem(new Item("apple", 3, 1, 1, 5));
                food++;
                takeTime = 4;
                break;
            case 1:
                //사냥
                Debug.Log("사냥");
                addItem(new Item("meat", 3, 1, 2, 10));
                food++;
                takeTime = 4;

                break;
            case 2:
                //습격
                Debug.Log("습격");
                SkipDay();
                takeTime = 10;
                break;
            case 3:
                //상인
                Debug.Log("상인");
                takeTime = 2;
                break;
            default:
                Debug.Log("아무일도 없었음");
                break;
        }

        hour += takeTime;
        if (hour > 12)
        {
            hour -= 12;
            day++;
            nextDay();//시간이 끝났으니 강제 이동
        }

        //이벤트 시 전부 줄어들기
        addDesire(4, -1);
        printItem();
    }


    void SkipDay()
    {
        addDesire(4, -1);

        if (ps.desire[(int)Desire.Hungry] == 0)
            GameOver("배고파서 죽음");
        if (ps.desire[(int)Desire.Energy] == 0)
            GameOver("에너지가 없어서 죽음");
    }

    void nextDay()
    {
        addDesire(ps.desire[(int)Desire.Hungry], -1);
        addDesire(ps.desire[(int)Desire.Clean], -1);
        addDesire(ps.desire[(int)Desire.Energy], 3);//잤다

        if (ps.desire[(int)Desire.Hungry] == 0)
            GameOver("배고파서 죽음");
        if (ps.desire[(int)Desire.Energy] == 0)
            GameOver("에너지가 없어서 죽음");
        // 밥먹기
        // 사람수만큼 식량 까기 => 식량 부족이면 사람 뒤짐 => 모두 죽으면 게임오버
        //씻기

        day++;

        //printItem();

        //if (night)
        //{
        //    night = false;
        //    DayNight.SetActive(night);
        //}

        // 자산 뭐 남은지 디스페리이 desire

        // 새로운날 표시day


        // 밤일 경우, 아침처럼 보이기 
        // 
    }
    void NewDay()
    {
        for (int i = 0; i < itemlength; i++)
        {
            if (item[i].itemType == 3)
            {
                if (ps.desire[(int)Desire.Hungry] < 3)
                {
                    Debug.Log(item[i].name + "먹음");
                    addDesire((int)Desire.Hungry, item[i].effect);

                    if (item[i].count > 1)//1보다 크면 깎고
                        item[i].count--;
                    else//아니면 지운다
                        deleteItem(i);

                    if (ps.desire[(int)Desire.Hungry] > 3)//배부르면 넘기고
                    {
                        ps.desire[(int)Desire.Hungry] = 3;
                        return;
                    }//아니면 다시
                    else
                        i--;
                }
            }
        }
    }

    public void GameOver(string reason)
    {

        Debug.Log(reason);
        Debug.Log(day + "일 동안 생존");
    }

    void printItem()
    {
        for (int i = 0; i < item.Length; i++)
            inventory.invenImg("0", i, 0, 0);

        for (int i = 0; i < itemlength; i++)
        {
            inventory.invenImg(item[i].name, i, 0, 0);
            if (item[i] != null)
            {
                inventory.invenImg(item[i].name, i, item[i].count, 1);
                Debug.LogFormat($"{i}번째에 {item[i].name}가 {item[i].count}개 있음");
            }
        }
    }
}

class Event
{
    void AdventureReward()
    {

    }

    void huntReward()
    {

    }
    void attackedReward()
    {

    }

    void traderReward()
    {

    }
}

class PlayerState
{
    /// <summary>
    /// 0 : h / 1 : e / 2 : c
    /// </summary>
    public int[] desire;
    public int apk;
    public int luck;
    public int money;
    public Item[] item = new Item[10];

    public PlayerState(int h, int e, int c, int a, int l, int m, Item[] i)
    {
        desire = new int[3];
        desire[0] = h;
        desire[1] = e;
        desire[2] = c;
        luck = l;
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
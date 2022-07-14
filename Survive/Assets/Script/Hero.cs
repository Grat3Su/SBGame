using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    PlayerState ps;
    Item[] item = new Item[10];
    Inventory inventory;
    int itemlength;
    int day;
    bool newday;
    bool death;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        day = 0;
        death = false;
        newday = true;
        ps = new PlayerState(3, 3, 5, 10, 10, 0, null);
        itemlength = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (death)
            return;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            NextDay();
            newday = true;
            day++;
            printItem();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            newday = false;
            EventTime();
            printItem();
        }
    }    

    void addItem(Item i)
    {
        itemlength++;
        if (itemlength > 10)//초과는 안됨
            itemlength = 10;

        for (int j = 0; j< itemlength; j++)
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
        for(int i = idx; i < itemlength; i++)
        {
            item[i] = item[i + 1];
        }
    }

    void EventTime()//하루 지나야됨
    {
        int type = Random.Range(0, 2);

        switch (type)
        {
            case 0:
                //탐색
                Debug.Log("탐색");
                addItem(new Item("사과", 3, 1, 1, 5));
                break;
            case 1:
                //사냥
                Debug.Log("사냥");
                addItem(new Item("고기", 3, 1, 2, 10));
                
                break;
            case 2:
                //습격
                Debug.Log("습격");
                SkipDay();
                break;
            case 3:
                //상인
                Debug.Log("상인");
                break;
            default:
                Debug.Log("아무일도 없었음");
                break;
        }
    }

    void printItem()
    {
        for(int i = 0; i<item.Length; i++)
            inventory.invenImg(i, 0, 0);

        for (int i = 0; i < itemlength; i++)
        {
            inventory.invenImg(i, 0, 0);
            if (item[i] != null)
            {
                inventory.invenImg(i, item[i].count, 1);
                Debug.LogFormat($"{i}번째에 {item[i].name}가 {item[i].count}개 있음");
            }
        }
        

    }

    void SkipDay()
    {
        ps.hunger--;
        ps.energy--;
        ps.clean--;
        if (ps.hunger == 0)
            GameOver("배고파서 죽음");
        if (ps.energy == 0)
            GameOver("에너지가 없어서 죽음");
    }

    void NextDay()
    {
        ps.NextDay();
        NewDay();

        if (ps.hunger == 0)
            GameOver("배고파서 죽음");

        //밥먹기
        //씻기
    }

    void NewDay()
    {
        for(int i = 0; i< itemlength; i++)
        {
            if (item[i].itemType == 3)
            {
                if (ps.hunger < 3)
                {
                    Debug.Log(item[i].name + "먹음");
                    ps.hunger += item[i].effect;
                    if (item[i].count > 1)//1보다 크면 깎고
                        item[i].count--;
                    else//아니면 지운다
                        deleteItem(i);

                    if (ps.hunger > 3)//배부르면 넘기고
                    {
                        ps.hunger = 3;
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
    public int hunger;// 안먹고 3일 버티기
    public int energy;// 안 자고 3일 버티기
    public int clean;// 안 씻고 5일 버티기
    public int apk;
    public int luck;
    public int money;
    public Item[] item = new Item[10];

    public PlayerState(int h, int e, int c, int a, int l, int m, Item[] i)
    {
        hunger = h;
        energy = e;
        clean = c;
        luck = l;
        item = i;
        money = m;
    }
    public void NextDay()
    {
        energy = 3;
        hunger -= 1;
        clean -= 1;

        if (clean < 0)
            clean = 0;
    }
}
class Item
{
    public string name;//이름
    public int itemType;//0:기타 1:장비 2:소모품 3: 음식
    public int effect;//효과
    public int money;//판매할 때 얼마를 받나
    public int count;
    public Item(string n,int t,int c, int e, int m)
    {
        name = n;
        itemType = t;
        count = c;
        effect = e;
        money = m;
    }
}
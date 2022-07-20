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
    Inventory inventory;
    public GameObject DayNight;//밤이면 켜

    int itemlength;
    int day, hour;
    bool death;
    int people;//생존자. 이벤트 시 랜덤하게 획득
    int food;//하루마다 사람수만큼 차감
    int water;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        day = 0;
        death = false;
        ps = new PlayerState(3, 3, 5, 10, 0, null);
        itemlength = 0;
        people = 1;//처음은 혼자 시작
        water = 0;
        food = 0;
        load();
    }

    void resetGame()
    {
        Debug.Log("Reset");
        day = 0;
        death = false;
        people = 1;//처음은 혼자 시작
        water = 0;
        food = 0;
        hour = 0;

        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (death)
        {
            if (Input.GetKeyDown(KeyCode.Space))//게임 새로 시작
                resetGame();
            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            hour = 0;
            nextDay();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
            randomevent();
        else if (Input.GetKeyDown(KeyCode.RightArrow))//탐색
            eventTime(0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))//사냥
            eventTime(1);

        else if (Input.GetKeyDown(KeyCode.S))//입출력
            save();
        else if (Input.GetKeyDown(KeyCode.L))
            load();

        // cheat key;;;;
        //
        //
        //
    }

    void save()
    {
        gd.day = day;
        gd.food = food;
        gd.people = people;
        gd.water = water;

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
            food = gd.food;
            people = gd.people;
            water = gd.water;

        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
    }

    void eventTime(int type)
    {
        int attack = Random.Range(0, 100);

        int takeTime = 0;
        int p = people / 2;
        if (attack > 80)
        {
            Debug.Log("습격");

            int r = Random.Range(0, p - 1);
            people -= r;

            Debug.Log("습격으로 인해 " + r + "명 사망");

            if (people < 0)
            {
                people = 0;
                Debug.Log("남은 생존자가 없다");
                Debug.Log(day + "일 동안 생존");
                death = true;
            }
            takeTime = 8;
        }
        else
        {
            if(type == 0)
            {
                //탐색
                Debug.Log("탐색");
                //addItem(new Item("food", 3, 2, 1, 5));//2개 획득. test
                water += (3 + Random.Range(0, p));//인구 수 보너스
                food += (1 + Random.Range(0, p));
                addPeople(3);
                takeTime = 4;
            }
            else if(type == 1)
            {
                //사냥
                Debug.Log("사냥");
                //addItem(new Item("food", 3, 3, 2, 10));//3개 획득
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//인구 수 보너스
                takeTime = 4;
            }
            else if(type == 3)
            {
                //상인
                //     Debug.Log("상인");
                //     takeTime = 2;                
            }
            else
            {
                Debug.Log("아무일도 없었음");
            }
        }

        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
        hour += takeTime;
        if (hour > 12)
        {
            hour -= 12;
            nextDay();//시간이 끝났으니 강제 이동
        }
        Debug.Log("현재 시간 : "+hour + "시");

        //printItem();
    }

    void randomevent()
    {
        int type = 0;
        if (type == 4)
            type = Random.Range(0, 2);

        int takeTime = 0;
        int p = people / 2;

        switch (type)
        {
            case 0:
                //탐색
                Debug.Log("탐색");
                //addItem(new Item("food", 3, 2, 1, 5));//2개 획득. test
                water += (3 + Random.Range(0, p));//인구 수 보너스
                food += (1 + Random.Range(0, p));
                addPeople(3);
                takeTime = 4;
                break;
            case 1:
                //사냥                Debug.Log("사냥");
                //addItem(new Item("food", 3, 3, 2, 10));//3개 획득
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//인구 수 보너스
                takeTime = 4;
                break;
            case 2:
                //습격
                Debug.Log("습격");

                int r = Random.Range(0, p - 1);
                people -= r;

                Debug.Log("습격으로 인해 "+ r + "명 사망");

                if (people < 0)
                {
                    people = 0;
                    Debug.Log("남은 생존자가 없다");
                    Debug.Log(day + "일 동안 생존");
                    death = true;
                }
                takeTime = 8;
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
            nextDay();//시간이 끝났으니 강제 이동
        }
        Debug.Log("현재 시간 : " + hour + "시");
        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
    }

    void addPeople(int max)
    {
        int r = Random.Range(0, max);
        if (r == 0)
            return;
        people += r;
        Debug.Log(r +"명 구출");
        Debug.Log("현재 인구 수 "+people + "명");
    }

    void nextDay()
    {
        //식량 소비        
        if(food <people)
        {
            Debug.Log(people - food + "명 사망");
            people -= (people - food);//식량이 부족한 수만큼 줄기
            food = 0;//다먹음
        }
        else
            food -= people;//사람 수만큼 줄이기

        if (water < people)
        {
            Debug.Log(people - water+"명 사망");
            people -= (people - water);//물이 부족한 수만큼 줄기
            water = 0;//다먹음
        }
        else
            water -= people;//사람 수만큼 줄이기

        if(people <0)
        {
            people = 0;
            Debug.Log("남은 생존자가 없다");
            Debug.Log(day + "일 동안 생존");
            death = true;
        }

        // 사람수만큼 식량 까기 => 식량 부족이면 인구 줄어들기 => 모두 죽으면 게임오버
        //씻기

        day++;

        //printItem();

        // 자산 뭐 남은지 디스플레이


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


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
    public int people, _people;//생존자. 이벤트 시 랜덤하게 획득
    public int food, _food;//하루마다 사람수만큼 차감
    public int water, _water;
    public int labExp, labLevel;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        ps = new PlayerState(3, 3, 5, 10, 0, null);
        itemlength = 0;

        resetGame();
        load();
    }

    void resetGame()
    {
        day = 0;
        death = false;
        people = 1;//처음은 혼자 시작
        water = 10;
        food = 10;
        hour = 0;

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
        else if( Input.GetKeyDown(KeyCode.W) )
        {//water--
            water -= 5;
        }
        else if( Input.GetKeyDown(KeyCode.F) )
        {//food--
            food -= 5;
        }

        if (death)
        {
            if (Input.GetKeyDown(KeyCode.Space))//게임 새로 시작
                resetGame();
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // cheat key;;;;
        //
        //
        //
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


        //             addPeople(3);
        //            Debug.Log("탐색");
        //            Debug.Log("사냥");

        //people += result.people;
        //if( people < 1 )
        //{
        //    people = 0;
        //    Debug.Log("남은 생존자가 없다");
        //    Debug.Log(day + "일 동안 생존");
        //    death = true;
        //}

        //Debug.Log("남은 생존자가 없다");
        //Debug.Log(day + "일 동안 생존");
        //Debug.Log("습격으로 인해 " + r + "명 사망");

        //labExp += result.labExp;
        //if (labExp > 4)
        //    labLevel++;
        //lablevel++;
        //// 최대값 증가(인,식,물)
        //if (lablevel == 0)
        //    max += 50;
        //// 무기(습격 시간 감소)
        //else if (lablevel == 1)
        //    atime = 6;
        //// 방어구(습격 사망자 감소)
        //else if (lablevel == 2) ;
        //
        //// 도구(식량/물 자체 생산)
        //else if (lablevel == 2)
        //{
        //
        //}

        //mount[0].text = people.ToString();
        //mount[1].text = food.ToString();
        //mount[2].text = water.ToString();

        
        //Debug.Log("현재 시간 : " + hour + "시");

        //printItem();

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
        gd._food= _food;
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

        day     = gd.day;
        hour    = gd.hour;
        food    = gd.food;
        _food   = gd._food;//하
        water   = gd.water;
        _water  = gd._water;
        people  = gd.people;
        _people = gd._people;
        labExp  = gd.labExp;
        labLevel= gd.labLevel;
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
            result.people = 3;
            result.water = (3 + Random.Range(0, p));//인구 수 보너스
            result.food = (Random.Range(0, p));
            result.takeTime = 4;
        }
        else if (eventType == 1)
        {
            //사냥
            result.people = 2;
            result.food = (3 + Random.Range(0, p));
            result.water = (Random.Range(0, p));//인구 수 보너스
            result.takeTime = 4;
        }
        else if (eventType == 2)
        {   // 습격
            result.people = -Random.Range(0, p);
            result.takeTime = 8;
            if (labLevel >1)
            result.takeTime = 6;
        }
        else if (eventType == 3)
        {
            //상인
            //     Debug.Log("상인");
            //result.takeTime = 2;                
        }
        else if (eventType == 4)
        {
            //연구
            result.labExp = Random.Range(1,3);// random
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
        if(labLevel == 0)
        {
            //연구소 증축
        }
        // 최대값 증가(인,식,물)
        if (labLevel == 1)
        {
            _people += 10;
            _food += 15;
            _water += 15;
        }
        // 무기(습격 시간 감소)
        else if (labLevel == 2)
        {

        }        
        // 최댓값 증가 물자는 주지 말자.
        else if (labLevel == 4)
        {
            //
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

        string[] str = new string[] { "탐색", "사냥", "습격", "상인", "연구" };
        el.add("[" + str[result.type] + "] 했습니다.");

        if( result.people>0 )
            el.add(result.people+"명을 구했습니다.");
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

        water += result.water;
        if (water > _water)
            water = _water;

        labExp += result.labExp;
        while (labExp > 4)
        {
            lab();
            labExp -= 4;
            labLevel++;
        }

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


    void addPeople(int max)
    {
        int r = Random.Range(0, max);
        if (r == 0)
            return;
        people += r;
        Debug.Log(r + "명 구출");
        Debug.Log("현재 인구 수 " + people + "명");
    }

    void nextDay()
    {
        //식량 소비        
        if (food < people)
        {
            Debug.Log(people - food + "명 사망");
            people -= (people - food);//식량이 부족한 수만큼 줄기
            food = 0;//다먹음
        }
        else
            food -= people;//사람 수만큼 줄이기

        if (water < people)
        {
            Debug.Log(people - water + "명 사망");
            people -= (people - water);//물이 부족한 수만큼 줄기
            water = 0;//다먹음
        }
        else
            water -= people;//사람 수만큼 줄이기

        if (people <= 0)
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


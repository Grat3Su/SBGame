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
    public GameObject DayNight;//���̸� ��

    int itemlength;
    int day, hour;
    bool death;
    public int people, _people;//������. �̺�Ʈ �� �����ϰ� ȹ��
    public int food, _food;//�Ϸ縶�� �������ŭ ����
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
        people = 1;//ó���� ȥ�� ����
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
        else if (Input.GetKeyDown(KeyCode.S))//�����
            save();
        else if (Input.GetKeyDown(KeyCode.Escape))//����
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
            if (Input.GetKeyDown(KeyCode.Space))//���� ���� ����
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
        if (Input.GetKeyDown(KeyCode.RightArrow))//Ž��
            displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 0));
        else if (Input.GetKeyDown(KeyCode.LeftArrow))//���
            displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 1));
        else if (Input.GetKeyDown(KeyCode.UpArrow))//����
            displayReal(doEvent(Random.Range(0, 100) < 20 ? 2 : 4));
        else if (Input.GetKeyDown(KeyCode.Space))
            displayReal(doEvent(Random.Range(0, 40) / 10));


        //             addPeople(3);
        //            Debug.Log("Ž��");
        //            Debug.Log("���");

        //people += result.people;
        //if( people < 1 )
        //{
        //    people = 0;
        //    Debug.Log("���� �����ڰ� ����");
        //    Debug.Log(day + "�� ���� ����");
        //    death = true;
        //}

        //Debug.Log("���� �����ڰ� ����");
        //Debug.Log(day + "�� ���� ����");
        //Debug.Log("�������� ���� " + r + "�� ���");

        //labExp += result.labExp;
        //if (labExp > 4)
        //    labLevel++;
        //lablevel++;
        //// �ִ밪 ����(��,��,��)
        //if (lablevel == 0)
        //    max += 50;
        //// ����(���� �ð� ����)
        //else if (lablevel == 1)
        //    atime = 6;
        //// ��(���� ����� ����)
        //else if (lablevel == 2) ;
        //
        //// ����(�ķ�/�� ��ü ����)
        //else if (lablevel == 2)
        //{
        //
        //}

        //mount[0].text = people.ToString();
        //mount[1].text = food.ToString();
        //mount[2].text = water.ToString();

        
        //Debug.Log("���� �ð� : " + hour + "��");

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

        day     = gd.day;
        hour    = gd.hour;
        food    = gd.food;
        _food   = gd._food;//��
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
            //Ž��
            result.people = 3;
            result.water = (3 + Random.Range(0, p));//�α� �� ���ʽ�
            result.food = (Random.Range(0, p));
            result.takeTime = 4;
        }
        else if (eventType == 1)
        {
            //���
            result.people = 2;
            result.food = (3 + Random.Range(0, p));
            result.water = (Random.Range(0, p));//�α� �� ���ʽ�
            result.takeTime = 4;
        }
        else if (eventType == 2)
        {   // ����
            result.people = -Random.Range(0, p);
            result.takeTime = 8;
            if (labLevel >1)
            result.takeTime = 6;
        }
        else if (eventType == 3)
        {
            //����
            //     Debug.Log("����");
            //result.takeTime = 2;                
        }
        else if (eventType == 4)
        {
            //����
            result.labExp = Random.Range(1,3);// random
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
        if(labLevel == 0)
        {
            //������ ����
        }
        // �ִ밪 ����(��,��,��)
        if (labLevel == 1)
        {
            _people += 10;
            _food += 15;
            _water += 15;
        }
        // ����(���� �ð� ����)
        else if (labLevel == 2)
        {

        }        
        // �ִ� ���� ���ڴ� ���� ����.
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

        string[] str = new string[] { "Ž��", "���", "����", "����", "����" };
        el.add("[" + str[result.type] + "] �߽��ϴ�.");

        if( result.people>0 )
            el.add(result.people+"���� ���߽��ϴ�.");
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
        Debug.Log(r + "�� ����");
        Debug.Log("���� �α� �� " + people + "��");
    }

    void nextDay()
    {
        //�ķ� �Һ�        
        if (food < people)
        {
            Debug.Log(people - food + "�� ���");
            people -= (people - food);//�ķ��� ������ ����ŭ �ٱ�
            food = 0;//�ٸ���
        }
        else
            food -= people;//��� ����ŭ ���̱�

        if (water < people)
        {
            Debug.Log(people - water + "�� ���");
            people -= (people - water);//���� ������ ����ŭ �ٱ�
            water = 0;//�ٸ���
        }
        else
            water -= people;//��� ����ŭ ���̱�

        if (people <= 0)
        {
            people = 0;
            Debug.Log("���� �����ڰ� ����");
            Debug.Log(day + "�� ���� ����");
            death = true;
        }

        // �������ŭ �ķ� ��� => �ķ� �����̸� �α� �پ��� => ��� ������ ���ӿ���
        //�ı�

        day++;

        //printItem();

        // �ڻ� �� ������ ���÷���


        // ���ο ǥ��day
        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
        Debug.LogFormat($"{day}���� ���� {food}��, �� {water}��, �α� {people}��");//���� �� �ڿ� �α� ��
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
        if (itemlength > 10)//�ʰ��� �ȵ�
            itemlength = 10;

        for (int j = 0; j < itemlength; j++)
        {
            if (item[j] == null)
            {
                item[j] = i;
            }
            else if (i.name == item[j].name)//�̸��� ������ ������ �ø���
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
                Debug.LogFormat($"{i}��°�� {item[i].name}�� {item[i].count}�� ����");
            }
        }
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


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
    int people;//������. �̺�Ʈ �� �����ϰ� ȹ��
    int food;//�Ϸ縶�� �������ŭ ����
    int water;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        day = 0;
        death = false;
        ps = new PlayerState(3, 3, 5, 10, 0, null);
        itemlength = 0;
        people = 1;//ó���� ȥ�� ����
        water = 0;
        food = 0;
        load();
    }

    void resetGame()
    {
        Debug.Log("Reset");
        day = 0;
        death = false;
        people = 1;//ó���� ȥ�� ����
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
            if (Input.GetKeyDown(KeyCode.Space))//���� ���� ����
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
        else if (Input.GetKeyDown(KeyCode.RightArrow))//Ž��
            eventTime(0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))//���
            eventTime(1);

        else if (Input.GetKeyDown(KeyCode.S))//�����
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
            Debug.Log("����");

            int r = Random.Range(0, p - 1);
            people -= r;

            Debug.Log("�������� ���� " + r + "�� ���");

            if (people < 0)
            {
                people = 0;
                Debug.Log("���� �����ڰ� ����");
                Debug.Log(day + "�� ���� ����");
                death = true;
            }
            takeTime = 8;
        }
        else
        {
            if(type == 0)
            {
                //Ž��
                Debug.Log("Ž��");
                //addItem(new Item("food", 3, 2, 1, 5));//2�� ȹ��. test
                water += (3 + Random.Range(0, p));//�α� �� ���ʽ�
                food += (1 + Random.Range(0, p));
                addPeople(3);
                takeTime = 4;
            }
            else if(type == 1)
            {
                //���
                Debug.Log("���");
                //addItem(new Item("food", 3, 3, 2, 10));//3�� ȹ��
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//�α� �� ���ʽ�
                takeTime = 4;
            }
            else if(type == 3)
            {
                //����
                //     Debug.Log("����");
                //     takeTime = 2;                
            }
            else
            {
                Debug.Log("�ƹ��ϵ� ������");
            }
        }

        mount[0].text = people.ToString();
        mount[1].text = food.ToString();
        mount[2].text = water.ToString();
        hour += takeTime;
        if (hour > 12)
        {
            hour -= 12;
            nextDay();//�ð��� �������� ���� �̵�
        }
        Debug.Log("���� �ð� : "+hour + "��");

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
                //Ž��
                Debug.Log("Ž��");
                //addItem(new Item("food", 3, 2, 1, 5));//2�� ȹ��. test
                water += (3 + Random.Range(0, p));//�α� �� ���ʽ�
                food += (1 + Random.Range(0, p));
                addPeople(3);
                takeTime = 4;
                break;
            case 1:
                //���                Debug.Log("���");
                //addItem(new Item("food", 3, 3, 2, 10));//3�� ȹ��
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//�α� �� ���ʽ�
                takeTime = 4;
                break;
            case 2:
                //����
                Debug.Log("����");

                int r = Random.Range(0, p - 1);
                people -= r;

                Debug.Log("�������� ���� "+ r + "�� ���");

                if (people < 0)
                {
                    people = 0;
                    Debug.Log("���� �����ڰ� ����");
                    Debug.Log(day + "�� ���� ����");
                    death = true;
                }
                takeTime = 8;
                break;
            case 3:
                //����
                Debug.Log("����");
                takeTime = 2;
                break;
            default:
                Debug.Log("�ƹ��ϵ� ������");
                break;
        }

        hour += takeTime;
        if (hour > 12)
        {
            hour -= 12;
            nextDay();//�ð��� �������� ���� �̵�
        }
        Debug.Log("���� �ð� : " + hour + "��");
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
        Debug.Log(r +"�� ����");
        Debug.Log("���� �α� �� "+people + "��");
    }

    void nextDay()
    {
        //�ķ� �Һ�        
        if(food <people)
        {
            Debug.Log(people - food + "�� ���");
            people -= (people - food);//�ķ��� ������ ����ŭ �ٱ�
            food = 0;//�ٸ���
        }
        else
            food -= people;//��� ����ŭ ���̱�

        if (water < people)
        {
            Debug.Log(people - water+"�� ���");
            people -= (people - water);//���� ������ ����ŭ �ٱ�
            water = 0;//�ٸ���
        }
        else
            water -= people;//��� ����ŭ ���̱�

        if(people <0)
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


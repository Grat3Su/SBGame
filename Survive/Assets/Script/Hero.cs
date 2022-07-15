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
    public GameObject DayNight;//���̸� ��

    int itemlength;
    int day, hour;
    bool night;//���ΰ�
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
        people = 1;//ó���� ȥ�� ����
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
            if (delay > 60.0f)//1�е��� �������� ������ ���� �̺�Ʈ ����
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
        if (idx == 4)//��� �ø���
            for (int i = 0; i < 3; i++)
                ps.desire[i] += m;
        else
            ps.desire[idx] += m;
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
                //Ž��
                Debug.Log("Ž��");
                addItem(new Item("apple", 3, 1, 1, 5));
                food++;
                takeTime = 4;
                break;
            case 1:
                //���
                Debug.Log("���");
                addItem(new Item("meat", 3, 1, 2, 10));
                food++;
                takeTime = 4;

                break;
            case 2:
                //����
                Debug.Log("����");
                SkipDay();
                takeTime = 10;
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
            day++;
            nextDay();//�ð��� �������� ���� �̵�
        }

        //�̺�Ʈ �� ���� �پ���
        addDesire(4, -1);
        printItem();
    }


    void SkipDay()
    {
        addDesire(4, -1);

        if (ps.desire[(int)Desire.Hungry] == 0)
            GameOver("����ļ� ����");
        if (ps.desire[(int)Desire.Energy] == 0)
            GameOver("�������� ��� ����");
    }

    void nextDay()
    {
        addDesire(ps.desire[(int)Desire.Hungry], -1);
        addDesire(ps.desire[(int)Desire.Clean], -1);
        addDesire(ps.desire[(int)Desire.Energy], 3);//���

        if (ps.desire[(int)Desire.Hungry] == 0)
            GameOver("����ļ� ����");
        if (ps.desire[(int)Desire.Energy] == 0)
            GameOver("�������� ��� ����");
        // ��Ա�
        // �������ŭ �ķ� ��� => �ķ� �����̸� ��� ���� => ��� ������ ���ӿ���
        //�ı�

        day++;

        //printItem();

        //if (night)
        //{
        //    night = false;
        //    DayNight.SetActive(night);
        //}

        // �ڻ� �� ������ ���丮�� desire

        // ���ο ǥ��day


        // ���� ���, ��ħó�� ���̱� 
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
                    Debug.Log(item[i].name + "����");
                    addDesire((int)Desire.Hungry, item[i].effect);

                    if (item[i].count > 1)//1���� ũ�� ���
                        item[i].count--;
                    else//�ƴϸ� �����
                        deleteItem(i);

                    if (ps.desire[(int)Desire.Hungry] > 3)//��θ��� �ѱ��
                    {
                        ps.desire[(int)Desire.Hungry] = 3;
                        return;
                    }//�ƴϸ� �ٽ�
                    else
                        i--;
                }
            }
        }
    }

    public void GameOver(string reason)
    {

        Debug.Log(reason);
        Debug.Log(day + "�� ���� ����");
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
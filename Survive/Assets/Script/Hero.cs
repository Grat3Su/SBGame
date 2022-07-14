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
        if (itemlength > 10)//�ʰ��� �ȵ�
            itemlength = 10;

        for (int j = 0; j< itemlength; j++)
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
        for(int i = idx; i < itemlength; i++)
        {
            item[i] = item[i + 1];
        }
    }

    void EventTime()//�Ϸ� �����ߵ�
    {
        int type = Random.Range(0, 2);

        switch (type)
        {
            case 0:
                //Ž��
                Debug.Log("Ž��");
                addItem(new Item("���", 3, 1, 1, 5));
                break;
            case 1:
                //���
                Debug.Log("���");
                addItem(new Item("���", 3, 1, 2, 10));
                
                break;
            case 2:
                //����
                Debug.Log("����");
                SkipDay();
                break;
            case 3:
                //����
                Debug.Log("����");
                break;
            default:
                Debug.Log("�ƹ��ϵ� ������");
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
                Debug.LogFormat($"{i}��°�� {item[i].name}�� {item[i].count}�� ����");
            }
        }
        

    }

    void SkipDay()
    {
        ps.hunger--;
        ps.energy--;
        ps.clean--;
        if (ps.hunger == 0)
            GameOver("����ļ� ����");
        if (ps.energy == 0)
            GameOver("�������� ��� ����");
    }

    void NextDay()
    {
        ps.NextDay();
        NewDay();

        if (ps.hunger == 0)
            GameOver("����ļ� ����");

        //��Ա�
        //�ı�
    }

    void NewDay()
    {
        for(int i = 0; i< itemlength; i++)
        {
            if (item[i].itemType == 3)
            {
                if (ps.hunger < 3)
                {
                    Debug.Log(item[i].name + "����");
                    ps.hunger += item[i].effect;
                    if (item[i].count > 1)//1���� ũ�� ���
                        item[i].count--;
                    else//�ƴϸ� �����
                        deleteItem(i);

                    if (ps.hunger > 3)//��θ��� �ѱ��
                    {
                        ps.hunger = 3;
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
    public int hunger;// �ȸ԰� 3�� ��Ƽ��
    public int energy;// �� �ڰ� 3�� ��Ƽ��
    public int clean;// �� �İ� 5�� ��Ƽ��
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
    public string name;//�̸�
    public int itemType;//0:��Ÿ 1:��� 2:�Ҹ�ǰ 3: ����
    public int effect;//ȿ��
    public int money;//�Ǹ��� �� �󸶸� �޳�
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
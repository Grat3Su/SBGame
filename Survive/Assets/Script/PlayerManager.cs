using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    int itemlength;
    int day, hour;
    bool death;
    int people;//������. �̺�Ʈ �� �����ϰ� ȹ��
    int food;//�Ϸ縶�� �������ŭ ����
    int water;

    // Start is called before the first frame update
    void Start()
    {
        day = 0;
        death = false;      
        itemlength = 0;
        people = 1;//ó���� ȥ�� ����
        water = 0;
        food = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void eventTime(int type)
    {
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
                //���
                Debug.Log("���");
                //addItem(new Item("food", 3, 3, 2, 10));//3�� ȹ��
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//�α� �� ���ʽ�
                takeTime = 4;
                break;
            case 2:
                //����
                Debug.Log("����");
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
            nextDay();//�ð��� �������� ���� �̵�
        }
        Debug.Log("���� �ð� : " + hour + "��");

        //printItem();
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
            food -= people;//��� ����ŭ ���̱�
            people -= (people - food);//�ķ��� ������ ����ŭ �ٱ�
            food = 0;//�ٸ���
        }
        else
            food -= people;//��� ����ŭ ���̱�

        if (water < people)
        {
            people -= (people - water);//�ķ��� ������ ����ŭ �ٱ�
            water = 0;//�ٸ���
        }
        else
            water -= people;//��� ����ŭ ���̱�

        if (people < 0)
        {
            people = 0;
            GameOver("���� ����� ����");
            death = true;
        }

        // �������ŭ �ķ� ��� => �ķ� �����̸� �α� �پ��� => ��� ������ ���ӿ���
        //�ı�

        day++;

        //printItem();

        // �ڻ� �� ������ ���丮��


        // ���ο ǥ��day
        Debug.LogFormat($"{day}���� ���� {food}��, �� {water}��, �α� {people}��");//���� �� �ڿ� �α� ��
    }

    public void GameOver(string reason)
    {

        Debug.Log(reason);
        Debug.Log(day + "�� ���� ����");
    }


}


class Display
{
    public static int[] day(int sec)
    {
        int[] m = new int[4];
        m[0] = sec % 60;
        m[1] = (sec / 60) % 60;
        m[2] = ((sec / 60) / 60) % 24;
        m[3] = ((sec / 60) / 60) / 24;
        return m;
    }

    public static string elecTime(int sec)
    {
        return "00:00";
    }

    public static string hanTime(int sec)
    {
        return "0�ð� 0 ��";
    }
}
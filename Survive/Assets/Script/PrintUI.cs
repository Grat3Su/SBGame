using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintUI : MonoBehaviour
{//�������Ұ� �����ϰ� �κ� �κ� �����ؾ��� �� �����ϱ�.


    //�ڿ�/�ִ밪
    //�ð�
    //������ ǥ��
    //ȭ�� ��ȯ(��¥ ���� �� �Ϸ� ����)
    Hero h;
    int people, _people;
    int food, _food;
    int water, _water;
    int day;
    
    void Start()
    {
        h = GameObject.Find("Hero").GetComponent<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setMax(int p, int f, int w)
    {
        //�ִ�, �ٲ� ���� �ҷ��´�. (Lab �湮 - �ִ� ������ ��)
        _people = p;
        _food = f;
        _water = w;
    }
    public void printNextDay()//Hero���� �Ϸ� ������ �� �θ���
	{
        //���� ��
        people = h.people;
        food = h.food;
        water = h.water;
        day = h.day;

        string pDay = day + "����";
        string report = "���� ��� : " + people + "��"
                        + "���� ���� : " + food + "��"
                        + "���� �� : " + water + "��";
        
    }
}

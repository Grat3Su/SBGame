using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLog
{
    public List<string> str;

    public EventLog()
    {
        str = new List<string>();
    }

    public void add(string s)
    {
        str.Add(s);
    }
    public void deleteStr()
	{
        str.Clear();
	}

    public void display()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
        for (int i=0; i<str.Count; i++)
            GUI.Label(new Rect(0, 20 * i, 150, 50), str[i]);
    }

    public void addToday(int personDie, int personAlive, int food, int water)
    {
        str.Add("#####################");
        str.Add("�Ϸ簡 �������ϴ�.");
        if(personDie > 0)
        str.Add(+personDie+"�� ���");
        str.Add("���� ��� : "+personAlive +"��");
        str.Add("���� ���� : " + food + "��");
        str.Add("���� �� : "+water+"��");
        str.Add("#####################");
    }
    public void addAtt(int personDie, int personAlive, int food, int water)
    {
        str.Add("#####################");
        str.Add("�Ϸ簡 �������ϴ�.");
        str.Add("��� " + personDie + "��� ");
    }
    public void addDef(int personDie, int personAlive, int food, int water)
    {
        str.Add("#####################");
        str.Add("�Ϸ簡 �������ϴ�.");
        str.Add("��� " + personDie + "��� ");
    }
}

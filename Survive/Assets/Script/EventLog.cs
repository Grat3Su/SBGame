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
        str.Add("하루가 지났습니다.");
        if(personDie > 0)
        str.Add(+personDie+"명 사망");
        str.Add("남은 사람 : "+personAlive +"명");
        str.Add("남은 음식 : " + food + "개");
        str.Add("남은 물 : "+water+"개");
        str.Add("#####################");
    }
    public void addAtt(int personDie, int personAlive, int food, int water)
    {
        str.Add("#####################");
        str.Add("하루가 지났습니다.");
        str.Add("사람 " + personDie + "사망 ");
    }
    public void addDef(int personDie, int personAlive, int food, int water)
    {
        str.Add("#####################");
        str.Add("하루가 지났습니다.");
        str.Add("사람 " + personDie + "사망 ");
    }
}

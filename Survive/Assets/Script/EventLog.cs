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

    public void display()
    {
        GUI.Box(new Rect(10, 10, 100, 90), "log");
        for (int i=0; i<str.Count; i++)
            GUI.Label(new Rect(0, 30 * i, 0, 0), str[i]);
    }

    public void addToday(int personDie, int personAlive, int food, int water)
    {
        str.Add("############################");
        str.Add("하루가 지났습니다.");
        str.Add("사람 "+personDie+"사망 ");
    }
    public void addAtt(int personDie, int personAlive, int food, int water)
    {
        str.Add("############################");
        str.Add("하루가 지났습니다.");
        str.Add("사람 " + personDie + "사망 ");
    }
    public void addDef(int personDie, int personAlive, int food, int water)
    {
        str.Add("############################");
        str.Add("하루가 지났습니다.");
        str.Add("사람 " + personDie + "사망 ");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Hero h;
    EventLog el;
    public List<string> str;
    // Start is called before the first frame update
    void Start()
    {
        h = GameObject.Find("Hero").GetComponent<Hero>();
        el = h.el;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {//이거 다시 보기
        
        GUI.Label(new Rect(10, 10, 150, 50), h.people.ToString());
        GUI.Label(new Rect(0, 0, 150, 50), h.food.ToString());
        GUI.Label(new Rect(0, 0, 150, 50), h.water.ToString());

        if (el == null)
            return;
        str = el.str;
        GUI.Box(new Rect(10, 10, 100, 90), "log");
        for (int i = 0; i < str.Count; i++)
            GUI.Label(new Rect(0, 30 * i, 0, 0), str[i]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Hero h;
    EventLog el;
    public List<string> str;

    public Vector2 scrollPosition = Vector2.zero;

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
        GUIStyle style = new GUIStyle();
        GUI.contentColor = Color.white;
        style.fontSize = 16;        
        //GUI.Label(new Rect(10, 10, 100, 150), "Hello World!", style);

        el = h.el;
        if (el == null)
            return;

        //scrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 250, 10, 250, 150)/*위치 / 크기*/, scrollPosition, new Rect(0, 0, 300, 300));
        scrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 260, 10, 260, Screen.height - 20)/*위치 / 크기*/, scrollPosition, new Rect(0, 0, 255, 10 +20 * str.Count));
        str = el.str;
        for (int i = 0; i < str.Count; i++)
        {
            GUI.Label(new Rect(0, 20 * i, 100, 100), str[i], style);
        }

        //GUI.BeginScrollView()
        GUI.EndScrollView();
    }    
}

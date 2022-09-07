using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static int devWidth = 1280, devHeight = 720;

    GameManager h;
    EventLog el;
    public List<string> str;

    void Start()
    {
#if false
        //h = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject go = GameObject.Find("GameManager");
        h = go.GetComponent<GameManager>();
#else
       // GameObject go = new GameObject("GameManager");
       // h =  go.AddComponent<GameManager>();
       // go.AddComponent<PrintUI>();
#endif
        //el = h.el;

    }

    // Update is called once per frame
    void Update()
    {
        iGUI.setResolution(devWidth, devHeight);                
    }

    public static iPoint mousePosition()
    {
        int sw = Screen.width, sh = Screen.height;
        float vx = Camera.main.rect.x * sw;
        float vy = Camera.main.rect.y * sh;
        float vw = Camera.main.rect.width * sw;
        float vh = Camera.main.rect.height * sh;

        Vector3 v = Input.mousePosition;
        iPoint p = new iPoint((v.x - vx) / vw * devWidth,
                                (1f - (v.y - vy) / vh) * devHeight);
        //Debug.LogFormat($"screen({sw},{sh}) : input({v.x},{v.y}) => use({p.x},{p.y})");
        return p;
    }

}

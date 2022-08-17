using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static int devWidth = 1280, devHeight = 720;

    GameManager h;
    EventLog el;
    public List<string> str;

    public Vector2 scrollPosition = Vector2.zero;
    Vector3 prevV;
    public float moveScroll;
    bool drag;
    public bool scroll;

    Vector3 firstPosition;
    Vector3 lastPosition;

    // Start is called before the first frame update
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
        drag = false;
        scroll = false;
        moveScroll = 0;
    }

    // Update is called once per frame
    void Update()
    {
        iGUI.setResolution(devWidth, devHeight);

        //µå·¡±×
        int btn = 0;// 0:left, 1:right
        if (Input.GetMouseButtonDown(btn))
        {
            drag = true;

            prevV = Input.mousePosition;
            iPoint p = mousePosition();

            if (p.x > 20 && p.x < 220 && p.y > 100 && p.y < 600)
            {
                scroll = true;
            }
        }
        else if (Input.GetMouseButtonUp(btn))
        {
            drag = false;
            scroll = false;
        }

        if (drag)
        {
            Vector3 v = Input.mousePosition;
            if (prevV == v)
            {
                moveScroll = 0;
                return;
            }

            if(scroll)
            {
                moveScroll = prevV.y - v.y;
            }
            prevV = v;

            iPoint p = mousePosition();            
        }
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

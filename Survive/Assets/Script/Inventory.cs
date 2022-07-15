using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Sprite[] sp;
    public Image[] img;//인벤토리 이미지
    public Text[] txt;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<10; i++)
        {
            Color c = img[i].color;
            c.a = 0;
            img[i].color = c;
            txt[i].color = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void invenImg(string name, int idx, int count, int alpha)
    {
        img[idx].sprite = sp[0];//기본
        if (name=="apple")
        img[idx].sprite = sp[0];//테스트
        else if(name == "meat")
            img[idx].sprite = sp[1];//테스트
        txt[idx].text = count.ToString();

        Color c = img[idx].color;
        c.a = alpha;
        img[idx].color = c;
        txt[idx].color = c;
    }
}

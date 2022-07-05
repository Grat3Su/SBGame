using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //�÷��̾� �屸
    public float p_hunger = 1.0f;
    public float p_energy = 1.0f;
    public float p_clean = 1.0f;

    //���� �ð�
    public float dec_hunger = 1.0f;
    public float dec_energy = 1.0f;
    public float dec_clean = 1.0f;

    public float speed = 5.0f;
    public int dash_speed = 1;

    GameObject manager;
    PlayerManager pm;

    GameObject[] items;
    int itemidx;

    float timer;
    public float timeSet = 10f;//���ӿ����� 1��
    int printTime;//������ ������ �ð�
    int day;//��¥

    bool bedtime = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager");
        pm = manager.GetComponent<PlayerManager>();
        items = new GameObject[10];
        itemidx = 0;

        timer = 0;
        printTime = 0;
        day = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        DecEnergy();
        MoveTime();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            bedtime = true;
        }
        if(bedtime)
            BedTime();
    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            dash_speed = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            dash_speed = 1;
        }
            //���� ������ ������ �����̱�
        float x = Input.GetAxis("Horizontal") * speed * dash_speed* Time.deltaTime;
        float y = Input.GetAxis("Vertical") * speed * dash_speed* Time.deltaTime;

        if (x != 0 || y != 0)
        {
            Debug.Log(x + " " + y);
            transform.Translate(x, y, 0f);
        }
    }

    void DecEnergy()//���� ����
    {
        if (p_hunger > 0.0f)
            p_hunger = (p_hunger - (Time.deltaTime * 0.01f * dec_hunger));
        else
            p_hunger = 0.0f;
        if (p_energy > 0.0f)
            p_energy = (p_energy - (Time.deltaTime * 0.01f * dec_energy));
        else
            p_energy = 0.0f;
        if (p_clean > 0.0f)
            p_clean = (p_clean - (Time.deltaTime * 0.01f * dec_clean));
        else
            p_clean = 0.0f;
    }

    void IncEnergy(int type, float incSize)
    {
        if(type == 0)
        {
            p_hunger += incSize;
            if (p_hunger > 100)
                p_hunger = 100;
        }
        else if (type == 1)
        {
            p_clean += incSize;
            if (p_clean > 100)
                p_clean = 100;
        }
        else if (type == 2)
        {
            p_energy += incSize;
            if (p_energy > 100)
                p_energy = 100;
        }
    }

    void MoveTime()
    {
        timer += Time.deltaTime;

        if (timer > timeSet)
        {
            printTime += 1;
            timer = 0;

            int minute = printTime % 60;//5�и��� ����
            if (minute % 5 == 0)
            {
                pm.PrintTime();
            }
        }
    }

    public int GetDay()
    {
        return day;
    }
    public int GetTime()
    {
        return printTime;
    }


    void BedTime()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("sleep");
            //�ð� 8�ð� �߰�
            //������ 80++
            printTime += ( 60 * 8);
            IncEnergy(2, 80);
            bedtime = false;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("nap");
            //�ð� 2�ð� �߰�
            //������ 30++
            printTime += ( 60 * 2);
            IncEnergy(2, 30);
            bedtime = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<item>()!=null)
        {
            items[itemidx] = other.gameObject;//������ ����
            itemidx++;
        }
    }
}

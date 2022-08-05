using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    // ������ �ൿ
    // idle, move, atack..
    // �̺�Ʈ �� taketime��ŭ �����

    string name;//�̸�
    int behave;// 0 : idle / 1 : move / 2 : attack
    bool defence;//����?
    public int takeTime;//�ʿ� ���� �ð�
    public int job = 0;// 0 : ��� / 1 : ������� / 2 : �ϲ� / 3 : ��� / 4 : ������
    public Sprite[][] sp;
    GameManager h;
    
    void Start()
    {
        behave = 0;
        defence = false;
        h = GameObject.Find("GameManager").GetComponent<GameManager>();
        //job = 0;

        string[] n = {"��","��", "��", "��","��","��","��","��","��","��","ī","Ÿ","��","��", "��","��","��","��"};

        name = n[Random.Range(0, n.Length)]+ n[Random.Range(0, n.Length)];
        Debug.Log(name+"����");
        //�̸� ���� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(takeTime > 4)
            jobAction();
        if (defence)
            ;
    }

    void fight()
	{

	}
    

    void jobAction()//-> 
    {// 0 : ��� / 1 : ������� / 2 : Ž�谡 / 3 : ��� / 4 : ������
        //taketime ==4
        takeTime -= 4;
        if (job == 0)
		{
            //�� ��ȸ
        }
        else if (job == 1)
        {
            //���� �� �ο��
        }
        else if (job == 2)
        {
            //�ķ�/�� ��������
            int food = Random.Range(0, 1);
            int water = Random.Range(0, 1);            
            h.getItem(name, food, water);
        }
        else if (job == 3)
        {
            //�ķ� 1 �߰�
            int food = Random.Range(0, 3);
            h.getItem(name, food, 0);
        }
        else if (job == 4)
        {
            //���� ����Ʈ 1 �߰�
            h.labExp++;
        }
    }
}

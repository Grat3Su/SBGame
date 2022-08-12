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
    public int job = 0;// 0 : ��� / 1 : Ž�谡 / 2 : �ϲ� / 3 : ��� / 4 : ������
    public Sprite[][] sp;
    public Event h;
    
    void Start()
    {
        behave = 0;
        defence = false;
        //h = GameObject.Find("GameManager").GetComponent<Event>();
        //job = 0;

        string[] n = {"��","��", "��", "��","��","��","��","��","��","��","ī","Ÿ","��","��", "��","��","��","��"};

        name = n[Random.Range(0, n.Length)]+ n[Random.Range(0, n.Length)];
        Debug.Log(name+"����");
        gameObject.name = name;
        //�̸� ���� ����

        job = Random.Range(0, 5);
        if (job == 1)
        {
            h.explorer++;
        }
        else if (job == 2)
            h.workman++;
    }

    // Update is called once per frame
    void Update()
    {
        if(takeTime > 4)
            jobAction();
        if (defence)
            ;
    }

    //���� ����
    public void comeBack()
	{        
        //2�ð� �̻��̸� ���� ������
        if (takeTime < 2)
        {
            takeTime = 0;
            return;
        }

        if (job == 0)
        {
            //�� ��ȸ
        }
        else if (job == 1)
        {
            Debug.Log(name + "����");
            //Ž��. ���� ������ �߰�
            h.storage.addStorage(4, 1);
            int people = Random.Range(0, 100);
            if (people > 80)
            {
                h.storage.addStorage(0, 1);
                Debug.Log(name + "�� ������ �߰�"); 
            }
            Debug.Log(name + " �� ����ġ 1 ȹ��");
        }
        else if (job == 2)
        {
            Debug.Log(name + "����");
            //�ķ�/�� ��������
            int food = (int)((float)Random.Range(0, 3)/2 + 0.5f);
            h.storage.addStorage(1, food);
            Debug.Log(name + " �ķ� "+food+" ȹ��");
        }
        else if (job == 3)
        {
            Debug.Log(name + "����");
            //�ķ� 1 �߰�
            int food = (int)((float)Random.Range(3, 4) / 2 + 0.5f);
            h.storage.addStorage(1, food);
        }
        else if (job == 4)
        {
            Debug.Log(name + "����");
            //���� ����Ʈ 1 �߰�
            h.storage.addStorage(3, 2);
            Debug.Log(name + " ���� ����ġ 1 ȹ��");
        }
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
            //Ž��. ���� ������ �߰�
            h.storage.addStorage(4, 2);
            int people = Random.Range(0, 100);
            if (people > 80)
            {
                h.storage.addStorage(0, 1);
                Debug.Log(name + "�� ������ �߰�");
            }
            Debug.Log(name + " �� ����ġ 2 ȹ��");
        }
        else if (job == 2)
        {
            //�ķ�/�� ��������
            int food = Random.Range(0, 3);
            h.storage.addStorage(1, food);

            Debug.Log(name + " �ķ� " + food + " ȹ��");
        }
        else if (job == 3)
        {
            //�ķ� 1 �߰�
            int food = Random.Range(3, 4);
            h.storage.addStorage(1, food);
            Debug.Log(name + " �ķ� " + food + " ȹ��");
        }
        else if (job == 4)
        {
            //���� ����Ʈ 2 �߰�
            h.storage.addStorage(3, 2);
            Debug.Log(name + " ���� ����ġ 2 ȹ��");
        }
    }

    //���� ����
    public void jobUpdate(int newjob)
	{
        if (newjob == job)//�� ������ �ٸ� ��
            return;

        if(job == 0)
		{

		}
        else if(job == 1)
		{
            h.explorer--;
		}
        else if(job == 2)
		{
            h.workman--;
		}

        job = newjob;

        if (job == 1)
            h.explorer++;
        else if (job == 2)
            h.workman++;
    }
}

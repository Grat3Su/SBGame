using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
	// ������ �ൿ
	// idle, move, atack..
	// �̺�Ʈ �� taketime��ŭ �����

	public int[] jobLevel;
	int jobExp;

	string name;//�̸�
	public int behave;// 0 : idle / 1 : move / 2 : attack / 2 : work / 3 : disease
	bool defence;//����?
	public int takeTime;//�ʿ� ���� �ð�
	public int job = 0;// 0 : ��� / 1 : Ž�谡 / 2 : �ϲ� / 3 : ��� / 4 : ������
	public Sprite[][] sp;
	public Event h;
	public Texture jobTex;

	void Start()
	{
		behave = 0;
		defence = false;
		jobLevel = new int[5] { 0, 1, 1, 1, 1 };//�ѹ� ���� ���������� ���� ���?
		jobExp = 0;

		string[] n = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "ī", "Ÿ", "��", "��", "��", "��", "��", "��", "��", "��" };
		jobTex = Resources.Load<Texture>("people");
		name = n[Math.random(0, n.Length)] + n[Math.random(0, n.Length)];
		Debug.Log(name + "����");
		gameObject.name = name;
		//�̸� ���� ����

		job = 0;
		int newjob = Math.random(0, 5);
		jobUpdate(newjob);
	}

	// Update is called once per frame
	void Update()
	{
		if (takeTime > 4)
			jobAction();
	}

	//���� ����
	public void comeBack()
	{
		jobExp += 1;

		if (jobLevel[job] < 20)
		{
			if (Math.random(0, 80 + jobLevel[job]) > 20)
			{
				Debug.Log("�ƹ��͵� ���� ����");

				if (jobExp > jobLevel[job] * 2)
				{
					jobExp -= 2;
					jobLevel[job]++;
				}
			}
		}

		if (jobExp > jobLevel[job] * 2)
		{
			jobExp -= 2;
			jobLevel[job]++;
		}
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
			h.getDay.stageExp += 1;

			int people = Math.random(0, 100);
			if (people > 80)
			{
				h.storage.addStorage(0, 1);
				h.getDay.people += 1;
				Debug.Log(name + "�� ������ �߰�");
			}
			Debug.Log(name + " �� ����ġ 1 ȹ��");
		}
		else if (job == 2)
		{
			jobTex = Resources.Load<Texture>("people");
			Debug.Log(name + "����");
			//�ķ�/�� ��������
			int food = (int)((float)Math.random(0, 3) / 2 + 0.5f);
			h.storage.addStorage(1, food);
			Debug.Log(name + " �ķ� " + food + " ȹ��");
			h.getDay.food += food;
		}
		else if (job == 3)
		{
			Debug.Log(name + "����");
			//�ķ� 1 �߰�
			int food = (int)((float)Math.random(3, 4) / 2 + 0.5f);
			h.storage.addStorage(1, food);
			h.getDay.food += food;
		}
		else if (job == 4)
		{
			Debug.Log(name + "����");
			//���� ����Ʈ 1 �߰�
			h.storage.addStorage(3, 2);
			h.getDay.labExp += 2;
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
			behave = 0;
		}
		else if (job == 1)
		{
			behave = 3;
			//Ž��. ���� ������ �߰�
			h.storage.addStorage(4, 2);
			h.getDay.stageExp += 2;
			int people = Math.random(0, 100);
			if (people > 80)
			{
				h.storage.addStorage(0, 1);
				h.getDay.people += 1;
				Debug.Log(name + "�� ������ �߰�");
			}
			Debug.Log(name + " �� ����ġ 2 ȹ��");
		}
		else if (job == 2)
		{
			behave = 3;
			//�ķ�/�� ��������
			int food = Math.random(0, 3);
			h.storage.addStorage(1, food);
			h.getDay.food += food;

			Debug.Log(name + " �ķ� " + food + " ȹ��");
		}
		else if (job == 3)
		{
			behave = 3;
			//�ķ� 1 �߰�
			int food = Math.random(3, 4);
			h.storage.addStorage(1, food);
			h.getDay.food += food;
			Debug.Log(name + " �ķ� " + food + " ȹ��");
		}
		else if (job == 4)
		{
			behave = 3;
			//���� ����Ʈ 2 �߰�
			h.storage.addStorage(3, 2);
			h.getDay.labExp += 2;
			Debug.Log(name + " ���� ����ġ 2 ȹ��");
		}
		jobExp += 2;
		if (jobExp > jobLevel[job] * 2)
		{
			jobExp -= 2;
			jobLevel[job]++;
		}
	}

	//���� ����
	public void jobUpdate(int newjob)
	{
		if (newjob == job)//�� ������ �ٸ� ��
			return;

		if (job == 0)
		{
		}
		else if (job == 1)
		{
			h.explorer--;
		}
		else if (job == 2)
		{
			h.workman--;
		}

		job = newjob;
		jobExp = 0;//����ġ �ʱ�ȭ

		if (job == 0)
		{
			jobTex = Resources.Load<Texture>("people");
		}
		else if (job == 1)
		{
			jobTex = Resources.Load<Texture>("explorer");
		}
		else if (job == 2)
		{
			jobTex = Resources.Load<Texture>("farmer");
		}
		else if (job == 3)
		{
			jobTex = Resources.Load<Texture>("farmer");
		}
		else if (job == 3)
		{
			jobTex = Resources.Load<Texture>("researcher");
		}
	}
}

class newpState
{//�ؾ��ϴ� �� : ������
	public int[] jobLevel;
	int jobExp;

	string name;//�̸�
	public int behave;// 0 : idle / 1 : move / 2 : attack / 2 : work / 3 : disease
	bool defence;//����?
	public int takeTime;//�ʿ� ���� �ð�
	public int job = 0;// 0 : ��� / 1 : Ž�谡 / 2 : �ϲ� / 3 : ��� / 4 : ������
	public Sprite[][] sp;
	public Event h;
	public Texture jobTex;

	void init()
	{
		behave = 0;
		defence = false;
		jobLevel = new int[5] { 0, 1, 1, 1, 1 };//�ѹ� ���� ���������� ���� ���?
		jobExp = 0;

		string[] n = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "ī", "Ÿ", "��", "��", "��", "��", "��", "��", "��", "��" };
		jobTex = Resources.Load<Texture>("people");
		name = n[Math.random(0, n.Length)] + n[Math.random(0, n.Length)];
		Debug.Log(name + "����");
		//gameObject.name = name;
		//�̸� ���� ����

		job = 0;
		int newjob = Math.random(0, 5);
	}

	void update()
	{

	}

	void jobAction()// 0 : ��� / 1 : ������� / 2 : Ž�谡 / 3 : ��� / 4 : ������
	{
		if (job == 2)//Ž�谡
		{
			//
		}
		else if (job == 3)
		{

		}
		else if (job == 4)
		{

		}
	}
}

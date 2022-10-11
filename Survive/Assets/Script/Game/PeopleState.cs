using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
	// ������ �ൿ
	// idle, move, atack..
	// �̺�Ʈ �� taketime��ŭ �����

	public int[] jobLevel;
	int jobExp;
	public iPoint pos;

	public string name;//�̸�
	public int behave;// 0 : idle / 1 : move / 2 : back / 3 : work / 4 : disease / 5: ���	
	public float moveDt;
	int healthTime;
	public int takeTime;//�ʿ� ���� �ð�
	public int job = 0;// 0 : ��� / 1 : Ž�谡 / 2 : �ϲ� / 3 : ��� / 4 : ������
	public int jobReserve;
	public Sprite[][] sp;
	public Event h;

	void Start()
	{
		init();
	}
	public void init()
	{
		pos = new iPoint(0, 0);
		behave = 0;
		moveDt = 0f;
		jobLevel = new int[5] { 0, 1, 1, 1, 1 };//�ѹ� ���� ���������� ���� ���?
		jobExp = 0;
		healthTime = 3;

		job = 0;
		jobReserve = -1;
		int newjob = Math.random(0, 5);
		gameObject.AddComponent<SpriteRenderer>().sortingLayerName = "Layer 1";
		jobUpdate(newjob);
	}

	// Update is called once per frame
	void Update()
	{
		if (name == "null")
			return;
		if (takeTime > 3 && behave==2)
			jobAction();
		else if(behave == 4)
		{
			if (healthTime > 0)
			{
				healthTime--;
			}
			else if (healthTime == 0)
			{
				healthTime = 3;
				behave = 0;
				Debug.Log("����");
			}
			takeTime -= 4;
		}

		if (behave == 5)
			name = "null";
	}

	void jobAction()//-> 
	{// 0 : ��� / 1 : ������� / 2 : Ž�谡 / 3 : ��� / 4 : ������
	 //taketime ==4
		takeTime -= 4;
		if (takeTime < 0)
			takeTime = 0;
		//Debug.Log("work");
		int bonus = jobLevel[job] * 2;

		if (job == 0)
		{
			//�� ��ȸ
			behave = 0;
		}
		else if (job == 1)
		{
			//Ž��. ���� ������ �߰�
			if (Random.Range(0, 100) > (80 - bonus))
			{
				h.storage.addStorage(0, 1);
				h.plusItem.people += 1;
			}
			h.storage.addStorage(4, 2 + bonus);
			h.plusItem.mapExp += 2;
		}
		else if (job == 2)
		{
			//�ķ�/�� ��������
			int mount = Math.random(bonus, bonus + 3);
			h.plusItem.food += mount;
			h.storage.addStorage(1, mount);
		}
		else if (job == 3)
		{
			//�ķ� �߰�
			int mount = Math.random(bonus, bonus + 2);
			h.storage.addStorage(1, mount);
			h.plusItem.food += mount;
		}
		else if (job == 4)
		{
			//���� ����Ʈ 2 �߰�
			int mount = Math.random(bonus, bonus + 2);
			h.storage.addStorage(3, mount);
			h.plusItem.labExp += mount;
		}

		jobExp += 2;
		if (jobExp > jobLevel[job] * 5)
		{
			jobExp -= (jobLevel[job]-1) * 5;
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

		Sprite sp = Resources.Load<Sprite>("jobless");
		if (job == 0)
		{
			sp = Resources.Load<Sprite>("jobless");
		}
		else if (job == 1)
		{
			sp = Resources.Load<Sprite>("explorer");
		}
		else if (job == 2)
		{
			sp = Resources.Load<Sprite>("worker");
		}
		else if (job == 3)
		{
			sp = Resources.Load<Sprite>("farmer");
		}
		else if (job == 4)
		{
			sp = Resources.Load<Sprite>("researcher");
		}
		gameObject.GetComponent<SpriteRenderer>().sprite = sp;
	}
}
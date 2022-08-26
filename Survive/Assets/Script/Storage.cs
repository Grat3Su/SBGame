using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Storage
{
    int people, _people;
    int food, _food;
    int labExp;
    int labLevel;
    int stage;// 0-5 / 5-10 /
    int stageExp;

    public Storage(int p, int f, int e, int l, int se, int sl)
	{
        people = p;
        food = f;
        labExp = e;
        labLevel = l;
        stage = sl;
        stageExp = se; 
        
        _people = labLevel * 3;
        _food = labLevel * 5;
    }

    public int getStorage(int type)
	{
#if false
        if (type == 0)
            return people;
        else if (type == 1)
            return food;
        else if (type == 2)
            return water;
        if (type == 3)
            return labExp;
        if (type == 4)
            return labLevel;

        return 0;
#else
        int[] r = new int[] { people, food, labExp, labLevel, stage, stageExp };
        return r[type];
#endif
    }

    public string getStorageText(int type)
	{
        int[] r = new int[] { people, food, labLevel, stage};
        int[] r0 = new int[] { _people, _food, labExp, stageExp};
        return r[type] + " / " + r0[type];
	}

    public void addStorage(int type, int mount)
    {
        if (type == 0)
            people += mount;
        else if (type == 1)
            food += mount;
        if (type == 3)
            labExp += mount;
        if (type == 4)
            stageExp += mount;
        
        update();
    }

    public void peopleTakeItem()
	{
        if (food - people < 0)
        {
            people -= people - food;
            food = 0;
        }
        else
        food -= people;

        update();
    }
    //����, ����, ��, ����, ����, �� �̺�Ʈ �߰�.
    //�ڿ� �߰� - ������ �� �ִ� ���� ��.
    //�ʿ� �����ִ� �ڿ� �߰�?
    //
    private void update()
    {
        int need = labLevel < 10 ? 4 * labLevel :2 * labLevel+20;
        while (labExp > need)
        {
            labExp -= need;
            labLevel++;

            _people = labLevel * 3;
            _food = labLevel * 5;

            need = labLevel < 10 ? 4 * labLevel : 2 * labLevel + 20;
        }

        //4 8 12 16 ...

        while(stageExp > stage*4)
		{
                stage++;
		}

        if (people > _people)
            people = _people;
        else if (people < 0)
            people = 0;

        if (food > _food)
            food = _food;
        else if (food == 0)
            food = 0;

        Debug.Log("------------------------------------------");
        Debug.Log("peopel : "+people+"/"+_people);
        Debug.Log("food : "+ food + "/" + _food);
        Debug.Log("labLevel : "+ labLevel + " / exp : "+labExp);
        Debug.Log("stage : "+ stage + " / exp : " + stageExp);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Storage
{
    int people, _people;
    int food, _food;
    int water, _water;
    int labExp;
    int labLevel;
    int stage;// 0-5 / 5-10 /
    int stageExp;

    public Storage(int p, int f, int w, int e, int l, int se, int sl)
	{
        people = p;
        food = f;
        water = w ;
        labExp = e;
        labLevel = l;
        stage = sl;
        stageExp = se; 
        
        _people = labLevel * 3;
        _food = labLevel * 5;
        _water = labLevel * 5;
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
        int[] r = new int[] { people, food, water, labExp, labLevel, stage, stageExp };
        return r[type];
#endif

    }

    public void addStorage(int type, int mount)
    {
        if (type == 0)
            people += mount;
        else if (type == 1)
            food += mount;
        else if (type == 2)
            water += mount;
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

        if (water - people < 0)
        {
            water = 0;
            people -= people - water;
        }
        else
        water -= people;

        update();
    }
    //내분, 독립, 병, 협력, 습격, 등 이벤트 추가.
    //자원 추가 - 공격할 수 있는 무기 등.
    //맵에 남아있는 자원 추가?
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
            _water = labLevel * 5;

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

        if (water > _water)
            water = _water;
        else if (water == 0)
            water = 0;


        Debug.Log("------------------------------------------");
        Debug.Log("peopel : "+people);
        Debug.Log("food : "+ food);
        Debug.Log("water : "+ water);
        Debug.Log("labLevel : "+ labLevel + " / exp : "+labExp);
        Debug.Log("stage : "+ stage + " / exp : " + stageExp);

    }
}

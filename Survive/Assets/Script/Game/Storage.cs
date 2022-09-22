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
        int[] r = new int[] { people, food, labExp, labLevel, stage, stageExp };
        return r[type];
    }

    public string getStorageText(int type)
	{
        int[] r = new int[] { people, food, labLevel, stage, labExp, stageExp };
        int[] r0 = new int[] { _people, _food };

        int need = labLevel < 10 ? 4 * labLevel : 2 * labLevel + 20;
        if (type == 4)
            return r[type] + " / " + need;
        else if (type == 5)
            return r[type] + " / " + r[type] * 4;
        else if (type > 1)
            return r[type] + "레벨";

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
public class newStorage//저장고. 주로 자원 보관.
{
    public int people, _people;
    public int food, _food;
    public int lab, map;
    public int labExp, mapExp;

    public newStorage(int p, int f, int l, int m, int le, int me)
    {
        people = p;
        food = f;
        lab = l; labExp = le;
        map = m; mapExp = me;

        _people = lab * 3;
        _food = lab * 5;
    }
    public string getStorageText(int type)
    {
        int[] r = new int[] { people, food, lab, map, labExp, mapExp };
        int[] r0 = new int[] { _people, _food };

        int need = lab < 10 ? 4 * lab : 2 * lab + 20;
        if (type == 4)
            return r[type] + " / " + need;
        else if (type == 5)
            return r[type] + " / " + r[type] * 4;
        else if (type > 1)
            return r[type] + "레벨";

        return r[type] + " / " + r0[type];
    }

    public void update()
    {
        int need = lab < 10 ? 4 * lab : 2 * lab + 20;
        while (labExp > need)
        {
            labExp -= need;
            lab++;

            _people = lab * 3;
            _food = lab * 5;

            if (_people > 100)
                _people = 100;

            need = lab < 10 ? 4 * lab : 2 * lab + 20;
        }

        if (people > _people)
            people = _people;

        if (food > _food)
            food = _food;

        //4 8 12 16 ...

        while (mapExp > map * 4)
        {
            mapExp -= map * 4;
            map++;
        }

        if (people > _people)
            people = _people;
        else if (people < 0)
            people = 0;

        if (food > _food)
            food = _food;
        else if (food == 0)
            food = 0;
    }
}
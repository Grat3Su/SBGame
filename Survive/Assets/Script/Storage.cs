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

    public Storage(int p, int f, int w, int e, int l)
	{
        people = p;
        food = f;
        water = w ;
        labExp = e;
        labLevel = l;
        
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
        int[] r = new int[] { people, food, water, labExp, labLevel };
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

        update();
    }

    public void peopleTakeItem()
	{
        if (food - people < 0)
            people -= people - food;
        food -= people;
        if (water - people < 0)
            people -= people - water;
        water -= people;

        update();
    }

    private void update()
    {
        int need = labLevel > 5 ? 4 * labLevel : 2 * labLevel;
        while (labLevel < need)
        {
            if (labExp == need)
            {
                labExp -= need;
                labLevel++;

                _people = labLevel * 3;
                _food = labLevel * 5;
                _water = labLevel * 5;

                need = labLevel > 10 ? 4 * labLevel : 20 + 2 * labLevel;
            }
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
    }
}

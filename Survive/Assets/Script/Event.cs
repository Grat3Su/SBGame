using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        initGame();
    }

    public Storage storage;
    int day;
    int hour;
    int stage;// 0-5 / 5-10 /
    int stagExp;

    void initGame()
	{
        storage = new Storage(1, 5, 5, 0, 1);
        day = 0;
        hour = 0;
        stage = 1;// 0-5 / 5-10 /
        stagExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    enum DoEvent
	{
        Adventure,
        Hunt,
        Research,
        Defense,
        Disease
	}

    struct AddItem
	{
        public int people;
        public int food;
        public int water;
        public int takeTime;
        public int labExp;

        public AddItem(int init)
		{
            people = init;
            food = init;
            water = init;
            takeTime = init;
            labExp = init;
        }
	}

	void doEvent(DoEvent type)
	{
        AddItem item = new AddItem(0);
        if(type == DoEvent.Adventure)
		{
            item.food = Random.Range(0, 2);
            item.water = Random.Range(1, 3);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;

            stagExp += 4;

            while (stagExp > stage * 4)
            {
                if (stagExp > stage * 4)
                {
                    stagExp -= stage * 4;
                    stage++;
                }
            }
        }
        else if (type == DoEvent.Hunt)
		{
            int maxGain = stage * storage.getStorage(0);
            item.food = Random.Range(1, 3);
            item.water = Random.Range(0, 2);
            item.people = Random.Range(0, 100) > 50 ? Random.Range(1, 2) : 0;
            item.takeTime = 4;
        }
        else if (type == DoEvent.Research)
		{
            int labLevel = storage.getStorage(4);
            item.labExp = labLevel < 5 ? Random.Range(1,3) : Random.Range(2, 5);
            item.takeTime = 4;
        }
        else if (type == DoEvent.Defense)
		{
            item.food = Random.Range(0, 100) > 80 ? -Random.Range(1, 2) : 0;
            item.people = Random.Range(0, 100) > 50 ? -Random.Range(1, 2) : 0;
            item.takeTime = 8;
        }
        else if (type == DoEvent.Disease)
		{
            int people = storage.getStorage(0);
            int weak = Random.Range(0, people - 1);
            item.food = -weak;
            item.water = -weak;
            item.people = Random.Range(0, 100) > 80 ? -Random.Range(0, weak) : 0;
            item.takeTime = 4;
        }

        updateEvent(item);
    }

    void updateEvent(AddItem item)
	{
        storage.addStorage(0, item.people);
        storage.addStorage(1, item.food);
        storage.addStorage(2, item.water);
        storage.addStorage(3, item.labExp);

        item.takeTime += hour;
        if(hour>11)//12½Ã ¶¯
		{
            hour -= 12;
            nextDay();
        }
	}

    void nextDay()
	{
        storage.peopleTakeItem();

    }

}
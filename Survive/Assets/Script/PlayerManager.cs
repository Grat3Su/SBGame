using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerBehaviour player;
    public Slider[] desire = new Slider[3];//hunger clean energy
    float fSliderBarTime;
    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        player = player.GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hunger.value);
        if (desire != null)
        {
            desire[0].value = player.p_hunger;
            desire[1].value = player.p_clean;
            desire[2].value = player.p_energy;
        }
    }

    public void PrintTime()
    {
        if (timeText)
        {
            int printTime = player.GetTime();
            int minute = printTime % 60;
            int hour = printTime / 60;

            timeText.text = string.Format($"{player.GetDay()}일차 {hour}시 {minute}분");
        }
    }

}
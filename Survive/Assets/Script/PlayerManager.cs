using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerBehaviour player;
    public Slider[] desire = new Slider[3];//hunger clean energy
    float fSliderBarTime;
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

}
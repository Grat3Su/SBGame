using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerBehaviour player;
    public Slider[3] desire;//hunger clean energy
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
        hunger.desire[0] = player.p_hunger;
        hunger.desire[1] = player.p_clean;
        hunger.desire[2] = player.p_energy;
    }
}
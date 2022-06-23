using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //플레이어 욕구
    public float p_hunger = 1.0f;
    public float p_energy = 1.0f;
    public float p_clean = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p_hunger > 0.0f)
            p_hunger = (p_hunger - (Time.deltaTime*0.01f));
        else
            p_hunger = 0.0f;

       // Debug.Log(p_hunger);
    }

    public float GetHunger()
    {
        return p_hunger;
    }
}

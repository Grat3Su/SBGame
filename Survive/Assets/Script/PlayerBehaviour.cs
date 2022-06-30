using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //플레이어 욕구
    public float p_hunger = 1.0f;
    public float p_energy = 1.0f;
    public float p_clean = 1.0f;

    //감소 시간
    public float dec_hunger = 1.0f;
    public float dec_energy = 1.0f;
    public float dec_clean = 1.0f;

    public float speed = 5.0f;
    public int dash_speed = 1;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerMove();
        DecEnergy();
    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            dash_speed = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            dash_speed = 1;
        }
            //쉽고 빠르고 간단한 움직이기
        float x = Input.GetAxis("Horizontal") * speed * dash_speed* Time.deltaTime;
        float y = Input.GetAxis("Vertical") * speed * dash_speed* Time.deltaTime;

        if (x != 0 || y != 0)
        {
            Debug.Log(x + " " + y);
            transform.Translate(x, y, 0f);
        }
    }

    void DecEnergy()
    {
        if (p_hunger > 0.0f)
            p_hunger = (p_hunger - (Time.deltaTime * 0.01f * dec_hunger));
        else
            p_hunger = 0.0f;
        if (p_energy > 0.0f)
            p_energy = (p_energy - (Time.deltaTime * 0.01f * dec_energy));
        else
            p_energy = 0.0f;
        if (p_clean > 0.0f)
            p_clean = (p_clean - (Time.deltaTime * 0.01f * dec_clean));
        else
            p_clean = 0.0f;
    }
    
}

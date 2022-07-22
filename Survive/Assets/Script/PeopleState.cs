using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    // 생존자 행동
    // idle, move, atack..
    // 이벤트 시 사라지기까지

    int behave;// 0 : idle / 1 : move / 2 : attack

    void Start()
    {
        behave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

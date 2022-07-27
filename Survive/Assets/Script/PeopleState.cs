using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    // 생존자 행동
    // idle, move, atack..
    // 이벤트 시 사라지기까지

    string name;//이름
    int behave;// 0 : idle / 1 : move / 2 : attack
    int takeTime;//맵에 없을 시간

    void Start()
    {
        behave = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GoEvent()
    {
        //지시에 따라 이벤트 수행
    }
}

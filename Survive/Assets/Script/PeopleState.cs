using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleState : MonoBehaviour
{
    // ������ �ൿ
    // idle, move, atack..
    // �̺�Ʈ �� ����������

    string name;//�̸�
    int behave;// 0 : idle / 1 : move / 2 : attack
    int takeTime;//�ʿ� ���� �ð�

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
        //���ÿ� ���� �̺�Ʈ ����
    }
}

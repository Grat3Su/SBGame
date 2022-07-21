using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    int itemlength;
    int day, hour;
    bool death;
    int people;//생존자. 이벤트 시 랜덤하게 획득
    int food;//하루마다 사람수만큼 차감
    int water;

    // Start is called before the first frame update
    void Start()
    {
        day = 0;
        death = false;      
        itemlength = 0;
        people = 1;//처음은 혼자 시작
        water = 0;
        food = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void eventTime(int type)
    {
        if (type == 4)
            type = Random.Range(0, 2);

        int takeTime = 0;
        int p = people / 2;

        switch (type)
        {
            case 0:
                //탐색
                Debug.Log("탐색");
                //addItem(new Item("food", 3, 2, 1, 5));//2개 획득. test
                water += (3 + Random.Range(0, p));//인구 수 보너스
                food += (1 + Random.Range(0, p));
                addPeople(3);
                takeTime = 4;
                break;
            case 1:
                //사냥
                Debug.Log("사냥");
                //addItem(new Item("food", 3, 3, 2, 10));//3개 획득
                addPeople(2);
                food += (3 + Random.Range(0, p));
                water += (1 + Random.Range(0, p));//인구 수 보너스
                takeTime = 4;
                break;
            case 2:
                //습격
                Debug.Log("습격");
                takeTime = 10;
                break;
            case 3:
                //상인
                Debug.Log("상인");
                takeTime = 2;
                break;
            default:
                Debug.Log("아무일도 없었음");
                break;
        }

        hour += takeTime;
        if (hour > 12)
        {
            hour -= 12;
            nextDay();//시간이 끝났으니 강제 이동
        }
        Debug.Log("현재 시간 : " + hour + "시");

        //printItem();
    }

    void addPeople(int max)
    {
        int r = Random.Range(0, max);
        if (r == 0)
            return;
        people += r;
        Debug.Log(r + "명 구출");
        Debug.Log("현재 인구 수 " + people + "명");
    }

    void nextDay()
    {
        //식량 소비        
        if (food < people)
        {
            food -= people;//사람 수만큼 줄이기
            people -= (people - food);//식량이 부족한 수만큼 줄기
            food = 0;//다먹음
        }
        else
            food -= people;//사람 수만큼 줄이기

        if (water < people)
        {
            people -= (people - water);//식량이 부족한 수만큼 줄기
            water = 0;//다먹음
        }
        else
            water -= people;//사람 수만큼 줄이기

        if (people < 0)
        {
            people = 0;
            GameOver("남은 사람이 없다");
            death = true;
        }

        // 사람수만큼 식량 까기 => 식량 부족이면 인구 줄어들기 => 모두 죽으면 게임오버
        //씻기

        day++;

        //printItem();

        // 자산 뭐 남은지 디스페리이


        // 새로운날 표시day
        Debug.LogFormat($"{day}일차 음식 {food}개, 물 {water}개, 인구 {people}명");//음식 물 자원 인구 수
    }

    public void GameOver(string reason)
    {

        Debug.Log(reason);
        Debug.Log(day + "일 동안 생존");
    }


}


class Display
{
    public static int[] day(int sec)
    {
        int[] m = new int[4];
        m[0] = sec % 60;
        m[1] = (sec / 60) % 60;
        m[2] = ((sec / 60) / 60) % 24;
        m[3] = ((sec / 60) / 60) / 24;
        return m;
    }

    public static string elecTime(int sec)
    {
        return "00:00";
    }

    public static string hanTime(int sec)
    {
        return "0시간 0 분";
    }
}
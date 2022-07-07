using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tent : MonoBehaviour
{
    PlayerBehaviour player;
    public GameObject bedUI;
    public Text nap;
    public Text sleep;

    bool bedtime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
        bedtime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bedtime)
            BedTime();
    }

    void BedTime()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("sleep");
            //시간 8시간 추가
            //에너지 80++

            player.SetTime(60 * 8);
            player.IncEnergy(2, 8);

        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("nap");
            //시간 2시간 추가
            //에너지 30++

            player.SetTime(60 * 2);
            player.IncEnergy(2, 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player");
        if (other.tag == "Player")
        {
            bedUI.SetActive(true);
            sleep.text = "F1 : Sleep";
            nap.text = "F1 : Nap";

            bedtime = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player");
        if (collision.tag == "Player")
        {
            bedUI.SetActive(true);
            sleep.text = "F1 : Sleep";
            nap.text = "F1 : Nap";

            bedtime = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bedUI.SetActive(false);
            bedtime = false;
        }
    }
}

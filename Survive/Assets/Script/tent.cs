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

    private Transform target;
    public float height = 2.0f;

    bool bedtime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
        bedtime = false;

        target = GetComponent<Transform>();
        bedUI.SetActive(false);

        //moveUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (bedtime)
        {
            moveUI();
            BedTime();
        }
    }

    void moveUI()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Vector3 tpos = target.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(tpos.x+2, tpos.y+2, tpos.z));//Ÿ���� �������� ����Ʈ ��ǥ�� ��ȯ

        bedUI.transform.position = screenPos;
    }

    void BedTime()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("sleep");
            //�ð� 8�ð� �߰�
            //������ 80++

            player.SetTime(60 * 8);//�ð� ����
            player.IncEnergy(2, 8);//������ ä��

        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("nap");
            //�ð� 2�ð� �߰�
            //������ 30++

            player.SetTime(60 * 2);
            player.IncEnergy(2, 3);
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

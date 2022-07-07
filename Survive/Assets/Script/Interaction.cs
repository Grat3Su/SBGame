using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public GameObject targetUI;
    bool harvest;
    SpriteRenderer renderer;
    Sprite[] sp;
    bool inter;
    public Text t;

    private Transform target;
    public float height = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        harvest = false;
        renderer = GetComponent<SpriteRenderer>();
        sp = Resources.LoadAll<Sprite>("bush");

        if (sp != null)
            Debug.Log(sp[0].ToString());
        inter = false;

        target = GetComponent<Transform>();
        targetUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inter)
        {
            moveUI();
            if (Input.GetKeyDown(KeyCode.Space))
                if (!harvest)
                    Harvest();
        }
    }

    void Grow()
    {
        if(harvest)
            renderer.sprite = sp[1];
    }

    public void Harvest()
    {
        Debug.Log("apple");
        renderer.sprite = sp[0];

        GameObject apple = new GameObject("apple");
        SpriteRenderer sr = apple.gameObject.AddComponent<SpriteRenderer>();
        //SpriteRenderer sr = apple.GetComponent<SpriteRenderer>();
        sr.sprite = sp[2];
        apple.transform.position = new Vector2(transform.position.x, transform.position.y -1);

        harvest = true;
    }

    void moveUI()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Vector3 tpos = target.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(tpos.x + 1, tpos.y + height, tpos.z));//Å¸°ÙÀÇ Æ÷Áö¼ÇÀ» ºäÆ÷Æ® ÁÂÇ¥·Î º¯È¯

        targetUI.transform.position = screenPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player");
        if (collision.tag == "Player")
        {
            targetUI.SetActive(true);
            t.text = "Space : harvest";

            inter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            targetUI.SetActive(false);
            inter = false;
        }
    }
}

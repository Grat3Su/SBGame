using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    bool harvest;
    SpriteRenderer renderer;
    Sprite[] sp;
    // Start is called before the first frame update
    void Start()
    {
        harvest = false;
        renderer = GetComponent<SpriteRenderer>();
        sp = Resources.LoadAll<Sprite>("bush");

        if (sp != null)
            Debug.Log(sp[0].ToString());

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            if(!harvest)
                Harvest();
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
}

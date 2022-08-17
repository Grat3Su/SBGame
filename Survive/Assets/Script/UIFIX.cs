using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFIX : iGUI
{
	public Event pEvent;
	string[] btnTxt;
	int idx;
	int x;
	int y;
	int startp;
    int curidx;
	// Start is called before the first frame update
	void Start()
	{
        init();

        setProject();
		btnTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };

		idx = 0;
        curidx = -1;
        startp = 0;
	}


	// Update is called once per frame
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
            if (curidx > -1)
                curidx = -1;
    }

	RenderTexture texScrollView;
	int prevFrameCount;

	private void OnGUI()
	{
		setProject();

		float delta = 0.0f;
		if( prevFrameCount!=Time.frameCount)
		{
			prevFrameCount = Time.frameCount;
			delta = Time.deltaTime;
		}
		updateScrollView(delta);
		GUI.DrawTexture(new Rect(20, 100, texScrollView.width, texScrollView.height), texScrollView);

        if (curidx > -1)
        {
            setRGBA(0, 0, 0, 0.5f);
            fillRect(MainCamera.devWidth / 2 - 200, 30, 800, MainCamera.devHeight - 60);
            idx = pEvent.pState[curidx].job;

            setRGBA(1, 1, 1, 1);
            if (drawButton(new Rect(MainCamera.devWidth / 2+400, MainCamera.devHeight / 2 -50, 100, 50), btnTxt[pEvent.pState[curidx].job]))
            {
                idx++;
                if (idx > btnTxt.Length - 1)
                    idx = 0;
                pEvent.pState[curidx].jobUpdate(idx);
            }
        }
    }

	float offX, offY;
	string[] names;
	float svDt;
    float move;
    //prev - cur
	void updateScrollView(float dt)
	{
		if( texScrollView==null )
		{
			texScrollView = new RenderTexture(200, 500, 32, RenderTextureFormat.ARGB32);
			names = new string[] {
				"name 0", "name 1", "name 2", "name 3", "name 4",
				"name 5", "name 6", "name 7", "name 8", "name 9" , "name 19" , "name 29" , "name 9" , "name 9" , "name 9" , "name 9" , "name 9" , "name 9" , "name 59" };
			offX = (texScrollView.width - 150)/2;
			offY = 10;
			svDt = 0.0f;
		}
        int people = pEvent.storage.getStorage(0);
        if (Camera.main.GetComponent<MainCamera>().scroll)
        {
            float move = Camera.main.GetComponent<MainCamera>().moveScroll;
           
            if (move < 0)//down
            {
                if (offY > -((int)(people/5-2) * 300.0f + 50))
                {
                    offY += move;
                }
            }
            else if (move > 0)//up
            {
                offY += move;
                if (offY > 9)
                    offY = 10;
            }
        }

        //랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
        RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texScrollView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
		
		GL.Clear(true, true, new Color(0, 0, 1, 0.5f));

		//for (int i=0; i<15; i++)
		//{
		//	GUI.Button(new Rect(offX, offY + 55 * i, 150, 50), names[i]);
		//}
		//svDt += dt;

        for (int i = 0; i < people; i++)
        {
            if (pEvent.pState[i] == null)
            {
                break;
            }
            if (GUI.Button(new Rect(offX, offY + 60 * i, 150, 50), pEvent.pState[i].name))
            {
                if (curidx == i)
                {
                    curidx = -1;
                    break;
                }
                curidx = i;                
            }
        }

        RenderTexture.active = bk;
		GUI.matrix = bkM;
	}
}

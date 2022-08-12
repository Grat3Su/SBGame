using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFIX : iGUI
{
	public Event pEvent;
	bool[] btn;
	string[] btnTxt;
	int idx;
	int x;
	int y;
	int startp;
	// Start is called before the first frame update
	void Start()
	{
		setProject();
		btn = new bool[100];
		for (int i = 0; i < 100; i++)
			btn[i] = false;
		btnTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };

		idx = 0;
		startp = 0;
	}


	// Update is called once per frame
	void Update()
	{

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

		x = Screen.width;
		//x = 10;
		y = Screen.height;
		for (int i = startp; i < startp + 5; i++)
		{
			if (pEvent.pState[i] == null)
			{
				break;
			}
			if (GUI.Button(new Rect(x - 100, 10 + 60 * i, 80, 50), pEvent.pState[i].name))
			{
				if (btn[i] == false) btn[i] = true;
				else if (btn[i] == true) btn[i] = false;
			}
			if (btn[i])
			{
				idx = pEvent.pState[i].job;
				if (GUI.Button(new Rect(x - 230, 10 + 60 * i, 100, 50), btnTxt[pEvent.pState[i].job]))
				{
					idx++;
					if (idx > btnTxt.Length - 1)
						idx = 0;
					pEvent.pState[i].jobUpdate(idx);
					//btn[i] = false;
				}
			}
		}

		if(pEvent.storage.getStorage(0) > 6)
			if(GUI.Button(new Rect(x - 80, y - 50, 50, 50), ">"))
				startp += 5;
		if (pEvent.storage.getStorage(0) > startp+6)
			if (GUI.Button(new Rect(x - 200, y - 50, 50, 50), "<"))
				startp -= 5;
	}

	float offX, offY;
	string[] names;
	float svDt;

	void updateScrollView(float dt)
	{
		if( texScrollView==null )
		{
			texScrollView = new RenderTexture(200, 500, 32, RenderTextureFormat.ARGB32);
			names = new string[] {
				"name 0", "name 1", "name 2", "name 3", "name 4",
				"name 5", "name 6", "name 7", "name 8", "name 9" };
			offX = (texScrollView.width - 150)/2;
			offY = 10;
			svDt = 0.0f;
		}

		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용?
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texScrollView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
		
		GL.Clear(true, true, new Color(0, 0, 1, 0.5f));

		for (int i=0; i<10; i++)
		{
			GUI.Button(new Rect(offX, offY + 55 * i, 150, 50), names[i]);
		}
		svDt += dt;
		offY = 10 -500 * Mathf.Abs(Mathf.Sin(45 * svDt*Mathf.Deg2Rad));

		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFIX : iGUI
{
	public Event pEvent;
	string[] btnTxt;
	string[] stateTxt;
	int idx;
	int x;
	int y;
	int curidx;
	// Start is called before the first frame update
	void Start()
	{
		init();

		setProject();
		btnTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };
		stateTxt = new string[] { "움직임", "공격", "일", "병" };

		idx = 0;
		curidx = -1;
	}


	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			if (curidx > -1)
				curidx = -1;
	}

	RenderTexture texScrollView;
	RenderTexture texinfoView;
	int prevFrameCount;

	private void OnGUI()
	{
		setProject();

		updateScrollView();
		GUI.DrawTexture(new Rect(20, 100, texScrollView.width, texScrollView.height), texScrollView);


		if (curidx > -1)
		{
			updateInfo();
			GUI.DrawTexture(new Rect((MainCamera.devWidth - texinfoView.width) / 2, 100, 
							texinfoView.width, texinfoView.height), texinfoView);			
		}

		setRGBA(0, 0, 0, 0.8f);
		fillRect(0, 0, MainCamera.devWidth, 60);

		setRGBA(1, 1, 1, 1);
		fillRect(5, 5, 40, 40);
		//보유량 적기
		
	}

	float offX, offY, maxY;
	string[] names;
	float move;
	void updateScrollView()
	{
		if (texScrollView == null)
		{//텍스쳐
			texScrollView = new RenderTexture(200, 500, 32, RenderTextureFormat.ARGB32);

		}
		offX = (texScrollView.width - 150) / 2;
		offY = 10;
		maxY = 10;
		//스크롤 최대 최소값 설정
		int people = pEvent.storage.getStorage(0);
		float minY;
		minY = texScrollView.height - 60 * (people - 1);
		if (Camera.main.GetComponent<MainCamera>().scroll)
		{
			offY += Camera.main.GetComponent<MainCamera>().moveScroll;
			if (offY < minY)
				offY = minY;
			else if (offY > maxY)
				offY = maxY;
			//Debug.Log(offY);
		}

		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texScrollView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, new Color(0, 0, 1, 0.5f));

		for (int i = 0; i < people; i++)
		{
			if (pEvent.pState[i] == null)
			{
				break;
			}
			if (drawButton(new Rect(offX, offY + 60 * i, 150, 50), pEvent.pState[i].name))
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

	void updateInfo()
	{
		if (texinfoView == null)
		{//텍스쳐
			texinfoView = new RenderTexture(700, 400, 32, RenderTextureFormat.ARGB32);

		}
		offX = 10;
		offY = 10;
		maxY = 10;
		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texinfoView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, new Color(0, 0, 0, 0.5f));

		idx = pEvent.pState[curidx].job;

		setRGBA(1, 1, 1, 1);

		fillRect(50, 50, 300, 300);

		Texture people = pEvent.pState[curidx].jobTex;
		iPoint p = new iPoint(50, 50);
		
		drawImage(people, p, 300.0f / people.width, 300.0f / people.height, LEFT|HCENTER);

		setStringRGBA(1, 1, 1, 1);
		setStringSize(50);
		p = new iPoint(texinfoView.width / 2 + 50, texinfoView.height / 2 - 150);
		drawString(pEvent.pState[curidx].name, p.x, p.y, LEFT | HCENTER);
		setStringSize(30);
		drawString("상태 : " + stateTxt[pEvent.pState[curidx].behave], p.x, p.y + 100, LEFT | HCENTER);

		// (MainCamera.devWidth - texinfoView.width) / 2, 100
		if (drawButton(new Rect(p.x, p.y + 200, 150, 50), btnTxt[pEvent.pState[curidx].job]))
		{
			idx++;
			if (idx > btnTxt.Length - 1)
				idx = 0;
			pEvent.pState[curidx].jobUpdate(idx);
		}
		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}


}

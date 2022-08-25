using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFIX : iGUI
{
	public Event pEvent;
	string[] btnTxt;
	string[] stateTxt;
	int idx;
	int curidx;

	Vector3 prevV;
	public float moveScroll;
	bool drag;
	public bool scroll;
	bool pclick;
	bool click;
	iPoint mousePos;

	void Start()
	{
		init();

		setProject();
		btnTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };
		stateTxt = new string[] { "움직임", "공격", "일", "병" };

		idx = 0;
		curidx = -1;
		pclick = false;
		mousePos = new iPoint(0, 0);
		moveSView = -300;
		maxMoveSView = 20;

		drag = false;
		scroll = false;
		moveScroll = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			if (curidx > -1)
				curidx = -1;

		//드래그
		if (Input.GetMouseButtonDown(0))
		{
			drag = true;

			prevV = Input.mousePosition;
			mousePos = MainCamera.mousePosition();
			if(touchCheck(new Rect(moveSView, 100, texScrollView.width, texScrollView.height), mousePos))
			{
				scroll = true;
			}

			if (mousePos.x > 0 && mousePos.x < 150 && mousePos.y > 0 && mousePos.y < 60)
			{
				pclick = pclick == true ? false : true;
				if (curidx > -1)
					curidx = -1;
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			mousePos = MainCamera.mousePosition();
			if (mousePos.x > 20 && mousePos.x < 220 && mousePos.y > 100 && mousePos.y < 600)
			{
				scroll = true;
				moveScroll = Input.GetAxis("Mouse ScrollWheel") * 150;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			click = false;
			mousePos = new iPoint(0, 0);
			drag = false;
			scroll = false;
		}

		if (drag)
		{
			moveScroll = 0;
			Vector3 v = Input.mousePosition;
			if (prevV == v)
			{
				return;
			}

			if (scroll)
			{
				moveScroll = prevV.y - v.y;
			}
			prevV = v;
		}

		//버늩 키처리
	}

	RenderTexture texScrollView;
	RenderTexture texpinfoView;
	RenderTexture resourceView;
	int prevFrameCount;
	float moveSView;
	float maxMoveSView;
	float move;

	private void OnGUI()
	{
		setProject();

		updateResourceView();
		updateScrollView();
		if (pclick == true)
		{
			moveSView += Time.deltaTime * 80;
			if (moveSView > maxMoveSView)
			{
				moveSView = maxMoveSView;
			}
		}
		else if (pclick == false)
		{
			moveSView -= Time.deltaTime * 80;
			if (moveSView < -300)
			{
				moveSView = -300;
			}
		}
		
		GUI.DrawTexture(new Rect(moveSView, 100, texScrollView.width, texScrollView.height), texScrollView);

		if (curidx > -1)
		{
			updatePeopleInfo();
			GUI.DrawTexture(new Rect((MainCamera.devWidth - texpinfoView.width) / 2, 100,
							texpinfoView.width, texpinfoView.height), texpinfoView);
		}
	}

	void updateinfo(float dt)
	{

	}

	//자원 보이기
	void updateResourceView()
	{
		//자원 보유량 적기
		setRGBA(0, 0, 0, 0.5f);
		fillRect(0, 0, MainCamera.devWidth, 60);

		setRGBA(1, 1, 1, 1);
		fillRect(5, 5, 40, 40);
		iPoint p;
		p.x = 5;
		p.y = 10;
		setStringSize(20);
		for (int i = 0; i < 4; i++)
		{
			drawString(pEvent.storage.getStorageText(i), 60 + i * 150, 15, RIGHT | HCENTER);
			string[] texname = new string[] { "people", "meat", "lab", "map" };
			Texture resource = Resources.Load<Texture>(texname[i]);
			p.x = 5 + i * 150;
			drawImage(resource, p, 40.0f / resource.width, 40.0f / resource.height, LEFT | HCENTER);
		}
	}

	float offX, offY, maxY;
	string[] names;
	void updateScrollView()
	{
		if (texScrollView == null)
		{//텍스쳐
			texScrollView = new RenderTexture(200, 500, 32, RenderTextureFormat.ARGB32);
			offX = (texScrollView.width - 150) / 2;
			offY = 10;
			maxY = 10;
		}

		//스크롤 최대 최소값 설정
		int people = pEvent.storage.getStorage(0);
		float minY;
		minY = texScrollView.height - 60 * (people - 1) - 10;
		if (scroll)
		{
			offY += moveScroll;
			if (offY < minY)
				offY = minY;
			else if (offY > maxY)
				offY = maxY;
		}

		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texScrollView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, new Color(0, 0, 0, 0.5f));

		for (int i = 0; i < people-1; i++)
		{
			if (pEvent.pState[i] == null)
			{
				break;
			}

			setStringRGBA(0, 0, 0, 1);

			if (curidx != i)
				setRGBA(1, 1, 1, 1);
			else// if( curidx==i )
				setRGBA(0.3f, 0.3f, 0.3f, 1);

			Rect r = new Rect(offX, offY + 60 * i, 150, 50);
			createButton(pEvent.pState[i].name, r);

			if (mousePos.x != 0 && mousePos.y != 0)
			{
				r = new Rect(moveSView + offX, 100 + offY + 60 * i, 150, 50);
				if (touchCheck(r, mousePos) && !click)//이미지 처리
				{
					click = true;
					curidx = i;
				}
			}
		}
		GUI.color = Color.white;
		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}

	void createButton(string str, Rect r)
	{
		fillRect(r);
		drawString(str, new iPoint(r.x + r.width / 2, r.y + r.height / 2), VCENTER | HCENTER);
	}

	bool touchCheck(Rect r, iPoint p)
	{
		if (r.x < p.x && r.x + r.width > p.x && r.y < p.y && r.y + r.height > p.y)
			return true;

		return false;
	}

	void updatePeopleInfo()
	{
		if (texpinfoView == null)
		{//텍스쳐
			texpinfoView = new RenderTexture(700, 400, 32, RenderTextureFormat.ARGB32);
		}
		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texpinfoView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, new Color(0, 0, 0, 0.5f));

		idx = pEvent.pState[curidx].job;

		setRGBA(1, 1, 1, 1);

		fillRect(50, 50, 300, 300);

		Texture people = pEvent.pState[curidx].jobTex;
		iPoint p = new iPoint(50, 50);
		drawImage(people, p, 300.0f / people.width, 300.0f / people.height, LEFT | HCENTER);

		float stringSize = getStringSize();
		setStringSize(50);
		p = new iPoint(texpinfoView.width / 2 + 50, texpinfoView.height / 2 - 150);
		drawString(pEvent.pState[curidx].name, p.x, p.y, LEFT | HCENTER);
		setStringSize(stringSize);
		drawString("상태 : " + stateTxt[pEvent.pState[curidx].behave], p.x, p.y + 100, LEFT | HCENTER);

		// (MainCamera.devWidth - texinfoView.width) / 2, 100
		Rect r = new Rect(p.x, p.y + 200, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		createButton(btnTxt[pEvent.pState[curidx].job], r);

		//MainCamera.devWidth - texinfoView.width) / 2, 100 : 위치
		r.x = (MainCamera.devWidth - texpinfoView.width) / 2 + p.x;
		r.y = 300 + p.y;

		if (!click)
		{
			if (touchCheck(r, mousePos))
			{
				idx++;
				if (idx > btnTxt.Length - 1)
					idx = 0;
				pEvent.pState[curidx].jobUpdate(idx);
				click = true;
			}
		}
		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}
}

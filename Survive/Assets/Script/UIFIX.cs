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
		
	void Start()
	{
		init();

		setProject();
		btnTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };
		stateTxt = new string[] { "움직임", "공격", "일", "병" };

		idx = 0;
		curidx = -1;
		pclick = false;
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
			iPoint p = MainCamera.mousePosition();

			if (p.x > 20 && p.x < 220 && p.y > 100 && p.y < 600)
			{
				scroll = true;
			}

			iPoint mouse = MainCamera.mousePosition();
			if (mouse.x > 0 && mouse.x < 150 && mouse.y > 0 && mouse.y < 60)
			{
				pclick = pclick == true ? false : true;
				if(curidx >-1)
				curidx = -1;
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			iPoint p = MainCamera.mousePosition();
			if (p.x > 20 && p.x < 220 && p.y > 100 && p.y < 600)
			{
				scroll = true;
				moveScroll = Input.GetAxis("Mouse ScrollWheel") * 150;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
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
	RenderTexture texinfoView;
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
			updateInfo();
			GUI.DrawTexture(new Rect((MainCamera.devWidth - texinfoView.width) / 2, 100, 
							texinfoView.width, texinfoView.height), texinfoView);			
		}


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
		paint();
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

		GL.Clear(true, true, new Color(0, 0, 1, 0.5f));

		for (int i = 0; i < people; i++)
		{
			if (pEvent.pState[i] == null)
			{
				break;
			}

			setStringRGBA(0, 0, 0, 1);
			drawLabel(new Rect(offX, offY + 60 * i, 150, 50), pEvent.pState[i].name, VCENTER|HCENTER);
			
			//이미지 처리
			iPoint p = new iPoint(offX, offY + 60 * i);

			//if (drawButton(new Rect(offX, offY + 60 * i, 150, 50), pEvent.pState[i].name))
			//{
			//	if (curidx == i)
			//	{
			//		curidx = -1;
			//		break;
			//	}
			//	curidx = i;
			//}
			imgButton[i].paint(Time.deltaTime, new iPoint(offX, offY + 60 * i));
		}

		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}

	iImage[] imgButton;
	iStrTex[][] stButton;
	void createButton(int people)
	{
		imgButton = new iImage[people];

		stButton = new iStrTex[people][];
		for(int i=0; i<people; i++)
		{
			imgButton[i] = new iImage();

			stButton[i] = new iStrTex[2];
			for(int j=0; j<2; j++)
			{
				stButton[i][j] = new iStrTex(MethodSt, 150, 50);
				stButton[i][j].setString(j + "\n" + pEvent.pState[i].name);

				imgButton[i].add(stButton[i][j].tex);
			}
		}
	}
	void MethodSt(iStrTex st)
	{
#if true// #issue
		if (st.tex.tex != st.texReserve)
			((RenderTexture)st.tex.tex).Release();
#endif
		st.tex.tex = st.texReserve;

		RenderTexture bkT = RenderTexture.active;
		RenderTexture.active = (RenderTexture)st.tex.tex;
		//Rect bkR = Camera.main.rect;
		//Camera.main.rect = new Rect(0, 0, 1, 1);
		Matrix4x4 matrixBk = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(
			Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, Color.clear);// add

		string sn = getStringName();
		float ss = getStringSize();
		Color sc = getStringRGBA();
		setStringName("BM-JUA");
		setStringSize(50);
		setStringRGBA(1, 1, 1, 1);

		iSize size = new iSize(st.wid, st.hei);

		// 0\n게임시작 => "0", "게임시작"
		string[] strs = st.str.Split('\n');
#if false
		char[] c = strs[0].ToCharArray();
		int index = c[0] - '0';
		Debug.Log("index = "+ index);
#else
		int index = int.Parse(strs[0]);
#endif

#if false// custumizeing
		if (index == 0) setRGBA(0, 0, 0, 0.5f);
		else setRGBA(0.5f, 0.5f, 0.5f, 0.5f);
		fillRect(0, 0, size.width, size.height);

		setLineWidth(3);
		if (index == 0) setRGBA(1, 1, 1, 1);
		else setRGBA(1, 1, 0, 1);
		drawRect(1, 1, size.width - 2, size.height - 2);

		drawString(strs[1], size.width / 2, size.height / 2, VCENTER | HCENTER);

		setStringName(sn);
		setStringSize(ss);
		setStringRGBA(sc.r, sc.g, sc.b, sc.a);

		string path = "defense0" + (1 + 2 * index);
		Texture t = Resources.Load<Texture>(path);
		drawImage(t, 0, 0, TOP | LEFT);
		//drawImage(t, size.width, 0, TOP | RIGHT);
		drawImage(t, size.width, 0, 1f, 1f, TOP | RIGHT, 2, 0, REVERSE_WIDTH);
#endif
		RenderTexture.active = bkT;
		//Camera.main.rect = bkR;
		GUI.matrix = matrixBk;
	}

	public List<iImage> listImg;
	RenderTexture btnTexture;
	void paint()
	{
		if(btnTexture == null)
		{
			texScrollView = new RenderTexture(150,50, 32, RenderTextureFormat.ARGB32);
		}
		listImg = new List<iImage>();

		RenderTexture bkT = RenderTexture.active;
		RenderTexture.active = texScrollView;
		Matrix4x4 matrixBk = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(
			Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		iPoint p = new iPoint(0,0);
		iGUI.instance.drawImage(texScrollView, p.x, p.y, iGUI.TOP | iGUI.LEFT);
		
		RenderTexture.active = bkT;
		//Camera.main.rect = bkR;
		GUI.matrix = matrixBk;

	}

	void updateInfo()
	{
		if (texinfoView == null)
		{//텍스쳐
			texinfoView = new RenderTexture(700, 400, 32, RenderTextureFormat.ARGB32);
		}
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

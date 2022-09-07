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
			if(pEvent.storage.getStorage(0)>1)
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
	}

	RenderTexture texScrollView;
	RenderTexture texpinfoView;
	RenderTexture texNextView;
	RenderTexture resourceView;
	int prevFrameCount;
	float moveSView;
	float maxMoveSView;
	float move;

	private void OnGUI()
	{
		setProject();
        setStringRGBA(0, 0, 0, 1);
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
		if (pEvent.newday)
		{
            updateNextDay();
			drawTexture(new Rect(100, 100, texNextView.width, texNextView.height), texNextView);
		}

		if(moveSView >-300)
			drawTexture(new Rect(moveSView, 100, texScrollView.width, texScrollView.height), texScrollView);

		if (curidx > -1&& pclick ==true)
		{
			updatePeopleInfo();
			drawTexture(new Rect((MainCamera.devWidth - texpinfoView.width) / 2, 100,
							texpinfoView.width, texpinfoView.height), texpinfoView);
		}
	}

	void updateNextDay()//다음날
	{
		if (texNextView == null)
			texNextView = new RenderTexture(MainCamera.devWidth-200, MainCamera.devHeight - 200, 32, RenderTextureFormat.ARGB32);

		//랜더 텍스쳐는 따로 그리기 때문에 고유한 매트릭스 사용
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = texNextView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, new Color(0, 0, 0, 0.5f));		
		
		setRGBA(1, 1, 1, 1);
		setStringRGBA(1, 1, 1, 1);

		float fontsize = getStringSize();
		setStringSize(100);

		iPoint p = new iPoint(texNextView.width / 2, texNextView.height/2 - 100);
		int strData = pEvent.day;
		drawString(strData + "일차", p, VCENTER | HCENTER);

		string[] texname = new string[] { "people", "food", "lab", "map" };
		
		setStringSize(30);
		for (int i = 0; i<4; i++)		
		{			
			if (i == 0)
			{
				strData = pEvent.getDay.people;
			}
			else if (i == 1)
			{
				strData = pEvent.getDay.food;
			}
			else if (i == 2)
			{
				strData = pEvent.getDay.labExp;
			}
			else if (i == 3)
			{
				strData = pEvent.getDay.stageExp;
			}

			if(strData > 0)
				setStringRGBA(0, 1, 0, 1);

			else if (strData < 0)
				setStringRGBA(1, 0, 0, 1);
			else
				setRGBA(1, 1, 1, 1);

			p = new iPoint(texNextView.width / 4.0f * (i+1) - 150, texNextView.height / 2 + 150);
			drawString(strData.ToString(), p, VCENTER | HCENTER);

			p.y = texNextView.height / 2 + 100;			
			Texture resource = Resources.Load<Texture>(texname[i]);
			p.x = texNextView.width / 4.0f * (i + 1) - 150;
			drawImage(resource, p, 50.0f / resource.width, 50.0f / resource.height, VCENTER | HCENTER);
		}

		setStringSize(fontsize);
		setStringRGBA(1, 1, 1, 1);
		GUI.color = Color.white;
		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}

	float alpha;

	void updateinfo(int type)//상세정보
	{
		if (resourceView == null)
			resourceView = new RenderTexture(300, 100, 32, RenderTextureFormat.ARGB32);
		Color c = getStringRGBA();
		RenderTexture bk = RenderTexture.active;
		RenderTexture.active = resourceView;
		Matrix4x4 bkM = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
		
		GL.Clear(true, true, new Color(0, 0, 0, 0.5f));
		setStringRGBA(1, 1, 1, 1);
		drawString(pEvent.storage.getStorageText(type), resourceView.width/2, 30, VCENTER | HCENTER);

		setStringRGBA(c.r, c.g, c.b, 1);
		GUI.color = Color.white;
		RenderTexture.active = bk;
		GUI.matrix = bkM;
	}

	//자원 보이기
	void updateResourceView()
	{
		//자원 보유량 적기
		setRGBA(0, 0, 0, 0.8f);
		fillRect(0, 0, MainCamera.devWidth, 60);
		if(pEvent.specialEvent!=0)
		fillRect(MainCamera.devWidth - 160, 60, 150, 50);

		setRGBA(1, 1, 1, 1);
		setStringRGBA(1, 1, 1, 1);
		//fillRect(5, 5, 40, 40);
		iPoint p;
		p.x = 5;
		p.y = 10;
		setStringSize(20);
		for (int i = 0; i < 4; i++)
		{
			drawString(pEvent.storage.getStorageText(i), 60 + i * 150, 15, RIGHT | HCENTER);
			string[] texname = new string[] { "people", "food", "lab", "map" };
			Texture resource = Resources.Load<Texture>(texname[i]);
			p.x = 5 + i * 150;
			drawImage(resource, p, 40.0f / resource.width, 40.0f / resource.height, LEFT | HCENTER);
		}
		if (pEvent.specialEvent != 0)
		{
			string[] spEvent = new string[] { "test", "습격", "병", "내분", "독립요구" };
			setStringRGBA(1, 0, 0, 1);
			drawString(spEvent[pEvent.specialEvent], (MainCamera.devWidth - 85), 85, VCENTER | HCENTER);
			setStringRGBA(1, 1, 1, 1);
		}
		p = new iPoint(60 + 3 * 150, 0);

		if (touchCheck(new Rect(5 + 3 * 150, p.y, 130,60), MainCamera.mousePosition()))
		{
			updateinfo(5);
			drawTexture(new Rect(5 + 3 * 130, 80, 
							resourceView.width, resourceView.height), resourceView);
		}
		else if (touchCheck(new Rect(5 + 2 * 150, 0, 130,60), MainCamera.mousePosition()))
		{
			updateinfo(4);
			drawTexture(new Rect(5 + 2 * 130, 80, resourceView.width, resourceView.height), 
								resourceView);
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

		GL.Clear(true, true, new Color(1, 1, 1, 1));

		idx = pEvent.pState[curidx].job;

		setRGBA(1, 1, 1, 1);
		fillRect(50, 50, 300, 300);

		setRGBA(0, 0, 0, 0.5f);
		fillRect(texpinfoView.width/2, 0, 300, 900);
		setRGBA(1, 1, 1, 1);
		Texture people = pEvent.pState[curidx].jobTex;
		iPoint p = new iPoint(50, 50);
		drawImage(people, p, 300.0f / people.width, 300.0f / people.height, LEFT | HCENTER);

		float stringSize = getStringSize();
        PeopleState prople = pEvent.pState[curidx];
		setStringSize(50);
		p = new iPoint(texpinfoView.width / 2 + 50, texpinfoView.height / 2 - 150);
		drawString(prople.name, p.x, p.y, LEFT | HCENTER);
		//setStringRGBA(1, 1, 1, 1);
		setStringSize(stringSize);
		if(prople.job != 0)
        drawString("레벨 : " + prople.jobLevel[prople.job], p.x, p.y+70, LEFT | HCENTER);
        drawString("상태 : " + stateTxt[prople.behave], p.x, p.y + 100, LEFT | HCENTER);

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

        setRGBA(1, 0, 0, 1);
        fillRect(texpinfoView.width-60, 10, 50,50);

        //(new Rect((MainCamera.devWidth - texpinfoView.width) / 2, 100,                            texpinfoView.width, texpinfoView.height), texpinfoView);
        r = new Rect((MainCamera.devWidth - texpinfoView.width) / 2 + (texpinfoView.width - 60), 110, 50, 50);
        setStringSize(50);
        setStringRGBA(0, 0, 0, 1);
        drawString("X", new iPoint(texpinfoView.width - 60+25, 10+25), VCENTER | HCENTER);

        if (touchCheck(r, mousePos))
        {
            curidx = -1;
            Debug.Log("click");
        }
        GUI.color = Color.white;
        RenderTexture.active = bk;
		GUI.matrix = bkM;
	}
}

#if false




수색
- ?명 수색
- ?명 발견 / 구함
- 사망 수, 탄약 소모,  



기습

- ?놈의 무리가 기습
- ?명 사망 ?명 치명적 상해 => 의약품 ?개 소모, 무기 탄약 ?개 소모





연구실적

- 레벨마다 요구 연구양
- 1사람마다 연구 실적 발생
int getExp = 20;

int labNeedExp[] = { 100, 150, 200, 280, .... };
int labLv = 0;
int labExp = 0;

labExp += getExp * person;

 "연구실적 ?명 연구 달성량 getExp * person : labExp  / labNeedExp[labLv]"
 "연구실적 ?명 연구 달성량 getExp * person :  레벨3달성 labExp  / labNeedExp[labLv]"

 "연구실적이 오르면 식량 구할 확률이 올라갑니다."


 최종
 총 물자 어떻게 변경되었는지 

 사람 +- ?명
 물 +=
 ? +=
 클릭을 해서 새로운 하루 시작합니다!!


#endif


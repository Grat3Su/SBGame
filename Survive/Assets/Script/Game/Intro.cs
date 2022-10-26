using STD;
using System.Data;
using UnityEditor.Timeline;
using UnityEngine;

public class Intro : gGUI
{
	public override void load()
	{
		// logo or symbol (fade in/out)

		// title (상용게임 처럼 버전표시:메일)
		// press any key (on / off)

		// menu popup
		// start, opt, howtoplay, exit

		// 저장 슬롯 ?
		// 진짜 시작..새로...

		// opt - 창모드/전체화면
		// sfx
		// bgm		

		// howto
		// ctrller(pad/keyboard/mouse)
		// stroy 세계관 / 생존 부연설명
		// ===> 나의 대한 얘기

		// 정말종료?

		loadTitle();
		loadMenu();
		loadSetting();
		loadH2P();
	}

	public override void free()
	{
		MainCamera.destroyMethodMouse(key);
	}

	public override void draw(float dt)
	{
		drawBG();
		drawTitle(dt);
		drawMenu(dt);
		drawSetting(dt);
		drawH2P(dt);
	}

	public override bool key(iKeystate stat, iPoint point)
	{
		if (keySetting(stat, point))
			return false ;

        if (stat == iKeystate.Began)
        {
            Debug.Log(point.x + ", " + point.y);

            if (splitNum < 7)
            {
                splitNum = 7;
                titleOffy = 200;
            }
        }
		keyPopMenu(stat, point);
		keyPopH2P(stat, point);

		return false;
	}

	iPopup popTitle = null;
	iStrTex stTitle;
	void loadTitle()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStTitle, MainCamera.devWidth, 500);
		// st.setString imgPersonInfoBtn 아직 생성안함]
#if true
		splitNum = 0;
		titleOffy = 0;
		titledt = 0.5f;
		st.setString(splitNum + " S U R V I V E");
#endif
		img.add(st.tex);
		pop.add(img);
		title_dt = 0.5f;
		stTitle = st;

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(MainCamera.devWidth / 2 - st.wid / 2, MainCamera.devHeight / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth / 2 - st.wid / 2, 10);
		pop._aniDt = 0.1f;
		popTitle = pop;

		pop.show(true);
	}

	float titledt;
	int splitNum;
	float titleOffy;
	void methodStTitle(iStrTex st)
	{
		string[] strs = st.str.Split(" ");
		string s = " ";
		for (int i = 0; i < splitNum; i++)
			s += strs[i + 1];
		setStringName("BMJUA_ttf");

		setRGBA(0, 0, 0, 1);
		setStringSize(180);
		drawString(s, new iPoint(st.wid / 2, st.hei / 2 + 100 - titleOffy), VCENTER | HCENTER);
		//Debug.Log(s);
		//setStringSize(50);
		//350,200
	}

	float title_dt;
	void drawTitle(float dt)
	{
		if (!popTitle.bShow)
			return;

		if (titleOffy < 200)
			titledt += dt;
		else if (!popMenu.bShow)
		{
			popMenu.show(true);
		}

		if (titledt > title_dt)
		{
			titledt = 0;
			if (splitNum < 7)
				splitNum++;
			else
			{
				if (title_dt > 0.05f)
					title_dt = 0.05f;
				titleOffy += 300 * dt * 5;
			}
		}
		stTitle.setString(splitNum + " S U R V I V E " + titleOffy);
		popTitle.paint(dt);
	}

	void drawBG()
	{
		setRGBA(1, 1, 1, 1);
		//fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);

		//Util.createTexture("IntroBG");
		setStringName("BMJUA_ttf");
		drawImage(Util.createTexture("IntroBG"), new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2), (float)MainCamera.devWidth / Util.createTexture("IntroBG").width, (float)MainCamera.devHeight / Util.createTexture("IntroBG").height, VCENTER | HCENTER);
		setStringSize(30);
		drawString("------@gmail.com", new iPoint(50, MainCamera.devHeight - 50), LEFT | HCENTER);
	}

	iPopup popMenu = null;

	iStrTex stMenu;
	iImage[] imgMenuBtn;
	iStrTex[][] stMenuBtn;
	void loadMenu()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStMenu, MainCamera.devWidth, MainCamera.devHeight);
		// st.setString imgPersonInfoBtn 아직 생성안함
		img.add(st.tex);
		pop.add(img);
		stMenu = st;

		imgMenuBtn = new iImage[4]; // 0 : start / 1 : h2p / 2: Quit
		stMenuBtn = new iStrTex[4][];//눌렸을 때
		setStringSize(30);

		for (int i = 0; i < 4; i++)
		{
			stMenuBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//닫기 : 0
				if (i == 0)
				{
					st = new iStrTex(methodStMenuBtn, 200, 150);
					st.setString(j + "\n" + " Start " + "\n" + i);
				}
				//직업 바꾸기 : 1
				else if (i == 1)
				{
					st = new iStrTex(methodStMenuBtn, 200, 150);
					st.setString(j + "\n" + "How to Play" + "\n" + i);
				}
				else if (i == 2)
				{
					st = new iStrTex(methodStMenuBtn, 200, 150);
					st.setString(j + "\n" + "Option" + "\n" + i);
				}
				else
				{
					st = new iStrTex(methodStMenuBtn, 200, 150);
					st.setString(j + "\n" + "Quit" + "\n" + i);
				}
				img.add(st.tex);

				stMenuBtn[i][j] = st;
			}
			if (i == 0)
				img.position = new iPoint(200, 450);
			else if (i == 1)
				img.position = new iPoint(450, 450);
			else if (i == 2)
				img.position = new iPoint(700, 450);
			else if (i == 3)
				img.position = new iPoint(950, 450);
			imgMenuBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2);
		pop.closePoint = new iPoint(0, 0);
		pop._aniDt = 0.5f;
		popMenu = pop;
	}
	public void methodStMenu(iStrTex st)
	{
		setRGBA(1, 1, 1, 1);
		for (int i = 0; i < 4; i++)
		{
			imgMenuBtn[i].frame = (popMenu.selected == i ? 1 : 0);
			imgMenuBtn[i].paint(0.0f, new iPoint(0, 0));
		}
	}

	public void methodStMenuBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		int bindex = int.Parse(strs[2]);
		string s = strs[1];

		iPoint pos = new iPoint(0, 0);

		setStringName("BMJUA_ttf");
		if (index == 0)
		{
			setRGBA(1, 1, 1, 1);
			setStringRGBA(0, 0, 0, 1);
		}

		else if (index > 0)
		{
			setRGBA(0.3f, 0.3f, 0.3f, 1);
		}

		int w = st.tex.tex.width;
		int h = st.tex.tex.height;

		//fillRect(0, 0, w, h);
		string[] texname = new string[] { "player", "explorer", "researcher", "jobless" };

		drawString(s, w / 2, 10, VCENTER | HCENTER);
		pos.x = w / 2;
		pos.y = h / 2;
		Texture tex = Util.createTexture(texname[bindex]);
		drawImage(tex, pos, 100.0f / tex.width, 100.0f / tex.height, VCENTER | HCENTER);
	}

	void drawMenu(float dt)
	{
		stMenu.setString(popMenu.selected + "");
		popMenu.paint(dt);
	}

	bool keyPopMenu(iKeystate stat, iPoint point)
	{
		if (popMenu.bShow == false)
			return false;

		iPoint p;
		p = popMenu.closePoint;
		iSize s = new iSize(0, 0);
		int i = 0;
		switch (stat)
		{
			case iKeystate.Began:
				for (i = 0; i < imgMenuBtn.Length; i++)
				{
					if (imgMenuBtn[i].touchRect(p, s).containPoint(point))
						popMenu.selected = i;
				}
				break;
			case iKeystate.Moved:
				i = popMenu.selected;
				if (i == -1) break;
				if (imgMenuBtn[i].touchRect(p, s).containPoint(point) == false)
				{
					//Debug.Log("음:취소");

					popMenu.selected = -1;
				}
				break;
			case iKeystate.Ended:
				i = popMenu.selected;

				switch (i)
				{
					case 0:
						Main.me.reset("Proc");
						break;
					case 1:
						Debug.Log("h2p");
						popH2P.show(true);
						break;
					case 2:
						Debug.Log("option");
						popSetting.show(true);
						break;
					case 3:
						Debug.Log("quit");
						Application.Quit();
#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
#endif
						break;
				}
				popMenu.selected = -1;
				break;

		}
		return false;
	}

	iPopup popSetting = null;
	iStrTex stSetting;
	void loadSetting()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStSetting, 500, MainCamera.devHeight - 100);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stSetting = st;

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(800, 500);
		pop.closePoint = new iPoint((MainCamera.devWidth - 500) / 2, 50);
		pop._aniDt = 0.5f;
		popSetting = pop;
	}
	
	void methodStSetting(iStrTex st)
	{
		setRGBA(1, 1, 1, 1);
		fillRect(0, 0, 500, MainCamera.devHeight - 100);
	}

	void drawSetting(float dt)
	{
		popSetting.paint(dt);
	}

	bool keySetting(iKeystate stat, iPoint point)
	{
		if(!popSetting.bShow)
			return false;
		
		iPoint p;
		p = popH2P.closePoint;
		iSize s = new iSize(0, 0);

		if (stat == iKeystate.Began)
		{
			popSetting.show(false);
		}
		else if(stat == iKeystate.Moved)
		{

		}
		else if(stat == iKeystate.Ended)
		{

		}
		return true;
	}

	iPopup popH2P = null;
	iStrTex stH2P;
	iImage[] imgH2PBtn;
	int pageIdx;
	void loadH2P()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStH2P, 500, MainCamera.devHeight - 100);
		pageIdx = 0;
		st.setString(""+pageIdx);
		img.add(st.tex);
		pop.add(img);
		stH2P = st;

		imgH2PBtn = new iImage[3]; // 0 : start / 1 : h2p / 2: Quit

		string[] strName = new string[3] { "X", "<", ">" };
		iPoint[] pos = new iPoint[] {
			new iPoint(450, 0),
			new iPoint(-70, 550),
			new iPoint(520, 550)
		};
		for (int i = 0; i < 3; i++)
		{
			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				st = new iStrTex(methodStH2PBtn, 80, 80);
				st.setString(j + "\n" + strName[i] + "\n" + i);
				img.add(st.tex);
			}
			img.position = pos[i];
			pop.add(img);
			imgH2PBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(500, 500);
		pop.closePoint = new iPoint((MainCamera.devWidth-500)/2, 50);
		pop._aniDt = 0.5f;
		popH2P = pop;
	}

	void methodStH2P(iStrTex st)
	{
		setRGBA(1, 1, 1, 1);
		fillRect(0, 0, 500, MainCamera.devHeight - 100);

		setStringRGBA(0, 0, 0, 1);
		drawString(""+(pageIdx+1), 250, st.tex.tex.height - 50, VCENTER | HCENTER);
        setStringSize(50);
        if ( pageIdx==0 )
		{
            // Game
            drawString("How to Play?", new iPoint(250, 50), VCENTER | HCENTER);

            setStringSize(30);
            drawString("생존자들이 모두 사망하면", new iPoint(250, 100), VCENTER | HCENTER);
            drawString("게임이 끝납니다.", new iPoint(250, 150), VCENTER | HCENTER);
            drawString("생존자들의 직업,", new iPoint(250, 200), VCENTER | HCENTER);
            drawString("플레이어의 선택을", new iPoint(250, 250), VCENTER | HCENTER);
            drawString("적절히 분배해서 생존하세요!", new iPoint(250, 300), VCENTER | HCENTER);
        }
		else if (pageIdx == 1)
		{
            //Skill
            drawString("제작환경", new iPoint(250, 50), VCENTER | HCENTER);
            drawString("할수있는 언어", new iPoint(250, 200), VCENTER | HCENTER);
            drawString("쓸 수 있는 엔진", new iPoint(250, 300), VCENTER | HCENTER);

            setStringSize(30);
            drawString("Unity", new iPoint(250, 100), VCENTER | HCENTER);
            drawString("제작 기간 : 3개월", new iPoint(250, 150), VCENTER | HCENTER);

            drawString("C / C++ / C#", new iPoint(250, 250), VCENTER | HCENTER);
            drawString("Unity / Unreal / Cocos-2dx", new iPoint(250, 350), VCENTER | HCENTER);
        }
		else if (pageIdx == 2)
		{
            // Me ?
            //drawString("제작자", new iPoint(250, 100), VCENTER | HCENTER);
            //drawString("Su", new iPoint(250, 150), VCENTER | HCENTER);
            setStringSize(30);

            drawString("이 작품은 7월 한국에서 부터 시작되어", new iPoint(250, 100), VCENTER | HCENTER);
            drawString("11월까지 만들어진 작품으로", new iPoint(250, 150), VCENTER | HCENTER);
            drawString("어느날 갑자기 심즈가 땡긴 저는 게임을 하다가", new iPoint(250, 200), VCENTER | HCENTER);
            drawString("욕구 시스템을 넣은 생존 게임을 만들어보고 싶어졌습니다", new iPoint(250, 250), VCENTER | HCENTER);
            drawString("그렇게 저는 개발을 시작했고", new iPoint(250, 300), VCENTER | HCENTER);
            drawString("만들고 보니 욕구 시스템은 사라져 있었습니다", new iPoint(250, 350), VCENTER | HCENTER);
            drawString("하지만 이것에는 이유가 있었으니..", new iPoint(250, 400), VCENTER | HCENTER);
            drawString("생존 게임에 대한 레퍼런스를 찾던 중", new iPoint(250, 450), VCENTER | HCENTER);
            drawString("여러 생존 게임을 하게 되었고", new iPoint(250, 500), VCENTER | HCENTER);
            drawString("욕구 시스템을 폐기하고 생존에 치중하게 되었기 때문입니다.", new iPoint(250, 550), VCENTER | HCENTER);
        }

		for (int i = 0; i < 3; i++)
		{
			imgH2PBtn[i].frame = (popH2P.selected == i ? 1 : 0);
			imgH2PBtn[i].paint(0.0f, new iPoint(0, 0));
		}
	}

	void methodStH2PBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int click = int.Parse(strs[0]);
		int index = int.Parse(strs[2]);
		string s = strs[1];
		setStringName("BMJUA_ttf");

        float colorSet = 0.0f;
        if (click == 0)
        {
            if(index != 0)
            {
                colorSet = 1;

                if (pageIdx == 0)
                {
                    if (index == 1)//isue
                        colorSet = 0.3f;
                }
                else if (pageIdx == 2)
                {
                    if (index == 3)
                        colorSet = 0.3f;
                }
            }
        }
        else if (click == 1)
            colorSet = 0.3f;

        setRGBA(colorSet, colorSet, colorSet, 1);

        if (click == 0)
            if (index == 0)
                setRGBA(1, 0, 0, 1);

            fillRect(0, 0, 50, 50);
		setStringRGBA(0, 0, 0, 1);

		int w = st.tex.tex.width;
		int h = st.tex.tex.height;
		drawString(s, 30, 30, VCENTER | HCENTER);
	}

	void drawH2P(float dt)
	{
		stH2P.setString("" + pageIdx + "" + popH2P.selected);
		popH2P.paint(dt);
	}

	bool keyPopH2P(iKeystate stat, iPoint point)
	{
		if (!popH2P.bShow)
			return false;

		int i;
		iPoint p;
		p = popH2P.closePoint;
		iSize s = new iSize(0, 0);

		if (stat == iKeystate.Began)
		{
			for (i = 0; i < 3; i++)
			{
				if (imgH2PBtn[i].touchRect(p, s).containPoint(point))
				{
					popH2P.selected = i;
					break;
				}
			}
		}
		else if(stat == iKeystate.Moved)
		{
			popH2P.selected = -1;
			for (i = 0; i < 3; i++)
			{
				if (imgH2PBtn[i].touchRect(p, s).containPoint(point))
				{
					popH2P.selected = i;
					break;
				}
			}
		}
		else if(stat == iKeystate.Ended)
		{
			if(popH2P.selected == 0)
			{
				popH2P.selected = -1;
				popH2P.show(false);
			}
			else if(popH2P.selected == 1)
			{
				if (pageIdx != 0)
					pageIdx--;
				Debug.Log("page : " + pageIdx);
				//이전 장으로
			}
			else if(popH2P.selected == 2)
			{
				if (pageIdx != 2)
					pageIdx++;
				Debug.Log("page : " + pageIdx);
				//다음 장으로
			}
		}
		return true;
	}
}
//키보드 되게 부드럽게 잘 눌린다., 무접점 적춛ㄱ?
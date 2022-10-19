using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

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
	}

	public override void free()
	{

	}

	public override void draw(float dt)
	{
		drawBG();
		drawTitle(dt);
        drawMenu(dt);
	}

	public override bool key(iKeystate stat, iPoint point)
	{
		if (stat == iKeystate.Began)
		{
			Debug.Log(point.x + ", " + point.y);
			//Main.me.reset("Proc");
		}
        keyPopMenu(stat, point);
        

        return false;
	}

	iPopup popTitle = null;
	iStrTex stTitle;
	void loadTitle()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStTitle, MainCamera.devWidth,500);
		// st.setString imgPersonInfoBtn 아직 생성안함]
#if true
		splitNum = 0;
		titleOffy = 0;
		titledt = 0.5f;
		st.setString(splitNum + " S U R V I V E");
#endif
		img.add(st.tex);
		pop.add(img);

		//splitNum = 0;
		//titledt = 1.0f;
		//st.setString(splitNum + " S U R V I V E");
		title_dt = 0.5f;
		stTitle = st;

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(MainCamera.devWidth / 2 - st.wid / 2, MainCamera.devHeight/2);
		pop.closePoint = new iPoint(MainCamera.devWidth /2 - st.wid / 2, 10);
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
			s += strs[i+1];

		setRGBA(0, 0, 0, 1);
		setStringSize(150);
		drawString(s, new iPoint(st.wid/2, st.hei/2 + 100 - titleOffy), VCENTER | HCENTER);
		//Debug.Log(s);
		setStringSize(30);
		//350,200
	}

	float title_dt;	
	void drawTitle(float dt)
	{
		if (!popTitle.bShow)
			return;

        if (titleOffy < 150)
            titledt += dt;
        else if(!popMenu.bShow)
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
				if(title_dt > 0.05f)
					title_dt = 0.05f;
				titleOffy += 300 * dt*5;
			}
		}

		stTitle.setString(splitNum + " S U R V I V E " + titleOffy);
		popTitle.paint(dt);
	}

	void drawBG()
	{
		setRGBA(1, 1, 1, 1);
		fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);

		//Util.createTexture("IntroBG");
		drawImage(Util.createTexture("IntroBG"), new iPoint(0, 0), MainCamera.devWidth / Util.createTexture("IntroBG").width, MainCamera.devHeight / Util.createTexture("IntroBG").height, VCENTER | HCENTER);
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

		imgMenuBtn = new iImage[3]; // 0 : start / 1 : h2p / 2: Quit
		stMenuBtn = new iStrTex[3][];//눌렸을 때

		for (int i = 0; i < 3; i++)
		{
			stMenuBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//닫기 : 0
				if (i == 0)
				{
					st = new iStrTex(methodStMenuBtn, 150, 150);
					st.setString(j + "\n" + " Start " + "\n" + i);
				}
				//직업 바꾸기 : 1
				else if (i == 1)
				{
					st = new iStrTex(methodStMenuBtn, 150, 150);
					st.setString(j + "\n" + "How to Play" + "\n" + i);
				}
				else
				{
					st = new iStrTex(methodStMenuBtn, 150, 150);
					st.setString(j + "\n" + "Quit" + "\n" + i);
				}
				img.add(st.tex);

				stMenuBtn[i][j] = st;
			}
			if (i == 0)
				img.position = new iPoint(250, 400);
			else if (i == 1)
				img.position = new iPoint(500, 400);
            //img.position = new iPoint(750, 400);
            else if (i == 2)
				img.position = new iPoint(1000, 400);
			imgMenuBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(MainCamera.devWidth/2, MainCamera.devHeight/2);
		pop.closePoint = new iPoint(0,0);		
		pop._aniDt = 0.5f;
		popMenu = pop;
	}
	public void methodStMenu(iStrTex st)
	{
        setRGBA(1, 1, 1, 1);
        for (int i = 0; i < 3; i++)
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

        if (index == 0)
        {
            setRGBA(0, 0, 0, 1);
            setRGBA(0.8f, 0.8f, 0.8f, 1);
            setStringRGBA(0, 0, 0, 1);
        }

        else if (index > 0)
        {
            setRGBA(0.3f, 0.3f, 0.3f, 1);
        }

        int w = st.tex.tex.width;
        int h = st.tex.tex.height;

        //fillRect(0, 0, w, h);
        string[] texname = new string[] { "explorer", "researcher", "jobless" };
        
        drawString(s, w/2, 10, VCENTER | HCENTER);
        pos.x = w / 2;
        pos.y = h / 2;        
        Texture tex = Util.createTexture(texname[bindex]);
        drawImage(tex, pos, 100.0f/tex.width, 100.0f/tex.height, VCENTER | HCENTER);
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
                for(i = 0; i<imgMenuBtn.Length; i++)
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
                        break;
                    case 2:
                        Debug.Log("quit");
                        break;
                    case 3:
                        break;
                }
                popMenu.selected = -1;
                break;

        }
                return false;
    }


}

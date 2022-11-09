using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;
using System.Drawing;

/*
 * 고등학교 - 과학고, 특목고(중학교 나의 능력)
 * 대학 - 고등학교의 능력
 * 25 : 대학교
 * 
 * 포트폴리오(자소서, 동영상, 파포:코드)
 * 
 * 장점 - 경쟁력(남들과 비교해서 잘하는거), 남들보다 못하는거 없는개
 * 단점 - 없는게 점
 * 
 * => 포폴 증명....
 * 구현속도, 디버깅, 신기술 도입, 응용
 * cs, algori 좋암
 * 디자인...
 * 
 * 막히면 ㅔ일단 정리를 해서 뭘 체크해야할지 확인하고
 * -> 생각으로만 정리하는게 아니라 책이나 메모장에 적어가면서 확인하고 아닌 부분 체크하거나
 * 원인인 경우 다시 정리핵ㅅ서 기록한ㄷ가 = 정리를 잘한닫
 * 개발 습관. 남들이 안하는 무언가?
 * 
 * Release Note
 * 1990 - 지구에 잘못 떨어짐.
 * 
 * 2017
 * 지구에 있는 생명체 같이 개발(지구인을 알게되ㅑㅁ)
 */

public class ProcEvent
{
	Event playerEvent;
	public ProcEvent()
	{
		playerEvent = GameObject.Find("Main Camera").GetComponent<Event>();

		loadNewDay();
		loadGameOver();
		//남은 생존자, 연구 레벨, 맵 레벨

		popNewDay.show(false);
		popGameOver.show(false);
	}

	public void paint(float dt)
	{
		drawGameOver(dt);
		drawNewDay(dt);
	}
	public void showNewDay(bool stat)
	{
		popNewDay.show(stat);
	}

	public void showGameOver() 
	{
		if (popGameOver.bShow)
			return;
		popGameOver.show(true);
		SoundManager.instance().play(iSound.PopUp);
	}

	public bool key(iKeystate stat, iPoint point)
	{
		if (keyGameOver(stat, point))
			return true;
		if (keyNewDay(stat, point))
			return true;

		return false;
	}

	public bool keyboard(iKeystate stat, iKeyboard key)
	{
		if (keyboardGameOver(stat, key)||keyboardNewDay(stat, key))
			return true;

			return false;
	}

	public bool bShowNewDay()
	{
		return popNewDay.bShow; 
	}

	// ===========================
	// New Day Report
	// ==========================
	iPopup popNewDay = null;

	iStrTex stNewDay;
	iImage imgNewDayBtn;
	iStrTex[] stNewDayBtn;
	int newDayNum;
	float newDayDt;
	void loadNewDay()//새로운 날 ui : 얻은 자원 , 총 자원, 이벤트 요약?
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStNewDay, MainCamera.devWidth - 200, MainCamera.devHeight - 200);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stNewDay = st;
		imgNewDayBtn = new iImage();
		stNewDayBtn = new iStrTex[2];

		int w = MainCamera.devWidth;
		pop.style = iPopupStyle.zoom;
		//pop.methodOpen = closePopNewDay;
		pop.methodClose = closePopNewDay;
		pop.methodDrawBefore = drawPopNewDay;
		pop.openPoint = new iPoint(w / 2, MainCamera.devHeight / 2);
		pop.closePoint = new iPoint((w - (w - 200)) / 2, 100);
		pop._aniDt = 0.5f;
		popNewDay = pop;

		newDayNum = 0;
		newDayDt = -0.5f;
	}

	void closePopNewDay(iPopup pop)
	{
		newDayNum = 0;
		newDayDt = -0.5f;
		stNewDay.setString(newDayNum + " " + playerEvent.day);// click, move
	}

	//void methodStNewDay

	void drawPopNewDay(float dt, iPopup pop, iPoint zero)
	{
		if (newDayNum < 4)
		{
			newDayDt += dt;
			if (newDayDt > 0.5f)
			{
				newDayDt = 0.0f;
				newDayNum++;
				stNewDay.setString(newDayNum + " " + playerEvent.day);// click, move
			}
		}
	}

	void methodStNewDay(iStrTex st)
	{
		int w = MainCamera.devWidth - 200;
		int h = MainCamera.devHeight - 200;
		iGUI.instance.setRGBA(0.3f, .3f, 0.3f, 0.5f);
		iGUI.instance.fillRect(10, 10, w - 20, h - 20);
		iGUI.instance.setRGBA(1, 1, 1, 1f);
		iGUI.instance.setStringRGBA(1, 1, 1, 1);
		float size = iGUI.instance.getStringSize();
		iGUI.instance.setStringSize(80);
		//playerEvent.day;
		int strData = playerEvent.day;
		iGUI.instance.drawString(strData + "일차", w / 2, h / 2 - 150, iGUI.VCENTER | iGUI.HCENTER);
		iGUI.instance.setStringSize(30);

		string[] texname = new string[] { "people", "food", "lab", "map" };
		string[] strs = st.str.Split(" ");
		int n = int.Parse(strs[0]);
		for (int i = 0; i < n; i++)
		{
			iGUI.instance.setStringRGBA(1, 1, 1, 1);

			iPoint p = new iPoint(w / 4 * (i + 1) - 150, h / 2 + 30);
			iGUI.instance.drawImage(Util.createTexture(texname[i]), p, 50.0f / Util.createTexture(texname[i]).width, 50.0f / Util.createTexture(texname[i]).height, iGUI.VCENTER | iGUI.HCENTER);
			iGUI.instance.drawString(playerEvent.storage.getStorageText(i) + "", w / 4 * (i + 1) - 150, h / 2 + 100, iGUI.VCENTER | iGUI.HCENTER);
			iGUI.instance.setStringRGBA(0, 1, 0, 1);

			if (i == 0)
			{
				iGUI.instance.drawString("+ " + playerEvent.plusItem.people, w / 4 * (i + 1) - 150, h / 2 + 150, iGUI.VCENTER | iGUI.HCENTER);
				iGUI.instance.setStringRGBA(1, 0, 0, 1);
				iGUI.instance.drawString("- " + playerEvent.minusItem.people, w / 4 * (i + 1) - 150, h / 2 + 200, iGUI.VCENTER | iGUI.HCENTER);
			}
			else if (i == 1)
			{
				iGUI.instance.drawString("+ " + playerEvent.plusItem.food, w / 4 * (i + 1) - 150, h / 2 + 150, iGUI.VCENTER | iGUI.HCENTER);
				iGUI.instance.setStringRGBA(1, 0, 0, 1);
				iGUI.instance.drawString("- " + playerEvent.minusItem.food, w / 4 * (i + 1) - 150, h / 2 + 200, iGUI.VCENTER | iGUI.HCENTER);
			}
			else if (i == 2)
			{
				iGUI.instance.drawString("+ " + playerEvent.plusItem.labExp + " exp", w / 4 * (i + 1) - 150, h / 2 + 150, iGUI.VCENTER | iGUI.HCENTER);
				iGUI.instance.setStringRGBA(1, 1, 1, 1);
				iGUI.instance.drawString(playerEvent.storage.getStorageText(4), w / 4 * (i + 1) - 150, h / 2 + 200, iGUI.VCENTER | iGUI.HCENTER);
			}
			else if (i == 3)
			{
				iGUI.instance.drawString("+ " + playerEvent.plusItem.mapExp + " exp", w / 4 * (i + 1) - 150, h / 2 + 150, iGUI.VCENTER | iGUI.HCENTER);
				iGUI.instance.setStringRGBA(1, 1, 1, 1);
				iGUI.instance.drawString(playerEvent.storage.getStorageText(5), w / 4 * (i + 1) - 150, h / 2 + 200, iGUI.VCENTER | iGUI.HCENTER);
			}
		}

		iGUI.instance.setStringSize(size);
	}

	void drawNewDay(float dt)
	{
		stNewDay.setString(newDayNum + " " + playerEvent.day);// click, move
		popNewDay.paint(dt);
	}

	bool keyNewDay(iKeystate stat, iPoint point)
	{
		if (popNewDay.bShow == false || popNewDay.state == iPopupState.close)
			return false;

		switch (stat)
		{
			case iKeystate.Began:
				break;

			case iKeystate.Moved:

				break;

			case iKeystate.Ended:
				if (newDayNum < 4)
				{
					newDayNum = 4;
				}
				else
				{
					playerEvent.newday = false;
					playerEvent.initDay();
					popNewDay.show(false);
				}
				break;
		}

		return true;
	}
	bool keyboardNewDay(iKeystate stat, iKeyboard key)
	{
		if (!popNewDay.bShow || popNewDay.state == iPopupState.close)
			return false;
		if (stat == iKeystate.Began && key == iKeyboard.Space)
		{
			if (newDayNum < 4)
			{
				newDayNum = 4;
			}
			else
			{
				playerEvent.newday = false;
				playerEvent.initDay();
				popNewDay.show(false);
			}
		}
		//eventMove = 0.0f;
		return true;
	}


	//GameOver
	iPopup popGameOver = null;
	iStrTex stPopGameOver;

	iImage[] imgGameOverBtn;
	iStrTex[][] stGameOverBtn;
	public bool reset;
	void loadGameOver()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPopGameOver, 500, 500);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stPopGameOver = st;

		imgGameOverBtn = new iImage[2];
		stGameOverBtn = new iStrTex[2][];

		for (int i = 0; i < 2; i++)
		{
			stGameOverBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//닫기 : 0
				if (i == 0)
				{
					st = new iStrTex(methodStGameOverBtn, 200, 50);
					st.setString(j + "\n" + " 게임 다시하기 " + "\n" + i);
				}
				//직업 바꾸기 : 1
				else if (i == 1)
				{
					st = new iStrTex(methodStGameOverBtn, 200, 50);
					st.setString(j + "\n" + "타이틀로" + "\n" + i);
				}
				img.add(st.tex);

				stGameOverBtn[i][j] = st;
			}
			if (i == 0)
				img.position = new iPoint(500 / 2 - 210, 500 - 60);
			else if (i == 1)
				img.position = new iPoint(500 / 2 + 10, 500 - 60);

			pop.add(img);
			imgGameOverBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth / 2 - 250, MainCamera.devHeight / 2 - 250);
		pop.methodDrawBefore = drawPopGameOverBefore;

		pop._aniDt = 0.2f;
		popGameOver = pop;
		reset = false;
	}

	void methodStPopGameOver(iStrTex st)
	{
		//300 500
		iGUI.instance.setRGBA(0.5f, 0.5f, 0.5f, 1);
		iGUI.instance.fillRect(0, 0, 500, 500);

		iGUI.instance.setStringSize(50);
		iGUI.instance.setStringRGBA(0, 0, 0, 1);
		iGUI.instance.drawString("GameOver", new iPoint(250, 100), iGUI.VCENTER | iGUI.HCENTER);
		iGUI.instance.drawString(playerEvent.day + "일 동안 생존", new iPoint(250, 250), iGUI.VCENTER | iGUI.HCENTER);

	}

	void methodStGameOverBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		string s = strs[1];
				
		iGUI.instance.setRGBA(1, 1, 1, 1);
		iGUI.instance.fillRect(0, 0, 300, 50);

		int w = st.tex.tex.width;
		int h = st.tex.tex.height;
		iGUI.instance.setStringSize(30);
		iGUI.instance.setStringRGBA(0, 0, 0, 1);
		iGUI.instance.drawString(s, new iPoint(w / 2, h / 2), iGUI.VCENTER | iGUI.HCENTER);
	}

	void drawPopGameOverBefore(float dt, iPopup pop, iPoint zero)
	{
		for (int i = 0; i < 2; i++)
		{
			imgGameOverBtn[i].frame = (popGameOver.selected == i ? 1 : 0);
		}

		stPopGameOver.setString(popGameOver.selected + "");// newFlags;;;;;;;
	}

	void drawGameOver(float dt)
	{
		popGameOver.paint(dt);
	}
	public bool goTitle = false;
	bool keyGameOver(iKeystate stat, iPoint point)
	{
		if (!popGameOver.bShow)
			return false;

		iPoint p = popGameOver.closePoint;
		iSize s = new iSize(0, 0);

		if (stat == iKeystate.Began)
		{
			for (int i = 0; i < 2; i++)
			{
				if (imgGameOverBtn[i].touchRect(p, s).containPoint(point))
				{
					SoundManager.instance().play(iSound.ButtonClick);
					popGameOver.selected = i;
					break;
				}
			}
		}
		else if (stat == iKeystate.Moved)
		{
			for (int i = 0; i < 2; i++)
			{
				if (!imgGameOverBtn[i].touchRect(p, s).containPoint(point))
				{
					popGameOver.selected = -1;
					break;
				}
			}
		}
		else if (stat == iKeystate.Ended)
		{
			int select = popGameOver.selected;
			if (select != -1)
			{
				playerEvent.initGame();
				if (select == 0)
				{					
					reset = true;
				}
				else if (select == 1)
				{
					Debug.Log("titlfe");
					goTitle = true;
				}
				popGameOver.selected = -1;
				popGameOver.show(false);
			}
		}

		return true;
	}

	bool keyboardGameOver(iKeystate stat, iKeyboard key)
	{
		if (!popGameOver.bShow)
			return false;
		return true;
	}

}

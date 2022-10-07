using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Proc : gGUI
{
	public override void load()
	{
		loadBg();
		loadPlayer();
		loadPeople();

		createPopTop();
		createPopPerson();
		createPopInfo();
		createNewDay();
		createPopEvent();

		popTop.show(true);
		popPerson.show(false);
		popPersonInfo.show(false);
		popNewDay.show(false);
		popEvent.show(false);

		//우선순위
		MethodMouse[] m = new MethodMouse[]
		{
			keyPopInfo, keyPopPerson, keyPopTop, keyBg, keyPopEvent, keyNewDay, keyPopEvent
		};
		for (int i = 0; i < m.Length; i++)
			MainCamera.addMethodMouse(new MethodMouse(m[i]));

		MethodKeyboard[] mkeyboard = new MethodKeyboard[]
		{
			keyboardPlayer, keyboardPopEvent, keyboardNewDay
		};

		for (int i = 0; i < mkeyboard.Length; i++)
			MainCamera.addMethodKeyboard(new MethodKeyboard(mkeyboard[i]));

		MainCamera.addMethodWheel(new MethodWheel(wheelPopPerson));
	}

	public override void free()
	{

	}

	public override void draw(float dt)
	{
		drawBg(dt);
		drawPlayer(dt);
		drawPeople(dt);
		drawPopTop(dt);
		drawPopPerson(dt);
		drawPopInfo(dt);
		drawPopEvent(dt);
		drawNewDay(dt);

	}

	public override bool key(iKeystate stat, iPoint point)
	{
		//keyNewDay(stat, point);
		//keyPopInfo(stat, point);
		//keyPopPerson(stat, point);
		//keyBg(stat, point);
		//keyPopTop(stat, point)

		return false;
	}
	public override bool wheel(iPoint point)
	{
		wheelPopPerson(point);
		return false;
	}

	//=======================================================
	// bg
	//=======================================================
	void loadBg()
	{

	}

	void drawBg(float dt)
	{
		setRGBA(1, 1, 1, 1f);
		fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);
	}

	bool keyBg(iKeystate stat, iPoint point)
	{
		return false;
	}

	Event playerEvent;
	bool newDayOpen;

	//=======================================================
	// Player
	//=======================================================
	void loadPlayer()
	{
		newDayOpen = false;
		pPos = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2);
		speed = 50;
		nextPos = new iPoint(0, 0);
		psize = new iSize(50, 50);

		playerEvent = GameObject.Find("Main Camera").GetComponent<Event>();
		people = playerEvent.storage.people;
	}

	iPoint pPos;
	iPoint nextPos;
	int speed;
	iSize psize;
	void drawPlayer(float dt)
	{
		pPos.x += dt * nextPos.x * speed;
		pPos.y += dt * nextPos.y * speed;

		if (pPos.x < 0)
			pPos.x = 0;
		else if (pPos.x > MainCamera.devWidth - 50)
			pPos.x = MainCamera.devWidth - 50;

		if (pPos.y < 60)
			pPos.y = 60;
		else if (pPos.y > MainCamera.devHeight - 50)
			pPos.y = MainCamera.devHeight - 50;

		setRGBA(0, 1, 0, 1f);
		fillRect(pPos.x, pPos.y, psize.width, psize.height);

		nextPos = new iPoint(0, 0);

	}

	bool keyboardPlayer(iKeystate stat, iKeyboard key)
	{
		nextPos = new iPoint(0, 0);

		switch (key)
		{
			case iKeyboard.Down:
				nextPos.y += 5;

				break;
			case iKeyboard.Up:
				nextPos.y -= 5;

				break;
			case iKeyboard.Left:
				nextPos.x -= 5;

				break;
			case iKeyboard.Right:
				nextPos.x += 5;

				break;
			case iKeyboard.Space:
				if (methodPeople != null)
					break;

				float x, y;
				x = pPos.x + psize.width;
				y = pPos.y + (psize.height / 2) - 175;
#if false
				if (pPos.x > MainCamera.devWidth / 2)
					popEvent.closePoint = new iPoint(pPos.x - 300 + 75, y);
				else
					popEvent.closePoint = new iPoint(x + 10, y);
#else
				// 200, 350;
				iPoint[] p = new iPoint[2];
				p[0] = pPos + new iPoint(psize.width / 2, psize.height / 2);
				p[1] = p[0] + new iPoint(psize.width, -175);

				iPoint min = new iPoint(10, 10);
				iPoint max = new iPoint(MainCamera.devWidth - 500, MainCamera.devHeight - 380);

				if (p[1].x < min.x)
					p[1].x = min.x;
				else if (p[1].x > max.x)
					p[1].x = max.x;
				if (p[1].y < min.y)
					p[1].y = min.y;
				else if (p[1].y > max.y)
					p[1].y = max.y;

				popEvent.openPoint = p[0];
				popEvent.closePoint = p[1];
#endif
				popEvent.show(true);
				break;
		}

		return false;
	}

	//====================================================
	// people
	//====================================================

	void loadPeople()
	{
		people = playerEvent.storage.people;
		//if (!playerEvent.newday)
		{
			for (int i = 0; i < people; i++)
			{
				playerEvent.pState[i].pos = new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 150);
			}
		}

		_moveDt = 3f;
		peopleInOut = new iPoint[]
		{
			new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 150),//home
			new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight - 150),//street
			new iPoint(MainCamera.devWidth / 2, -50),//up
			new iPoint(MainCamera.devWidth - 280, MainCamera.devHeight/2 - 130),//lab
			new iPoint(MainCamera.devWidth / 2, -50),


		};
		float len = 0f, l = 0f;
		for (int i = 0; i < 2; i++)
		{
			iPoint p = peopleInOut[i] - peopleInOut[1 + i];
			float n = Mathf.Sqrt(p.x * p.x + p.y * p.y);
			len += n;
			if (i == 0)
				l = n;
		}
		moveRate = l / len;
		setPeople(1, cbPeopleGo);
	}

	void cbPeopleGo()
	{
		//popEvent.show(true);
	}

	float _moveDt;
	iPoint[] peopleInOut;
	float moveRate;

	delegate void MethodPeople();
	MethodPeople methodPeople = null;
	void setPeople(int behave, MethodPeople method)
	{
		for (int i = 0; i < people; i++)
		{
			PeopleState ps = playerEvent.pState[i];
			if ((behave == 1 && ps.behave == 0) || (behave == 2 && ps.behave == 3))
			{
				ps.behave = behave;// 1 or 2 go or back
				ps.moveDt = -0.2f * i;
			}
		}
		methodPeople = method;
	}
	
	void drawPeople(float dt)
	{
		people = playerEvent.storage.people;
		if (people == 0)
			return;

		bool endOfGo = true;
		bool endOfBack = true;

		for (int i = 0; i < people; i++)
		{
			PeopleState ps = playerEvent.pState[i];
			if (ps.moveDt < 0f)
			{
				ps.moveDt += dt;
				continue;
			}
			if (ps.behave == 1)
			{
				// go
				float r = ps.moveDt / _moveDt;
				if (r < moveRate)
					ps.pos = Math.linear(r / moveRate, peopleInOut[0], peopleInOut[1]);
				else
				{
					int at = ps.job != 4 ? 2 : 3;
					ps.pos = Math.linear((r - moveRate) / (1f - moveRate), peopleInOut[1], peopleInOut[at]);
				}

				ps.moveDt += dt;
				if (ps.moveDt > _moveDt)
					ps.behave = 3;
				else
					endOfGo = false;
			}
			else if (ps.behave == 2)
			{
				// back
				float r = ps.moveDt / _moveDt;
				if (r < moveRate)
				{
					int at = ps.job != 4 ? 2 : 3;
					ps.pos = Math.linear(r / moveRate, peopleInOut[at], peopleInOut[1]);
				}
				else
					ps.pos = Math.linear((r - moveRate) / (1f - moveRate), peopleInOut[1], peopleInOut[0]);

				ps.moveDt += dt;
				if (ps.moveDt > _moveDt)
					ps.behave = 0;
				else
					endOfBack = false;
			}
			setRGBA(1, 1, 1, 1);
			string[] texname = new string[] { "jobless", "explorer", "worker", "farmer", "researcher" };
			Texture peopleTex = Util.createTexture(texname[ps.job]);
			drawImage(peopleTex, ps.pos, psize.width / peopleTex.width, psize.height / peopleTex.height, LEFT | HCENTER);
			
			Texture building = Util.createTexture("house");
			drawImage(building, new iPoint(MainCamera.devWidth - 300, MainCamera.devHeight - 200), 100.0f / building.width, 100.0f / building.height, LEFT | HCENTER);
			drawImage(Util.createTexture("research"), new iPoint(MainCamera.devWidth - 300, MainCamera.devHeight/2 - 150), 100.0f / building.width, 100.0f / building.height, LEFT | HCENTER);
		}

		if (methodPeople != null)
		{
			if( endOfGo && endOfBack )
			{
				methodPeople();
				methodPeople = null;
			}
		}

#if false
		if (!playerEvent.newday)
		{
			// 0: MainCamera.devWidth - 250, MainCamera.devHeight - 150
			// 1: MainCamera.devWidth / 2, MainCamera.devHeight - 150
			// 1: MainCamera.devWidth / 2, -50

			for (int i = 0; i < people; i++)
			{
				PeopleState ps = playerEvent.pState[i];
				iPoint p = playerEvent.pState[i].pos;
				if (i < pindex)
				{
					if (p.x > MainCamera.devWidth / 2)
						playerEvent.pState[i].pos.x--;
					else if (p.y > -50)
						playerEvent.pState[i].pos.y--;
					else if(playerEvent.pState[i].behave == 1)
						playerEvent.pState[i].behave = 3;
				}

				fillRect(playerEvent.pState[i].pos.x, playerEvent.pState[i].pos.y, psize.width, psize.height);
			}
			pdt += dt;
			if (pdt > 0.5f)
			{
				pdt -= 0.5f;
				pindex++;
				if (pindex > people)
					pindex = people;
			}
		}
		else if(playerEvent.newday||playerEvent.pState[0].behave == 2)
		{
			for (int i = 0; i < people; i++)
			{
				PeopleState ps = playerEvent.pState[i];
				iPoint p = playerEvent.pState[i].pos;
				if (playerEvent.pState[i].behave != 2)
					playerEvent.pState[i].behave = 2;

				if (i < pindex)
				{
					if (p.y < MainCamera.devHeight - 150)
						playerEvent.pState[i].pos.y++;
					else if (p.x < MainCamera.devWidth - 250)
						playerEvent.pState[i].pos.x++;
					else if (playerEvent.pState[i].behave == 2)
						playerEvent.pState[i].behave = 0;
				}

				fillRect(playerEvent.pState[i].pos.x, playerEvent.pState[i].pos.y, psize.width, psize.height);
			}
			pdt += dt;
			if (pdt > 0.5f)
			{
				pdt -= 0.5f;
				pindex++;
				if (pindex > people)
					pindex = people;
			}
		}
#endif
	}

	//=======================================================
	// popTop Resource info
	//=======================================================
	iPopup popTop = null;
	iStrTex stPopTop;

	void createPopTop()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPopTop, MainCamera.devWidth, 60);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stPopTop = st;

		pop.style = iPopupStyle.move;
		pop.openPoint = new iPoint(0, -60);
		pop.closePoint = new iPoint(0, 0);
		pop._aniDt = 0.5f;
		popTop = pop;
	}

	void drawPopTop(float dt)
	{
		Storage sCheck = playerEvent.storage;
		for (int i = 0; i < 6; i++)
		{

			stPopTop.setString(sCheck.getStorage(i) + "");
		}
		popTop.paint(dt);
	}

	bool keyPopTop(iKeystate stat, iPoint point)
	{
		//if (popTop == null || popTop.bShow == false)
		//	return false;

		if (stat == iKeystate.Began)
		{

		}
		return false;
	}

	public void methodStPopTop(iStrTex st)
	{
		setRGBA(0, 0, 0, 0.8f);
		fillRect(0, 0, MainCamera.devWidth, 60);
		setRGBA(1, 1, 1, 1);
		iPoint p = new iPoint(5, 10);

		for (int i = 0; i < 4; i++)
		{
			drawString(playerEvent.storage.getStorageText(i), 60 + i * 150, 15, RIGHT | HCENTER);
			string[] texname = new string[] { "people", "food", "lab", "map" };
			//Texture resource = Util.createTexture(texname[i]);
			//Texture resource = Resources.Load<Texture>(texname[i]);
			p.x = 5 + i * 150;
			drawImage(Util.createTexture(texname[i]), p, 40.0f / Util.createTexture(texname[i]).width, 40.0f / Util.createTexture(texname[i]).height, LEFT | HCENTER);
		}
	}

	iRect checkScrollbar(int barW, int barH)
	{
		people = playerEvent.storage.people;
		// 가로 크기 / 총 크기
		int miniWidth = 200;
		int miniHeight = 500;

		int mapWidth = 200;
		int mapHeight = 60 * people;

		// 칸수
		float numW = 1.0f * mapWidth / miniWidth;
		float numH = 1.0f * mapHeight / miniHeight;

		//int bW = barW / bNumW;
		//int bH = barH / bNumH;
		int bW = barW * miniWidth / mapWidth;
		int bH = barH * miniHeight / mapHeight;

		int bX = (int)Math.linear(offPerson.x / offMin.x, 0, bW * (numW - 1));
		int bY = (int)Math.linear(offPerson.y / offMin.y, 0, bH * (numH - 1));

		return new iRect(bX, bY, bW, bH);
	}


	// ======================================================
	// popPerson
	// ======================================================
	iPopup popPerson = null;

	iStrTex stPerson;
	iImage[] imgPersonBtn;
	iStrTex[][] stPersonBtn;

	iPoint offPerson, offMin, offMax;
	string[] names;
	void createPopPerson()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPerson, 200, 500);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stPerson = st;
		people = playerEvent.storage.people;
		imgPersonBtn = new iImage[100];
		stPersonBtn = new iStrTex[100][];
#if false
		for (int i = 0; i < people; i++)
		{
			stPersonBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				st = new iStrTex(methodStPersonBtn, 150, 50);
				st.setString(j + "\n" + playerEvent.pState[i].name + "\n" +i);
				img.add(st.tex);

				stPersonBtn[i][j] = st;
			}
			img.position = new iPoint(20, 10 + 60 * i);
			imgPersonBtn[i] = img;
		}
#else
		for (int i = 0; i < 100; i++)
		{
			stPersonBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				st = new iStrTex(methodStPersonBtn, 150, 50);
				string s = "null";
				if (i < people) s = playerEvent.pState[i].name;
				st.setString(j + "\n" + s + "\n" + i);
				img.add(st.tex);

				stPersonBtn[i][j] = st;
			}
			img.position = new iPoint(20, 10 + 60 * i);
			imgPersonBtn[i] = img;
		}
#endif

		pop.style = iPopupStyle.move;
		pop.methodClose = closePopPerson;
		pop.methodOpen = closePopPerson;
		pop.openPoint = new iPoint(MainCamera.devWidth, (MainCamera.devHeight - 500) / 2);
		pop.closePoint = new iPoint(MainCamera.devWidth - 210, (MainCamera.devHeight - 500) / 2);
		pop._aniDt = 0.2f;
		popPerson = pop;

		offPerson = new iPoint(0, 0);
		offMin = new iPoint(0, 490 - 60 * people);
		offMax = new iPoint(0, 0);
	}

	int people = 30;
	public void methodStPerson(iStrTex st)
	{
		setRGBA(0.3f, 0.3f, 0.3f, 0.5f);
		fillRect(0, 0, 300, 600);
		people = playerEvent.storage.people;
		setRGBA(1, 1, 1, 1);
		for (int i = 0; i < people; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				string s = playerEvent.pState[i].name;
				if (s == null)
					s = "null";
				stPersonBtn[i][j].setString(j + "\n" + s + "\n" + i);
			}
			imgPersonBtn[i].frame = (popPerson.selected == i ? 1 : 0);
			imgPersonBtn[i].paint(0.0f, offPerson);
		}

		if (playerEvent.storage.getStorage(0) > 9)
		{
			iRect rt = checkScrollbar(200 - 20,
									500 - 40);
			// 상하 스크롤바
			float x = 200 - 20;
			float y = 10;
			float w = 10;
			float h = 500 - 20;
			setRGBA(0, 0, 0, 1f);
			fillRect(x + w / 2 - 2, y, 4, h);

			// 손잡이
			y += 10 + rt.origin.y;
			h = rt.size.height;
			fillRect(x, y, w, h);
		}
	}

	public void methodStPersonBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		int pindex = int.Parse(strs[2]);
		string s = strs[1];
		setRGBA(1, 1, 1, 1);
		if (index == 0)
		{
			setRGBA(1, 1, 1, 1);
			if (playerEvent.pState[pindex].behave == 3)
				setRGBA(0, 1, 0, 1);
		}
		else
			setRGBA(0.3f, 0.3f, 0.3f, 1);

		fillRect(0, 0, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		drawString(s, 150 / 2, 50 / 2, VCENTER | HCENTER);
		GUI.color = Color.white;
	}

	bool open = false;
	float closedt = 0;
	void closePerson(float dt)
	{
		closedt += dt;
		if (closedt > 1)
		{
			closedt = 0;
			popPerson.show(false);
			open = false;
		}
	}

	void closePopPerson(iPopup pop)
	{
		people = playerEvent.storage.people;
		offMin = new iPoint(0, 490 - 60 * people);
		offPerson = new iPoint(0, 0);
	}

	void drawPopPerson(float dt)
	{
		if (popNewDay.bShow)
			return;
		if (MainCamera.mousePosition().x > MainCamera.devWidth - 50 && !open)
		{
			open = true;
			popPerson.show(true);
		}
		else if (MainCamera.mousePosition().x > MainCamera.devWidth - 200 && open)
		{
			if (closedt != 0)
				closedt = 0;
		}
		else if (MainCamera.mousePosition().x < MainCamera.devWidth - 200
			&& open && select == -1)
			closePerson(dt);
		people = playerEvent.storage.people;
		stPerson.setString(popPerson.selected + " " + offPerson.y + " " + people);// click, move

		popPerson.paint(dt);
	}

	bool scroll;
	iPoint prevPoint, firstPoint, mp;

	bool keyPopPerson(iKeystate stat, iPoint point)
	{
		if (popPerson.bShow == false || popPerson.state == iPopupState.close)
			return false;

		if (popPerson.state != iPopupState.proc)
		{
			// 화면 안에 있을때
			return true;
			// 없을대 false
		}

		iPoint p;
		p = popPerson.closePoint;
		p.y += offPerson.y;

		int i, j = -1;
		iSize s = new iSize(0, 0);

		switch (stat)
		{
			case iKeystate.Began:
				scroll = false;
				firstPoint = point;
				prevPoint = point;
				people = playerEvent.storage.people;
				for (i = 0; i < people; i++)
				{
					if (imgPersonBtn[i].touchRect(p, s).containPoint(point))//클릭되면 ㅁ
					{
						j = i;
						break;
					}
				}
				if (j != -1)
				{
					// audio play button 효과음
					popPerson.selected = j;
					//select = j;// line 440
				}
				break;

			case iKeystate.Moved:
				if (scroll == false)
				{
					mp = point - firstPoint;
					if (Mathf.Sqrt(mp.x * mp.x + mp.y * mp.y) > 5)
					{
						if (point.x > popPerson.closePoint.x && point.x < popPerson.closePoint.x + 200 &&
							point.y > popPerson.closePoint.y && point.y < popPerson.closePoint.y + 500)
							scroll = true;
						prevPoint = point;

						popPerson.selected = -1;
					}
				}

				if (scroll)
				{
					people = playerEvent.storage.people;
					if (people > 8)
					{
						mp = point - prevPoint;
						prevPoint = point;

						//offX += mp.x;
						offPerson.y += mp.y;
						if (offPerson.y < offMin.y)
							offPerson.y = offMin.y;
						else if (offPerson.y > offMax.y)
							offPerson.y = offMax.y;
					}
				}
				break;

			case iKeystate.Ended:
				if (scroll == false)
				{
					if (popPerson.selected != -1)// line 403
						select = popPerson.selected;

					if (popPersonInfo.bShow == false)
					{
						if (popPerson.selected != -1)
						{
							if (!popNewDay.bShow)
								popPersonInfo.show(true);

							popPersonInfo.openPoint = imgPersonBtn[popPerson.selected].center(p);
						}
					}
				}
				break;
		}
		return false;
	}

	bool wheelPopPerson(iPoint point)
	{
		if (popNewDay.bShow || popPerson.state == iPopupState.close)
			return false;

		//if (playerEvent.storage.getStorage(0) < 9)
		//	return false;

		iPoint p = MainCamera.mousePosition();
		if (p.x > popPerson.closePoint.x && p.x < popPerson.closePoint.x + 200 &&
			p.y > popPerson.closePoint.y && p.y < popPerson.closePoint.y + 500)
			offPerson.y += point.y * 10.0f;

		if (offPerson.y < offMin.y)
			offPerson.y = offMin.y;
		else if (offPerson.y > offMax.y)
			offPerson.y = offMax.y;

		return true;
	}

	//=======================================================
	// popInfo
	//=======================================================
	iPopup popPersonInfo = null;

	iStrTex stPersonInfo;
	iImage[] imgPersonInfoBtn;
	iStrTex[][] stPersonInfoBtn;
	int select;
	void createPopInfo()
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPersonInfo, 700, 400);
		// st.setString imgPersonInfoBtn 아직 생성안함
		img.add(st.tex);
		pop.add(img);
		stPersonInfo = st;
		select = -1;

		//닫는버튼 / 직업 바꾸기 버튼 두 개만 필요
		imgPersonInfoBtn = new iImage[3]; // 0 : 닫기 1 : < 직업 바꾸기 | 2 : > 직업바꾸기
		stPersonInfoBtn = new iStrTex[3][];//눌렸을 때

		for (int i = 0; i < 3; i++)
		{
			stPersonInfoBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				//닫기 : 0
				if (i == 0)
				{
					st = new iStrTex(methodStPersonInfoBtn, 50, 50);
					st.setString(j + "\n" + " X " + "\n" + i);
				}
				//직업 바꾸기 : 1
				else if (i == 1)
				{
					st = new iStrTex(methodStPersonInfoBtn, 50, 50);
					st.setString(j + "\n" + "<" + "\n" + i);
				}
				else
				{
					st = new iStrTex(methodStPersonInfoBtn, 50, 50);
					st.setString(j + "\n" + ">" + "\n" + i);
				}
				img.add(st.tex);

				stPersonInfoBtn[i][j] = st;
			}
			if (i == 0)
				img.position = new iPoint(700 - 70, 10);
			else if (i == 1)
				img.position = new iPoint(450, 300);
			else if (i == 2)
				img.position = new iPoint(550, 300);
			imgPersonInfoBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(MainCamera.devWidth, MainCamera.devHeight);
		pop.closePoint = new iPoint(MainCamera.devWidth / 2 - 350, MainCamera.devHeight / 2 - 200);
		pop._aniDt = 0.5f;
		popPersonInfo = pop;
	}
	public void methodStPersonInfo(iStrTex st)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 1f);
		fillRect(0, 0, 700, 400);

		setRGBA(1, 1, 1, 1);

		string[] stateTxt = new string[] { "움직임", "공격", "일", "병" };
		string[] btnJobTxt = new string[] { "백수", "탐험가", "일꾼", "농부", "연구원" };
		string[] texname = new string[] { "jobless", "explorer", "worker", "farmer", "researcher" };
		if (select != -1)
		{
			iPoint p = new iPoint(50, 50);
			fillRect(new Rect(p.x, p.y, 300, 300));
			int jobindex = playerEvent.pState[select].job;

			drawImage(Util.createTexture(texname[jobindex]), p, 300.0f / Util.createTexture(texname[jobindex]).width, 300.0f / Util.createTexture(texname[jobindex]).height, LEFT | HCENTER);
			PeopleState ps = playerEvent.pState[select];
			drawString("이름 : " + ps.name, new iPoint(450, 100), LEFT | HCENTER);
			drawString("레벨 : " + ps.jobLevel[ps.job], new iPoint(450, 150), LEFT | HCENTER);
			drawString("동작 : " + stateTxt[ps.behave], new iPoint(450, 200), LEFT | HCENTER);
			drawString("직업 : " + btnJobTxt[ps.job], new iPoint(450, 250), LEFT | HCENTER);
		}

		for (int i = 0; i < 3; i++)
		{
			//for (int j = 0; j < 2; j++)
			//	stPersonBtn[i][j].setString(j + "\n" + i + "번");
			imgPersonInfoBtn[i].frame = (popPersonInfo.selected == i ? 1 : 0);
			imgPersonInfoBtn[i].paint(0.0f, new iPoint(0, 0));
		}
	}

	public void methodStPersonInfoBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		int bindex = int.Parse(strs[2]);
		string s = strs[1];

		iPoint pos = new iPoint(0, 0);

		if (index == 0 && bindex == 0)
			setRGBA(1, 0, 0, 1);
		else if (index == 0 && bindex > 0)
			setRGBA(1, 1, 1, 1);
		else if (index > 0)
		{
			setRGBA(0.3f, 0.3f, 0.3f, 1);
		}

		int w = st.tex.tex.width;
		int h = st.tex.tex.height;
		fillRect(0, 0, w, h);
		setStringRGBA(0, 0, 0, 1);
		drawString(s, w / 2, h / 2, VCENTER | HCENTER);
	}

	void drawPopInfo(float dt)
	{
		if (popNewDay.bShow)
		{
			popPerson.selected = -1;
			return;
		}
		stPersonInfo.setString(popPerson.selected + "" + popPersonInfo.selected + "" + select);
		popPersonInfo.paint(dt);
	}

	bool keyPopInfo(iKeystate stat, iPoint point)
	{
		if (popPersonInfo.bShow == false || popPersonInfo.state == iPopupState.close)
			return false;


		int i, j = -1;
		iPoint p;
		p = popPersonInfo.closePoint;
		iSize s = new iSize(0, 0);

		switch (stat)
		{
			case iKeystate.Began:
				for (i = 0; i < 3; i++)
				{
					if (imgPersonInfoBtn[i].touchRect(p, s).containPoint(point))
					{
						j = i;
						break;
					}
				}
				if (j != -1)
				{
					// audio play button 효과음
					popPersonInfo.selected = j;
				}
				break;

			case iKeystate.Moved:
				break;

			case iKeystate.Ended:
				if (popPersonInfo.selected != -1)
				{
					if (popPersonInfo.selected == 0)
					{
						popPerson.selected = -1;
						select = -1;

						popPersonInfo.show(false);
					}
					else if (popPersonInfo.selected == 1)
					{
						PeopleState ps = playerEvent.pState[select];
						int job = ps.job - 1;
						if (job < 0)
							job = 4;
						ps.jobUpdate(job);
						if (ps.behave == 3)
						{
							ps.behave = 2;
							ps.moveDt = -0.2f;
						}
					}
					else if (popPersonInfo.selected == 2)
					{
						PeopleState ps = playerEvent.pState[select];
						int job = ps.job + 1;
						if (job > 4)
							job = 0;
						ps.jobUpdate(job);
						if (ps.behave == 3)
						{
							ps.behave = 2;
							ps.moveDt = -0.2f;
						}
					}

					popPersonInfo.selected = -1;
				}

				break;
		}
		return true;
	}

	iPopup popNewDay = null;

	iStrTex stNewDay;
	int newDayNum;
	float newDayDt;
	void createNewDay()//새로운 날 ui : 얻은 자원 , 총 자원, 이벤트 요약?
	{
		iPopup pop = new iPopup();

		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStNewDay, MainCamera.devWidth - 200, MainCamera.devHeight - 200);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stNewDay = st;

		int w = MainCamera.devWidth;
		pop.style = iPopupStyle.zoom;
		pop.methodOpen = closePopNewDay;
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
		setRGBA(0f, 0f, 0f, 0.8f);
		int w = MainCamera.devWidth - 200;
		int h = MainCamera.devHeight - 200;
		fillRect(0, 0, w, h);
		setRGBA(0.3f, 0.3f, 0.3f, 1f);
		fillRect(10, 10, w - 20, h - 20);
		setRGBA(1, 1, 1, 1f);
		setStringRGBA(1, 1, 1, 1);
		float size = getStringSize();
		setStringSize(80);
		//playerEvent.day;
		int strData = playerEvent.day;
		drawString(strData + "일차", w / 2, h / 2 - 150, VCENTER | HCENTER);
		setStringSize(30);

		string[] texname = new string[] { "people", "food", "lab", "map" };
		string[] strs = st.str.Split(" ");
		int n = int.Parse(strs[0]);
		for (int i = 0; i < n; i++)
		{
			//fillRect(w / 4 * (i + 1) - 150, h / 2 + 100, 100,100);
			setStringRGBA(1, 1, 1, 1);
			//Texture resource = Resources.Load<Texture>(texname[i]);

			iPoint p = new iPoint(w / 4 * (i + 1) - 150, h / 2 + 30);
			drawImage(Util.createTexture(texname[i]), p, 50.0f / Util.createTexture(texname[i]).width, 50.0f / Util.createTexture(texname[i]).height, VCENTER | HCENTER);

			drawString(playerEvent.storage.getStorageText(i) + "", w / 4 * (i + 1) - 150, h / 2 + 100, VCENTER | HCENTER);

			setStringRGBA(0, 1, 0, 1);
			if (i == 0)
			{
				drawString("+ " + playerEvent.plusItem.people, w / 4 * (i + 1) - 150, h / 2 + 150, VCENTER | HCENTER);
				setStringRGBA(1, 0, 0, 1);
				drawString("- " + playerEvent.minusItem.people, w / 4 * (i + 1) - 150, h / 2 + 200, VCENTER | HCENTER);
			}
			else if (i == 1)
			{
				drawString("+ " + playerEvent.plusItem.food, w / 4 * (i + 1) - 150, h / 2 + 150, VCENTER | HCENTER);
				setStringRGBA(1, 0, 0, 1);
				drawString("- " + playerEvent.minusItem.food, w / 4 * (i + 1) - 150, h / 2 + 200, VCENTER | HCENTER);
			}
			else if (i == 2)
			{
				drawString("+ " + playerEvent.plusItem.labExp + " exp", w / 4 * (i + 1) - 150, h / 2 + 150, VCENTER | HCENTER);
				setStringRGBA(1, 1, 1, 1);
				drawString(playerEvent.storage.getStorageText(4), w / 4 * (i + 1) - 150, h / 2 + 200, VCENTER | HCENTER);
			}
			else if (i == 3)
			{
				drawString("+ " + playerEvent.plusItem.mapExp + " exp", w / 4 * (i + 1) - 150, h / 2 + 150, VCENTER | HCENTER);
				setStringRGBA(1, 1, 1, 1);
				drawString(playerEvent.storage.getStorageText(5), w / 4 * (i + 1) - 150, h / 2 + 200, VCENTER | HCENTER);
			}
		}

		setStringSize(size);
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
					newDayOpen = false;
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
				newDayOpen = false;
				playerEvent.newday = false;
				playerEvent.initDay();
				popNewDay.show(false);
			}
		}

		return true;
	}

	// ======================================================
	// event select popup
	// ======================================================

	iPopup popEvent = null;
	iStrTex stPopEvent;

	iImage[] imgPopEventBtn;
	iStrTex[][] stPopEventBtn;
	string[] btnPopEventTxt;
	void createPopEvent()
	{
		btnPopEventTxt = new string[] { "탐색", "사냥", "연구", "휴식" };

		iPopup pop = new iPopup();
		iImage img = new iImage();
		iStrTex st = new iStrTex(methodStPopEvent, 200, 350);
		st.setString("0");
		img.add(st.tex);
		pop.add(img);
		stPopEvent = st;

		imgPopEventBtn = new iImage[btnPopEventTxt.Length];
		stPopEventBtn = new iStrTex[btnPopEventTxt.Length][];

		for (int i = 0; i < btnPopEventTxt.Length; i++)
		{
			stPopEventBtn[i] = new iStrTex[2];

			img = new iImage();
			for (int j = 0; j < 2; j++)
			{
				st = new iStrTex(methodStPopEventBtn, 150, 50);
				st.setString(j + "\n" + btnPopEventTxt[i]);
				img.add(st.tex);

				stPopEventBtn[i][j] = st;
			}
			img.position = new iPoint(20, 10 + 80 * i);
			imgPopEventBtn[i] = img;
		}

		pop.style = iPopupStyle.zoom;
		pop.openPoint = new iPoint(pPos.x, pPos.y);
		//pop.closePoint = new iPoint(MainCamera.devWidth/2, MainCamera.devHeight/2);
		pop.closePoint = new iPoint(pPos.x - 300 + 75, pPos.y + (psize.height / 2) - 175);

		pop._aniDt = 0.2f;
		popEvent = pop;
	}

	public void methodStPopEvent(iStrTex st)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 0.8f);
		fillRect(0, 0, 300, 700);

		setRGBA(1, 1, 1, 1f);

		for (int i = 0; i < btnPopEventTxt.Length; i++)
		{
			imgPopEventBtn[i].frame = (popEvent.selected == i ? 1 : 0);
			imgPopEventBtn[i].paint(0.0f, new iPoint(10, 20));
		}
	}
	public void methodStPopEventBtn(iStrTex st)
	{
		string[] strs = st.str.Split("\n");
		int index = int.Parse(strs[0]);
		string s = strs[1];

		if (index == 0)
			setRGBA(1, 1, 1, 1);
		else
			setRGBA(0.3f, 0.3f, 0.3f, 1);

		fillRect(0, 0, 150, 50);

		setStringRGBA(0, 0, 0, 1);
		drawString(s, 150 / 2, 50 / 2, VCENTER | HCENTER);
		GUI.color = Color.white;
	}

	void drawPopEvent(float dt)
	{
		stPopEvent.setString(popEvent.selected + "");
		popEvent.paint(dt);
	}

	bool keyPopEvent(iKeystate stat, iPoint point)
	{
		if (!popEvent.bShow || popEvent.state == iPopupState.close)
			return false;

		int i, j = -1;
		iPoint p;
		p = popEvent.closePoint;
		p.x += 10;
		p.y += 20;
		//75, 30
		iSize s = new iSize(0, 0);

		switch (stat)
		{
			case iKeystate.Began:
				for (i = 0; i < btnPopEventTxt.Length; i++)
				{
					if (imgPopEventBtn[i].touchRect(p, s).containPoint(point))
					{
						Debug.Log("음:선택");
 						popEvent.selected = i;
						break;
					}
				}
				if( popEvent.selected==-1 )
				{
					popEvent.openPoint = new iPoint(pPos.x, pPos.y);
					popEvent.show(false);
				}
				break;

			case iKeystate.Moved:
				i = popEvent.selected;
				if (i == -1) break;
				if (imgPopEventBtn[i].touchRect(p, s).containPoint(point)==false)
				{
					Debug.Log("음:취소");
					popEvent.selected = -1;
				}
				break;

			case iKeystate.Ended:
				i = popEvent.selected;
				if (i == -1) break;
				popEvent.selected = -1;
				Debug.Log("음:실행");

				switch (i)
				{
					case 0:
						playerEvent.doEvent(Event.DoEvent.Adventure);						
						setPeople(1, cbPeopleGo);
						break;
					case 1:
						playerEvent.doEvent(Event.DoEvent.Hunt);
						setPeople(1, cbPeopleGo);
						break;
					case 2:
						playerEvent.doEvent(Event.DoEvent.Research);
						setPeople(1, cbPeopleGo);
						break;
					case 3:
						playerEvent.doEvent(Event.DoEvent.SkipDay);
						
						break;
				}
				if (playerEvent.newday)
					setPeople(2, cbPeopleBack);
				popEvent.openPoint = new iPoint(pPos.x, pPos.y);
				popEvent.show(false);
				break;
		}
		return true;
	}
	void cbPeopleBack()
	{		
		popPerson.show(false);
		popNewDay.show(true);
	}

	bool keyboardPopEvent(iKeystate stat, iKeyboard key)
	{
		if (!popEvent.bShow || popEvent.state == iPopupState.close)
			return false;

		if (key == iKeyboard.Space)
		{
			popEvent.openPoint = new iPoint(pPos.x, pPos.y);
		}

		return true;
	}
}

class Scroll //그려야하는 위치. 스크롤바 크기, 
{
	public iPoint off, offMin, offMax;
	iRect drawRt;
	iSize barSize;

	public Scroll(iRect rt, iSize size, iSize bs)//그려야하는 위치, 크기?
	{
		off = new iPoint(0, 0);
		offMax = new iPoint(0, 0);
		offMin = new iPoint(rt.size.width - size.width, rt.size.height - size.height);

		drawRt = rt;
		barSize = bs;
	}

	public void scrollMouse(iPoint mp)
	{
		off.y += mp.y;
		if (off.y < offMin.y)
			off.y = offMin.y;
		else if (off.y > offMax.y)
			off.y = offMax.y;
	}

	public void scrollWheel(iPoint mp)
	{
		off.y += mp.y * 10.0f;

		if (off.y < offMin.y)
			off.y = offMin.y;
		else if (off.y > offMax.y)
			off.y = offMax.y;
	}

	public iRect checkScrollbar(int total)
	{
		// 가로 크기 / 총 크기
		int miniWidth = (int)drawRt.size.width;
		int miniHeight = (int)drawRt.size.height;

		int mapWidth = (int)drawRt.size.width;
		int mapHeight = total;//총 크기

		// 칸수
		float numW = 1.0f * mapWidth / miniWidth;
		float numH = 1.0f * mapHeight / miniHeight;


		int bW = (int)barSize.width * miniWidth / mapWidth;
		int bH = (int)barSize.height * miniHeight / mapHeight;

		int bX = (int)Math.linear(off.x / offMin.x, 0, bW * (numW - 1));
		int bY = (int)Math.linear(off.y / offMin.y, 0, bH * (numH - 1));

		return new iRect(bX, bY, bW, bH);
	}
}

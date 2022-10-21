using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class MainCamera : MonoBehaviour
{
	// 개발 해상도(1080p, 720p)
	public static int devWidth = 1280, devHeight = 720;
	public static GameObject mainCamera = null;

	//mouse
	public static MethodMouse[] methodMouse = new MethodMouse[100];
	public static int numMethodMouse = 0;
	public static void addMethodMouse(MethodMouse mm)
	{
		methodMouse[numMethodMouse] = mm;
		numMethodMouse++;
	}
	public static void destroyMethodMouse(MethodMouse mm)
	{
		for (int i = 0; i < numMethodMouse; i++)
		{
			if (methodMouse[i] == mm)
			{
				numMethodMouse--;
				for (int j = i; j < numMethodMouse; j++)
				{
					methodMouse[i] = methodMouse[j];
				}
				return;
			}
		}
	}
	public static void runMethodMouse(iKeystate stat, iPoint point)
	{
		for (int i = numMethodMouse - 1; i > -1; i--)
		{
			if (methodMouse[i](stat, point))
				return;
		}
	}
	bool drag;
	Vector3 prevV;

	//wheel
	public static MethodWheel[] methodWheel = new MethodWheel[100];
	public static int numMethodWheel = 0;
	public static void addMethodWheel(MethodWheel mm)
	{
		methodWheel[numMethodWheel] = mm;
		numMethodWheel++;
	}
	public static void destroyMethodWheel(MethodWheel mm)
	{
		for (int i = 0; i < numMethodWheel; i++)
		{
			if (methodWheel[i] == mm)
			{
				numMethodWheel--;
				for (int j = i; j < numMethodWheel; j++)
				{
					methodWheel[i] = methodWheel[j];
				}
				return;
			}
		}
	}
	public static void runMethodWheel(iPoint point)
	{
		for (int i = numMethodWheel - 1; i > -1; i--)
		{
			if (methodWheel[i](point))
				return;
		}
	}

	//keyboard
	public static MethodKeyboard[] methodKeyboard = new MethodKeyboard[100];
	public static int numMethodKeyboard = 0;
	public static void addMethodKeyboard(MethodKeyboard mm)
	{
		methodKeyboard[numMethodKeyboard] = mm;
		numMethodKeyboard++;
	}
	public static void destroyMethodKeyboard(MethodKeyboard mm)
	{
		for(int i = 0; i<numMethodKeyboard; i++)
		{
			if(methodKeyboard[i] == mm)
			{
				numMethodKeyboard--;
				for(int j = i; j< numMethodKeyboard; j++)
				{
					methodKeyboard[i] = methodKeyboard[j];
				}
				return;
			}
		}
	}
	public static void runMethodKeyboard(iKeystate stat, iKeyboard key)
	{
		for (int i = numMethodKeyboard - 1; i > -1; i--)
		{
			if (methodKeyboard[i](stat, key))
				return;
		}
	}

	void Start()
	{
		drag = false;

		loadGameHierachy();
		addMethodMouse(keyGameHierachy);
		mainCamera = gameObject;
		new Main();		
		gameObject.AddComponent<Event>();
	}

	void Update()
	{
		iGUI.setResolution(devWidth, devHeight);

		// ctrl
		updateMouse();
		updateWheel();
		updateKeyboard();

		// view
		drawGameHierachy();
	}

	void updateMouse()
	{
		int btn = 0;// 0:left, 1:right, 2:wheel, 3foward, 4back
		if (Input.GetMouseButtonDown(btn))
		{
			iPoint p = mousePosition();
			//Debug.LogFormat($"Began p({p.x},{p.y})");
			drag = true;
			prevV = Input.mousePosition;// 누르자 말자 Moved 안들어오게 방지

			runMethodMouse(iKeystate.Began, p);
		}
		else if (Input.GetMouseButtonUp(btn))
		{
			iPoint p = mousePosition();
			//Debug.LogFormat($"Ended p({p.x},{p.y})");
			drag = false;

			runMethodMouse(iKeystate.Ended, p);
		}

		if (drag)
		{
			Vector3 v = Input.mousePosition;
			if (prevV == v)
				return;
			prevV = v;

			iPoint p = mousePosition();
			//Debug.LogFormat($"Moved p({p.x},{p.y})");

			runMethodMouse(iKeystate.Moved, p);
		}
	}

	void updateWheel()
	{
		if (methodWheel == null)
			return;

		if (Input.mouseScrollDelta != Vector2.zero)
		{
			runMethodWheel(new iPoint(Input.mouseScrollDelta.x,
									Input.mouseScrollDelta.y));
		}
	}

	void updateKeyboard()
	{
		if (methodKeyboard == null)
			return;

		for (int i = 0; i < kc.Length; i++)
		{
			KeyCode c = kc[i];

			if (Input.GetKeyDown(c))
				runMethodKeyboard(iKeystate.Began, (iKeyboard)i);
			else if (Input.GetKeyUp(c))
				runMethodKeyboard(iKeystate.Ended, (iKeyboard)i);
			else if (Input.GetKey(c))
				runMethodKeyboard(iKeystate.Moved, (iKeyboard)i);
		}
	}
	private KeyCode[] kc = new KeyCode[]
	{
		KeyCode.LeftArrow, KeyCode.RightArrow,
		KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Space, KeyCode.Escape
	};

	public static Vector3 iPointToVector3(iPoint p)
	{
		return new Vector3(p.x - devWidth / 2, devHeight / 2 - p.y, 0f);
	}

	public static iPoint vector3ToiPoint(Vector3 v)
	{
		return new iPoint(v.x + devWidth / 2, devHeight / 2 - v.y);
	}

	public static iPoint mousePosition()
	{
		int sw = Screen.width, sh = Screen.height;
		float vx = Camera.main.rect.x * sw;
		float vy = Camera.main.rect.y * sh;
		float vw = Camera.main.rect.width * sw;
		float vh = Camera.main.rect.height * sh;

		Vector3 v = Input.mousePosition;
		iPoint p = new iPoint((v.x - vx) / vw * devWidth,
								(1f - (v.y - vy) / vh) * devHeight);
		//Debug.LogFormat($"screen({sw},{sh}) : input({v.x},{v.y}) => use({p.x},{p.y})");
		return p;
	}

	// ===========================================================
	// GameHierachy
	// ===========================================================
	public static MainCamera mc;

	void loadGameHierachy()
	{
		mc = this;

	}

	void drawGameHierachy()
	{

	}

	bool keyGameHierachy(iKeystate stat, iPoint point)
	{
		return false;
	}
}

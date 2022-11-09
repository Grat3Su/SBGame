using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gGUI : iGUI
{
	~gGUI()
	{
		DestroyImmediate(texFbo);		
	}

	bool initialize;
	void Start()
	{
		init();

		texFbo = new RenderTexture(MainCamera.devWidth,
									MainCamera.devHeight, 32,
									RenderTextureFormat.ARGB32);

		Camera.onPreCull = onPreCull;
		Camera.onPreRender = onPrev;
		Camera.onPostRender = onEnd;

		initialize = true;
	}
	//void Update() { }

	RenderTexture texFbo;
	RenderTexture texBack;
	Rect rtBack;

	public void onPrev(Camera c)
	{
		texBack = c.targetTexture;
		c.targetTexture = texFbo;

		rtBack = Camera.main.rect;
		Camera.main.rect = new Rect(0, 0, 1, 1);
	}
	// void OnRenderObject(){}
	public void onEnd(Camera c)
	{
		c.targetTexture = texBack;

		Camera.main.rect = rtBack;
	}

	void onPreCull(Camera c)
	{
		preCull();
	}

	void OnPreCull()
	{
		preCull();
	}

	void OnGUI()
	{
		// 유니티에서 키 입력, 리페인팅 등 이벤트를 체크한다.
		// 현재 유니티가 그리는 속도에 맞춰져있기 때문에 OnGUI가 update랑 속도가 같아진다.
		
		if (UnityEngine.Event.current.type != EventType.Repaint)
			return;
		float delta = Time.deltaTime;//Time.frameCount;

		if (Input.GetKeyDown(KeyCode.Space))//리 페인팅 이벤트만 체크해서 안들어옴
			Debug.Log("space");

#if true// rt : onPrev() ~ onEnd()
		GL.Clear(true, true, Color.black);
		setProject();
		setRGBA(1, 1, 1, 1);
		drawImage(texFbo, 0, 0, TOP | LEFT);
#endif

		// 0. onPrev : c.targetTexture = rt;
		texBack = RenderTexture.active;
		RenderTexture.active = texFbo;
		//rtBack = Camera.main.rect;
		//Camera.main.rect = new Rect(0, 0, 1, 1);
		GUI.matrix = Matrix4x4.TRS(
			Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		// 1. OnRenderObject
		if (initialize)
		{
			MainCamera.addMethodMouse(new MethodMouse(key));
			MainCamera.addMethodKeyboard(new MethodKeyboard(keyboard));
			MainCamera.addMethodWheel(new MethodWheel(wheel));
			load();

			initialize = false;
		}
		iStrTex.runSt();
		draw(delta);

		// 2. onEnd : c.targetTexture = backupRt;
		RenderTexture.active = texBack;
		//Camera.main.rect = rtBack;
		setProject();

#if true// rt : 0 ~ 2 : drawGui
		setRGBA(1, 1, 1, 1);
		drawImage(texFbo, 0, 0, TOP | LEFT);
#endif
	}

	// ===========================================================
	// Game
	// ===========================================================
	public virtual void load()
	{
		// do nothing
	}

	public virtual void free()
	{
	}

	public virtual void draw(float dt)
	{
		// do nothing
	}

	public virtual bool key(iKeystate stat, iPoint point)
	{
		// do nothing
		return false;
	}

	public virtual bool keyboard(iKeystate stat, iKeyboard key)
	{
		// do nothing
		return false;
	}


	public virtual bool wheel(iPoint point)
	{
		// do nothing
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gGUI : iGUI
{
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

		MainCamera.addMethodMouse(new MethodMouse(key));
		MainCamera.addMethodWheel(new MethodWheel(wheel));
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

	int prevFrameCount = -1;
	int prevFramecountDelta = -1;
	void OnGUI()
	{
#if false// 첫번째만 그리는 로직(한 프레임에 딱 1번 그림)
		// #issue 안그릴때는, 화면을 검은색으로 안 비우도록 하는 방법 모름!!
		Camera.main.clearFlags = CameraClearFlags.Nothing;
		if (prevFrameCount == Time.frameCount)
		{
			Debug.Log("그림 XXXXXXXXX");
			return;
		}
		Debug.Log("그림 OOOOOOOO");
		prevFrameCount = Time.frameCount;
#else// 첫번째만 안그리는 로직(한 프레임에 1번 이상 그림)
		if (prevFrameCount != Time.frameCount)
		{
			//Debug.Log("그림 XXXXXXXXX");
			prevFrameCount = Time.frameCount;
			return;
		}
		//Debug.Log("그림 OOOOOOOO");
#endif

		float delta = 0.0f;
		if (prevFramecountDelta != Time.frameCount)
		{
			prevFramecountDelta = Time.frameCount;
			delta = Time.deltaTime;
		}

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
	public virtual bool wheel(iPoint point)
	{
		// do nothing
		return false;
	}
}

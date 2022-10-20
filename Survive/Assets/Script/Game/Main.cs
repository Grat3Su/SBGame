using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main
{
	public static Main me;

	gGUI currScene, nextScene;
	GameObject maincamera;

	public Main()
	{
		me = this;
		maincamera = GameObject.Find("Main Camera");
		currScene = createGameObject("Intro");
		nextScene = null;
	}

	public void reset(string name)
	{
		nextScene = createGameObject(name);

		//GameObject.Destroy(currScene.gameObject);
		currScene.free();
		GameObject.Destroy(currScene);
		currScene = nextScene;
	}

	gGUI createGameObject(string nameGUI)
	{
		//GameObject go = new GameObject(nameGUI);
		GameObject go = maincamera;

		var type = System.Type.GetType(nameGUI);
		gGUI scene = (gGUI)go.AddComponent(type);
		return scene;
	}
}

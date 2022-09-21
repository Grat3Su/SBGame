using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;


public class Intro : gGUI
{
	public override void load()
	{
	}

	public override void free()
	{

	}

	public override void draw(float dt)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 1);
		fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);

		setRGBA(1, 0, 0, 1);
		fillRect(100, 100, 300, 200);

	}

	public override bool key(iKeystate stat, iPoint point)
	{
		if (stat == iKeystate.Began)
		{
			Main.me.reset("Proc");
		}
		return false;
	}
}

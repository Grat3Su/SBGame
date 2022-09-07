using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iImage
{
	public List<iTexture> listTex;

	public iTexture tex;
	public iPoint position;

	public bool animation;
	public int repeatIdx, repeatNum; // 0 : loop, 1 ~ : count
	public int frame;
	public float frameDt, _frameDt;
	public float scale;

	public iImage()
	{
		listTex = new List<iTexture>();

		//tex;
		position = new iPoint(0, 0);

		animation = false;
		repeatIdx = 0;
		repeatNum = 0;
		frame = 0;
		_frameDt = 0.0167f;// 1 / 60
		frameDt = 0.0f;
		scale = 1.0f;
	}

	public void add(iTexture t)
	{
		listTex.Add(t);
		t.retainCount++;
	}

	public void set(int index)
	{
		frame = index;
	}

	public void paint(float dt)
	{
		paint(dt, new iPoint(0, 0));
	}
	public void paint(float dt, iPoint off)
	{
		if (animation)
		{
			frameDt += dt;
			if (frameDt >= _frameDt)
			{
				frameDt -= _frameDt;
				frame++;

				if (frame == listTex.Count)
				{
					frame = 0;
					repeatIdx++;

					if (repeatNum != 0)
					{
						if (repeatIdx == repeatNum)
						{
							animation = false;
						}
					}
				}
			}
		}

		tex = listTex[frame];
		Texture t = tex.tex;
		//iGUI.instance.drawImage(t, position + off, iGUI.TOP | iGUI.LEFT);
		off += position;
		if (scale != 1.0f)
		{
			off.x += (1 - scale) * t.width / 2;
			off.y += (1 - scale) * t.height / 2;
		}
		iGUI.instance.drawImage(t, off.x, off.y, scale, scale,
			iGUI.TOP | iGUI.LEFT, 2, 0, iGUI.REVERSE_NONE);
	}

	public void startAnimation()
	{
		animation = true;
		repeatIdx = 0;
		frame = 0;
		frameDt = 0.0f;
	}

	public iRect touchRect()
	{
		return touchRect(new iPoint(0, 0), new iSize(0, 0));
	}

	public iRect touchRect(iPoint off, iSize s)
	{
		return new iRect(position.x + off.x - s.width / 2,
							position.y + off.y - s.height / 2,
							tex.tex.width + s.width,
							tex.tex.height + s.height);
	}
}


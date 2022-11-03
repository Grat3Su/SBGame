using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

namespace STD
{
	public class iImage
	{
		public List<iTexture> listTex;

		public iTexture tex;
		public iPoint position;

		public bool animation;
		public int repeatIdx, repeatNum; // 0 : loop, 1 ~ : count
		public int frame;
		public float frameDt, _frameDt;
		public float scale, alpha;

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
			alpha = 1.0f;
		}

		public iImage clone()
		{
			iImage img = new iImage();
			for (int i = 0; i < listTex.Count; i++)
				img.add(listTex[i]);

			img.tex = tex;
			img.position = position;

			img.animation = animation;
			img.repeatIdx = repeatIdx;
			img.repeatNum = repeatNum;
			img.frame = frame;
			img.frameDt = frameDt;
			img._frameDt = _frameDt;
			img.scale = scale;

			return img;
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

						if (repeatNum == 0)
							;// loop
						else// if (repeatNum != 0)
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
			Color c = iGUI.instance.getStringRGBA();
			iGUI.instance.setRGBA(c.r, c.g, c.b, c.a * alpha);
			iGUI.instance.drawImage(t, off.x, off.y, scale, scale,
				iGUI.TOP | iGUI.LEFT, 2, 0, iGUI.REVERSE_NONE);
			iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
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

		public iPoint center()
		{
			return new iPoint(position.x + tex.tex.width / 2,
								position.y + tex.tex.height / 2);
		}
		public iPoint center(iPoint off)
		{
			return new iPoint(off.x + position.x + tex.tex.width / 2,
								off.y + position.y + tex.tex.height / 2);
		}
	}
}



// 메모리가 부족할 경우는 주석 처리
#define RenterTextureScale

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using STD;

namespace STD
{
	public delegate void MethodPopOpenClose(iPopup pop);
	// #issue 동일한 타입 재정의
	public delegate void MethodPopDraw(float dt, iPopup pop, iPoint zero);

	public enum iPopupStyle
	{
		alpha = 0,
		move,
		zoom,
		zoomRotate,
	}

	public enum iPopupState
	{
		open = 0,
		proc,
		close
	}

	public class iPopup
	{
		static RenderTexture renderTexture = null;

		public List<iImage> listImg;

		public iPopupStyle style;
		public iPoint openPoint, closePoint;
		public iPopupState state;
		public float aniDt, _aniDt;
		public bool bShow;
		public MethodPopOpenClose methodOpen, methodClose;
		public MethodPopDraw methodDrawBefore, methodDrawAfter;
		public int selected;

		public iPopup()
		{
			if (renderTexture == null)
			{
#if RenterTextureScale
				renderTexture = new RenderTexture(
					MainCamera.devWidth * 2,
					MainCamera.devHeight * 2, 32,
					RenderTextureFormat.ARGB32);
#else
				renderTexture = new RenderTexture(
					MainCamera.devWidth,
					MainCamera.devHeight, 32,
					RenderTextureFormat.ARGB32);
#endif
			}

			listImg = new List<iImage>();

			style = iPopupStyle.alpha;
			state = iPopupState.close;
			openPoint = new iPoint(0, 0);
			closePoint = new iPoint(0, 0);
			_aniDt = 0.5f;
			aniDt = 0.0f;
			bShow = false;
			methodOpen = null;
			methodClose = null;
			methodDrawBefore = null;
			methodDrawAfter = null;
			selected = -1;
		}

		public void add(iImage i)
		{
			listImg.Add(i);
		}

		public void show(bool show)
		{
			if (show)
			{
				bShow = true;
				state = iPopupState.open;
			}
			else
			{
				//bShow = false;
				state = iPopupState.close;
			}
			aniDt = 0.0f;// _aniDt = 0.5f default
		}

		public void paint(float dt)
		{
			if (bShow == false)
				return;

			float alpha = 1.0f;
			iPoint position = new iPoint(0, 0);
			float scale = 1.0f;
			float degree = 0.0f;

			if (style == iPopupStyle.alpha)
			{
				if (state == iPopupState.open)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						state = iPopupState.proc;
						if (methodOpen != null)
							methodOpen(this);
					}
					alpha = aniDt / _aniDt;
				}
				else if (state == iPopupState.proc)
				{
					alpha = 1f;
				}
				else if (state == iPopupState.close)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						bShow = false;
						if (methodClose != null)
						{
							methodClose(this);
							return;
						}
					}

					alpha = 1f - aniDt / _aniDt;// alpha 1 -> 0
				}
				position = closePoint;
			}
			else if (style == iPopupStyle.move)
			{
				if (state == iPopupState.open)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						state = iPopupState.proc;
						if (methodOpen != null)
							methodOpen(this);
					}
					float r = Math.linear(aniDt / _aniDt, 0, 1);
					//float r = Math.easeIn(aniDt / _aniDt, 0, 1);
					//float r = Math.easeOut(aniDt / _aniDt, 0, 1);
					position = openPoint * (1 - r) + closePoint * r;
					//Debug.LogFormat($"open {r} {position.x}, {position.y}");
				}
				else if (state == iPopupState.proc)
				{
					position = closePoint;
				}
				else if (state == iPopupState.close)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						bShow = false;
						if (methodClose != null)
						{
							methodClose(this);
							return;
						}
					}

					//float r = 1f - aniDt / _aniDt;// alpha 1 -> 0
					float r = Math.linear(aniDt / _aniDt, 1, 0);
					//float r = Math.easeIn(aniDt / _aniDt, 1, 0);// alpha 1 -> 0
					position = openPoint * (1 - r) + closePoint * r;
					//Debug.LogFormat($"close {r} {position.x}, {position.y}");
				}
			}
			else if (style == iPopupStyle.zoom)
			{
				if (state == iPopupState.open)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						state = iPopupState.proc;
						if (methodOpen != null)
							methodOpen(this);
					}
					float r = aniDt / _aniDt;
					position = openPoint * (1 - r) + closePoint * r;
					scale = r;
				}
				else if (state == iPopupState.proc)
				{
					alpha = 1f;
					position = closePoint;
					scale = 1f;
				}
				else if (state == iPopupState.close)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						bShow = false;
						if (methodClose != null)
						{
							methodClose(this);
							return;
						}
					}

					float r = 1f - aniDt / _aniDt;// alpha 1 -> 0
					position = openPoint * (1 - r) + closePoint * r;
					scale = r;
				}
			}
			else if (style == iPopupStyle.zoomRotate)
			{
				if (state == iPopupState.open)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						state = iPopupState.proc;
						if (methodOpen != null)
							methodOpen(this);
					}
					float r = aniDt / _aniDt;
					position = openPoint * (1 - r) + closePoint * r;
					scale = r;
					degree = 360 * r;
				}
				else if (state == iPopupState.proc)
				{
					position = closePoint;
					scale = 1f;
					degree = 0.0f;
				}
				else if (state == iPopupState.close)
				{
					aniDt += dt;
					if (aniDt >= _aniDt)
					{
						aniDt = _aniDt;
						bShow = false;
						if (methodClose != null)
						{
							methodClose(this);
							return;
						}
					}

					float r = 1f - aniDt / _aniDt;// alpha 1 -> 0
					position = openPoint * (1 - r) + closePoint * r;
					scale = r;
					degree = 360 * r;
				}
			}

			Color c = iGUI.instance.getStringRGBA();
			iGUI.instance.setRGBA(1, 1, 1, alpha);

#if false
			for (int i = 0; i < listImg.Count; i++)
			{
				iImage img = listImg[i];
				img.paint(dt, position);
			}
#else
			RenderTexture bkT = RenderTexture.active;
			RenderTexture.active = renderTexture;
			//Rect bkR = Camera.main.rect;
			//Camera.main.rect = new Rect(0, 0, 1, 1);
			Matrix4x4 matrixBk = GUI.matrix;
			GUI.matrix = Matrix4x4.TRS(
				Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
#if !RenterTextureScale
			Matrix4x4 mv = Matrix4x4.TRS(
				Vector3.zero, Quaternion.identity,
				new Vector3(0.5f, 0.5f, 1));
			GUI.matrix *= mv;
#endif

			GL.Clear(true, true, Color.clear);
			//iGUI.instance.setRGBA(1f, 0f, 1f, 0.5f);
			//iGUI.instance.fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);
			//iGUI.instance.setRGBA(1f, 1f, 1f, 1f);

			float left = 1000f, right = 0f, top = 1000f, bottom = 0f;
			for (int i = 0; i < listImg.Count; i++)
			{
				iImage img = listImg[i];
				if (img.tex == null)
					break;

				if (left > img.position.x)
					left = img.position.x;
				if (right < img.position.x + img.tex.tex.width)
					right = img.position.x + img.tex.tex.width;
				if (top > img.position.y)
					top = img.position.y;
				if (bottom < img.position.y + img.tex.tex.height)
					bottom = img.position.y + img.tex.tex.height;
			}
			float w = (right - left);
			float h = (bottom - top);
			iPoint gCenter = new iPoint(left + w / 2, top + h / 2);
			iPoint mCenter = new iPoint(renderTexture.width / 2, renderTexture.height / 2);
			iPoint move = mCenter - gCenter;
			//Debug.LogFormat($"rect({left}, {top}, {w}, {h}), move({move.x}, {move.y})");

			if (methodDrawBefore != null)
				methodDrawBefore(dt, this, move);
			for (int i = 0; i < listImg.Count; i++)
			{
				iImage img = listImg[i];
				img.paint(dt, move);
			}
			if (methodDrawAfter != null)
				methodDrawAfter(dt, this, move);

			RenderTexture.active = bkT;
			//Camera.main.rect = bkR;
			GUI.matrix = matrixBk;

			iGUI.instance.setRGBA(1, 1, 1, 1);
			//iGUI.instance.drawImage(renderTexture, position, iGUI.TOP|iGUI.LEFT);
			iPoint p = position - move * scale;
			//degree = 0;
#if RenterTextureScale
			iGUI.instance.drawImage(renderTexture, p.x, p.y, scale, scale,
				iGUI.TOP | iGUI.LEFT, 2, degree, iGUI.REVERSE_NONE);
#else
			iGUI.instance.drawImage(renderTexture, p.x, p.y, scale * 2, scale * 2,
				iGUI.TOP | iGUI.LEFT, 2, degree, iGUI.REVERSE_NONE);
#endif

#endif
			iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
		}
	}
}



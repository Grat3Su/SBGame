using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iGUI : MonoBehaviour
{
    public static iGUI instance = null;
    public iGUI()
    {
        instance = this;
    }

    Texture2D texDot;
    Color color;
    float lineWidth;

    // image
    public const int TOP = 1, BOTTOM = 2, VCENTER = 4,
                    LEFT = 8, RIGHT = 16, HCENTER = 32;
    public const int REVERSE_NONE = 0, REVERSE_WIDTH = 1, REVERSE_HEIGHT = 2;

    // string
    string stringName = null;
    float stringSize = 20f;
    Color stringColor = Color.white;

    // ======================================================
    // init & buffer
    // ======================================================
    public void init()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(1, 1, 1));
        tex.Apply();
        texDot = tex;
        lineWidth = 1f;
        color = Color.white;
    }
    
	public void OnPreCull()
	{
        GL.Clear(true, true, Color.clear);
        //GL.Clear(true, true, new Color(0, 0, 1, 0.5f));
    }

    public void setProject()
	{
        GUI.matrix = Matrix4x4.TRS(
                new Vector3(Camera.main.rect.x * Screen.width,
                            Camera.main.rect.y * Screen.height, 0),                
                Quaternion.identity,                
                new Vector3(Camera.main.rect.width * Screen.width / MainCamera.devWidth,
                            Camera.main.rect.height * Screen.height / MainCamera.devHeight, 1)
                );//사용할 크기 비례
    }

    public static void setResolution(int devWidth, int devHeight)
	{
        setResolutionClip(devWidth, devHeight);
    }

    public static void setResolutionClip(int devWidth, int devHeight)
    {//해상도가 바뀔 때만 체크한다.
        Screen.SetResolution(devWidth, devHeight, false);

        float r0 = (float)devWidth / devHeight;

        int width = Screen.width, height = Screen.height;
        float r1 = (float)width / height;

        if (r0 < r1)// 세로가 길때
        {
            float w = r0 / r1;
            float x = (1 - w) / 2;
            Camera.main.rect = new Rect(x, 0, w, 1);
        }
        else// 가로가 길때
        {
            float h = r1 / r0;
            float y = (1 - h) / 2;
            Camera.main.rect = new Rect(0, y, 1, h);
        }
    }

    public static void setResolutionFull(int devWidth, int devHeight)
    {
        Camera.main.rect = new Rect(0, 0, 1, 1);

        int width = Screen.width, height = Screen.height;
        // width : height = 4 : 3 = devWidth : h
        // h = height / width x devWidth
        float dh = (float)height / width * devWidth;
        Screen.SetResolution(devWidth, (int)dh, true);
        //Debug.LogFormat($"devResolution({devWidth}, {dh})");
    }
        public string getStringName()
    {
        return stringName;
    }
    public void setStringName(string name)
    {
        stringName = name;
    }
    public float getStringSize()
    {
        return stringSize;
    }
    public void setStringSize(float size)
    {
        stringSize = size;
    }    public Color getStringRGBA()
    {
        return stringColor;
    }
    public void setStringRGBA(float r, float g, float b, float a)
    {
        stringColor = new Color(r, g, b, a);
    }
    public iSize sizeOfString(string str)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.font = Resources.Load<Font>(name);
        style.fontSize = (int)stringSize;
        style.fontStyle = FontStyle.Normal;
        style.normal.textColor = stringColor;
        Vector2 size = style.CalcSize(new GUIContent(str));
        return new iSize(size.x, size.y);
    }

    public void drawString(string str, iPoint p, int anc)
    {
        drawString(str, p.x, p.y, anc);
    }
    public void drawString(string str, float x, float y, int anc = TOP | LEFT)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.font = Resources.Load<Font>(name);
        style.fontSize = (int)stringSize;
        style.fontStyle = FontStyle.Normal;
        style.normal.textColor = stringColor;
        style.hover.textColor = stringColor;
        Vector2 size = style.CalcSize(new GUIContent(str));
        switch (anc)
        {
            case TOP | LEFT: break;
            case TOP | RIGHT: x -= size.x; break;
            case TOP | HCENTER: x -= size.x / 2; break;
            case BOTTOM | LEFT: y -= size.y; break;
            case BOTTOM | RIGHT: x -= size.x; y -= size.y; break;
            case BOTTOM | HCENTER: x -= size.x / 2; y -= size.y; break;
            case VCENTER | LEFT: y -= size.y / 2; break;
            case VCENTER | RIGHT: x -= size.x; y -= size.y / 2; break;
            case VCENTER | HCENTER: x -= size.x / 2; y -= size.y / 2; break;
        }
        //마지막으로 그려진 색 반영되니까 조심
        GUI.color = color;// #issue
        GUI.Label(new Rect(x, y, size.x, size.y), str, style);
    }

    public void setRGBA(float r, float g, float b, float a)
    {
        color.r = r;
        color.g = g;
        color.b = b;
        color.a = a;
    }
    public void setLineWidth(float width)
    {
        lineWidth = width;
    }

    public void drawLine(iPoint s, iPoint e)
    {
        drawLine(s.x, s.y, e.x, e.y);
    }
    public void drawLine(float sx, float sy, float ex, float ey)
    {
        float cx = (sx + ex) / 2;
        float cy = (sy + ey) / 2;
        float dx = ex - sx;
        float dy = ey - sy;
        float len = Mathf.Sqrt(dx * dx + dy * dy);

        float degree = Math.angleDirection(sx, sy, ex, ey);
        drawImage(texDot, cx, cy, len, lineWidth, VCENTER | HCENTER, 2, -degree, REVERSE_NONE);
    }

    public void drawRect(float x, float y, float width, float height)
    {
        // top & bottom
        drawLine(x, y, x + width, y);
        drawLine(x, y + height - lineWidth, x + width, y + height - lineWidth);
        // left & right
        drawLine(x, y + lineWidth,
                    x, y + height - lineWidth);
        drawLine(x + width - lineWidth, y + lineWidth,
                    x + width - lineWidth, y + height - lineWidth);
    }

    public void fillRect(Rect r)
    {
        drawImage(texDot, r.x, r.y, r.width, r.height, TOP | LEFT, 2, 0, REVERSE_NONE);
    }

    public void fillRect(float x, float y, float width, float height)
    {
        drawImage(texDot, x, y, width, height, TOP | LEFT, 2, 0, REVERSE_NONE);
    }

    public void drawImage(Texture tex, iPoint p, int anc)
    {
        drawImage(tex, p.x, p.y, 1f, 1f, anc, 2, 0, REVERSE_NONE);
    }

    public void drawImage(Texture tex, float x, float y, int anc)
    {
        drawImage(tex, x, y, 1f, 1f, anc, 2, 0, REVERSE_NONE);
    }

    public void drawImage(Texture tex, iPoint p, float sx, float sy, int anc)
    {
        drawImage(tex, p.x, p.y, sx, sy, anc, 2, 0, REVERSE_NONE);
    }

    public void drawImage(Texture tex, iPoint p, float sx, float sy, int anc, int xyz, float degree, int reverse)
    {
        drawImage(tex, p.x, p.y, sx, sy, anc, xyz, degree, reverse);
    }

    public void drawImage(Texture tex, float x, float y, float sx, float sy, int anc, int xyz, float degree, int reverse,
        float tx = 0f, float ty = 0f, float tw = 1f, float th = 1f)
    {
        float w = tex.width * sx;
        float h = tex.height * sy;

        switch (anc)
        {
            case TOP | LEFT: break;
            case TOP | RIGHT: x -= w; break;
            case TOP | HCENTER: x -= w / 2; break;
            case BOTTOM | LEFT: y -= h; break;
            case BOTTOM | RIGHT: x -= w; y -= h; break;
            case BOTTOM | HCENTER: x -= w / 2; y -= h; break;
            case VCENTER | LEFT: y -= h / 2; break;
            case VCENTER | RIGHT: x -= w; y -= h / 2; break;
            case VCENTER | HCENTER: x -= w / 2; y -= h / 2; break;
        }

        Matrix4x4 matrixPrjection = GUI.matrix;// matrixProjection(개발좌표에서 화면좌표)

        Matrix4x4 matrixModelview = Matrix4x4.TRS(
            new Vector3(x + w / 2, y + h / 2, 0),
            Quaternion.Euler(xyz == 0 ? degree : 0,
                                xyz == 1 ? degree : 0,
                                xyz == 2 ? degree : 0),
            new Vector3(1, 1, 1)
            );
        GUI.matrix = GUI.matrix * matrixModelview;
        if (reverse == REVERSE_WIDTH)
        {
            Matrix4x4 m0 = Matrix4x4.TRS(
                new Vector3(0, 0, 0),
                Quaternion.Euler(0, 180, 0),
                new Vector3(1, 1, 1)
                );
            GUI.matrix = GUI.matrix * m0;
        }
        else if (reverse == REVERSE_HEIGHT)
        {
            Matrix4x4 m1 = Matrix4x4.TRS(
                new Vector3(0, 0, 0),
                Quaternion.Euler(180, 0, 0),
                new Vector3(1, 1, 1)
                );
            GUI.matrix = GUI.matrix * m1;
        }

#if false
			GUI.DrawTexture(new Rect(-w / 2, -h / 2, w, h), tex);
//#elif true
#elif false
			GUI.DrawTexture(new Rect(-w / 2, -h / 2, w, h), tex, ScaleMode.StretchToFill, true, h / w, color, 0, 0);
#elif true
        GUI.color = color;// #issue
        GUI.DrawTextureWithTexCoords(new Rect(-w / 2, -h / 2, w, h), tex, new Rect(tx, ty, tw, th), true);
#else
			ScaleMode scaleMode = ScaleMode.ScaleToFit;
			float imageAspect = h / w;
			float borderWidth = 0, borderRadius = 0;
			GUI.DrawTexture(new Rect(-w / 2, -h / 2, w, h), tex, scaleMode, true, imageAspect, color, borderWidth, borderRadius);
#endif
        GUI.matrix = matrixPrjection;
    }
}

class Math
	{
		public static float angleDirection(iPoint s, iPoint e)
		{
			return angleDirection(s.x, s.y, e.x, e.y);
		}
		public static float angleDirection(float sx, float sy, float ex, float ey)
		{
			sy = MainCamera.devHeight - sy;
			ey = MainCamera.devHeight - ey;

			iPoint v;
			v.x = ex - sx;
			v.y = ey - sy;
			//return Mathf.Atan2(v.y, v.x) * 180 / Mathf.PI;
			return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		}

		public static float angleRotate(float currDegree, float targetDegree, float speed)
		{
			if( currDegree==targetDegree )
				return currDegree;

			float diff = targetDegree - currDegree;
			float ad = Mathf.Abs(diff);
			if (ad > 180)
			{
				//ad = 180;
				ad = Mathf.Abs(ad - 360);
			}
			float r = speed / ad;
			if (r < 1.0f)
			{
				//currDegree = Math.angleRate(currDegree, targetDegree, r);
				if (diff > 360) diff -= 360;
				if (diff > 180) diff -= 360;
				currDegree = currDegree + diff * r;
			}
			else
				currDegree = targetDegree;

			return currDegree;
		}
	}
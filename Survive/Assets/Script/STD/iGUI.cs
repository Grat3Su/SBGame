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
        public void preCull()
    {
        GL.Clear(true, true, Color.clear);
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

    static iSize screenSize = new iSize(MainCamera.devWidth, MainCamera.devHeight);
    public static void setNewResolution()//나중에..
    {//해상도가 바뀔 때만 체크한다.
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        iSize newSize = new iSize(width, height);
        
        if (screenSize == newSize)
            return;

        if(screenSize.width == newSize.width)//세로가 변했을 때
		{
            newSize.width = (16 * newSize.height) / 9;
        }
        else if(screenSize.height == newSize.height)//가로가 변했을 때
        {
            newSize.height = (9 * newSize.width) / 16;
        }

        screenSize = newSize;
        MainCamera.devWidth = (int)newSize.width;
        MainCamera.devHeight= (int)newSize.height;
        Screen.SetResolution((int)newSize.width, (int)newSize.height, false);

        Camera.main.rect = new Rect(0, 0, 1, 1);        
    }

    public static void setResolution(int devWidth, int devHeight)
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
    public static void setResolutionFullWindow(int devWidth, int devHeight)
    {
        Camera.main.rect = new Rect(0, 0, 1, 1);
        int width = Screen.width, height = Screen.height;
        float dh = (float)height / width * devWidth;
        Screen.SetResolution(devWidth, (int)dh, false);
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
        style.font = Resources.Load<Font>(stringName);
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
        style.font = Resources.Load<Font>(stringName);
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
#elif false// final
        GUI.color = color;// #issue
        GUI.DrawTextureWithTexCoords(new Rect(-w / 2, -h / 2, w, h), tex, new Rect(tx, ty, tw, th), true);
#elif true// curr
		loadShader();
		Material m = mat[matIndex];
        m.SetTexture("_MainTex", tex);//마테리얼 내에서 텍스트 변경
		m.SetColor("inColor", color);
		m.SetFloat("x", fadeCircle.x);
		m.SetFloat("y", fadeCircle.y);
		m.SetFloat("radius", fadeCircle.z);
		m.SetColor("fadeColor", fadeColor);
		Graphics.DrawTexture(new Rect(-w / 2, -h / 2, w, h), tex, new Rect(tx, ty, tw, th), 0, 0, 0, 0, m);
#else
        ScaleMode scaleMode = ScaleMode.ScaleToFit;
			float imageAspect = h / w;
			float borderWidth = 0, borderRadius = 0;
			GUI.DrawTexture(new Rect(-w / 2, -h / 2, w, h), tex, scaleMode, true, imageAspect, color, borderWidth, borderRadius);
#endif
        GUI.matrix = matrixPrjection;
    }
    Material[] mat = null;
	int matIndex = 0;
	
	void loadShader()
	{
		if (mat != null)
			return;

		string[] path = new string[] { "STD", "FADE", "OUTLINE", "SHINING" };
		mat = new Material[path.Length];
		for(int i=0; i<path.Length; i++)
		{
			Shader shader = Shader.Find("Unlit/" + path[i]);
			mat[i] = new Material(shader);
		}
	}

	public void setShader(int index)
	{
		matIndex = index;
	}

	Vector4 fadeCircle;
	Color fadeColor;
	public void setShaderFade(float x, float y, float r, Color c)
	{
		fadeCircle = new Vector4(x, y, 800*r, 0);
		fadeColor = c;
	}

    public static void drawTexture(Rect r, RenderTexture tex)
	{
        GUI.DrawTexture(r, tex);
    }
}

class Math
{
    public static float linear(float rate, float a, float b)
    {
        rate = Mathf.Clamp(rate, 0, 1);
        return a * (1 - rate) + b * rate;
    }

    public static iPoint linear(float rate, iPoint a, iPoint b)
    {
        rate = Mathf.Clamp(rate, 0, 1);
        return a * (1 - rate) + b * rate;
    }


    public static float easeIn(float rate, float a, float b)
    {// y = x^2
        rate = Mathf.Clamp(rate, 0, 1);
        rate = rate * rate * rate;
        return a * (1 - rate) + b * rate;
    }

    public static float easeOut(float rate, float a, float b)
    {// y = 1 - (x-1)^2
        rate = Mathf.Clamp(rate, 0, 1);
#if false// #issue bug
			rate = rate - 1;
			rate = 1f - rate * rate * rate;
#else
        rate = Mathf.Sin(90 * rate * Mathf.Deg2Rad);
        rate = Mathf.Sin(90 * rate * Mathf.Deg2Rad);
#endif
        return a * (1 - rate) + b * rate;
    }


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
        if (currDegree == targetDegree)
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

    public static int random(int min, int max)
	{
        return Random.Range(min, max);
	}
    public static float random(float min, float max)
    {
        return Random.Range(min, max);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void init()
	{
		
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
}

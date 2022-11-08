using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if true
public class SoundManager
{
	private static SoundManager sm;
	AudioClip[] clip;
	AudioSource[] soundSource;

	private SoundManager()
	{
        clip = new AudioClip[6];
		clip[0] = Resources.Load<AudioClip>("Lost Kingdom (Piano Menu)");
		clip[1] = Resources.Load<AudioClip>("RPG_Menu_Confirm_01");
		clip[2] = Resources.Load<AudioClip>("RPG_Menu_Twinkle_01");
		clip[3] = Resources.Load<AudioClip>("RPG_Menu_Confirm_02");
		clip[4] = Resources.Load<AudioClip>("RPG_Menu_Twinkle_02");
		clip[5] = Resources.Load<AudioClip>("RPG_Menu_Cancel_04");
		
		soundSource = new AudioSource[5];		
		for(int i = 0; i<soundSource.Length; i++)
		{
			soundSource[i] = MainCamera.mainCamera.AddComponent<AudioSource>();
			soundSource[i].clip = clip[i];
            soundSource[i].volume = 0.5f;
		}
	}

	public static SoundManager instance()
	{
		if (sm == null)
			sm = new SoundManager();
		return sm;
	}

	public void addClip(iSound st, AudioClip newClip)
	{
		int idx = -1;
		if (st == iSound.BGM)
			idx = 0;
		else if (st == iSound.ButtonClick)
			idx = 1;
		else if (st == iSound.PopUp)
			idx = 2;
		else if (st == iSound.Event)
			idx = 3;
		else if (st == iSound.NextDay)
			idx = 4;
		else if (st == iSound.TitleSound)
			idx = 5;

		if (soundSource[idx].isPlaying)
			soundSource[idx].Stop();

		soundSource[idx].clip = newClip;
		soundSource[idx].Play();
	}

    public void play(iSound st)
    {
        int idx = -1;
        if (st == iSound.BGM)
        {
            soundSource[0].loop = true;
            idx = 0;
        }
        else if (st == iSound.ButtonClick)
            idx = 1;
        else if (st == iSound.PopUp)
            idx = 2;
        else if (st == iSound.Event)
            idx = 3;
        else if (st == iSound.NextDay)
            idx = 4;
        else if (st == iSound.TitleSound)
            idx = 5;

        if (!soundSource[idx].isPlaying)
        {
            soundSource[idx].Play();
        }
    }

    public void playForce(iSound st)
    {
        int idx = -1;
        if (st == iSound.BGM)
        {
            soundSource[0].loop = true;
            idx = 0;
        }
        else if (st == iSound.ButtonClick)
            idx = 1;
        else if (st == iSound.PopUp)
            idx = 2;
        else if (st == iSound.Event)
            idx = 3;
        else if (st == iSound.NextDay)
            idx = 4;
        else if (st == iSound.TitleSound)
            idx = 5;

            soundSource[idx].Play();
    }
    public void stop(iSound st)
	{
		int idx = -1;
		if (st == iSound.BGM)
			idx = 0;
		else if (st == iSound.ButtonClick)
			idx = 1;
		else if (st == iSound.PopUp)
			idx = 2;
		else if (st == iSound.Event)
			idx = 3;
		else if (st == iSound.NextDay)
			idx = 4;
		else if (st == iSound.TitleSound)
			idx = 5;

		if (soundSource[idx].isPlaying)
		{
			soundSource[idx].Stop();
		}
	}
	public void stopAll()
	{
		for (int i = 0; i < soundSource.Length; i++)
		{
			if (soundSource[i].isPlaying)
			{
				soundSource[i].Stop();
			}
		}
	}

	public void volume(iSound st, bool reduce)
	{
        // BGM/효과음만 조절
        float dt = Time.deltaTime;

        int idx = -1;
		if (st == iSound.BGM)
			idx = 0;
		else
			idx = 1;

		if (reduce)
		{
			if (soundSource[idx].volume > 0)
				soundSource[idx].volume -= 0.25f;
			else
				soundSource[idx].volume = 0.0f;
		}
		else
		{
            if (idx == 0)
            {
                if(soundSource[0].volume < 1.0f)
                    soundSource[0].volume += 0.25f;
                else
                    soundSource[0].volume = 1f;
            }
            else
                for (int i = 1; i < soundSource.Length; i++)
                {
                    if (soundSource[i].volume < 1.0)
                        soundSource[i].volume += 0.25f;
                    else
                        soundSource[i].volume = 1f;
                }
		}
	}

	public string printVolume(iSound st)
	{
        string s = "";

        if (st == iSound.BGM)
            s = string.Format("{0:F0}", soundSource[0].volume * 100.0f);
        else
            s = string.Format("{0:F0}", soundSource[1].volume * 100.0f);
        return s;
	}

	public int intVolume(iSound st)
	{
		int vol = 0;
		int idx = 0;
		if (st == iSound.ButtonClick)
			idx = 1;
		if (soundSource[idx].volume < 0.1f)
			vol = 0;
		else if (soundSource[idx].volume < 0.26f)
				vol = 1;
		else if (soundSource[idx].volume < 0.51f)
				vol = 2;
		else if (soundSource[idx].volume < 0.76f)
				vol = 3;
		else if (soundSource[idx].volume < 1.1f)
				vol = 4;

		return vol;
	}
}

#else

public class SoundManager// : MonoBehaviour
{
	private SoundManager()
	{
		//if (GetComponents<AudioSource>() == null)
		//	gameObject.AddComponent<AudioSource>();
		//source = GetComponents<AudioSource>();
		audioSource = new AudioSource();
	}

	private static SoundManager sm = null;

	public static SoundManager share()
	{
		if (sm == null)
			sm = new SoundManager();
		return sm;
	}


	AudioSource[] source;
	AudioSource audioSource;
	public AudioClip[] clip;

	public static int numMethodSound = 0;

	public static MethodPlaySound[] methodSound = new MethodPlaySound[50];//사운드 관리

	public static void addMethodSound(MethodPlaySound mm)
	{
		methodSound[numMethodSound] = mm;
		numMethodSound--;
	}

	public static void destroyMethodSound(MethodPlaySound mm)
	{
		for (int i = 0; i < numMethodSound; i++)
		{
			if (methodSound[i] == mm)
			{
				numMethodSound--;
				for (int j = i; j < numMethodSound; j++)
				{
					methodSound[i] = methodSound[j];
				}
				return;
			}
		}
	}
	public static void runMethodSound(iSound sound)
	{
		for (int i = numMethodSound - 1; i > -1; i--)
		{
			if (methodSound[i](sound))
				return;
		}
	}

	// Update is called once per frame
	void Update()
	{
		//clip.
	}

	void addClip()
	{

	}

	void methodPlayEffect(iSound stat)
	{
		
	}

	void playOneShot(iSound stat)
	{
		if (stat == iSound.ButtonClick)
		{
		}
		else if (stat == iSound.Event)
		{
		}
		else if (stat == iSound.NextDay)
		{
		}
		else if (stat == iSound.PopUp)
		{
		}

	}
}
#endif

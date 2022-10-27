using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource source;
    AudioSource sourceBG;
    public AudioClip[] clip;

    int playCount;

    void Start()
    {
        source = GetComponent<AudioSource>();
        sourceBG = GetComponent<AudioSource>();
        playCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        source.Play();
        //clip.
    }

    void addClip()
	{
        
	}
    
    void playOneShot(iSound stat)
	{
        if(stat == iSound.ButtonClick)
		{
            source.clip = clip[0];
		}
        else if(stat == iSound.Event)
		{
            source.clip = clip[1];
        }
        else if(stat == iSound.NextDay)
		{
            source.clip = clip[2];
        }
        else if(stat == iSound.PopUp)
		{
            source.clip = clip[3];
        }

	}
}


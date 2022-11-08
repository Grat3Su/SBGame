using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum iKeystate
{
	Began = 0,  // pressed
	Moved,      // moved
	Ended,      // released
	Double,
};

public delegate bool MethodMouse(iKeystate stat, iPoint point);
public delegate bool MethodWheel(iPoint wheel);

public enum iKeyboard
{
	Left = 0,// a, A, 4, <-
	Right,
	Up,
	Down,
	Space,
	ESC,
};

public delegate bool MethodKeyboard(iKeystate stat, iKeyboard key);

public enum iSound
{
	BGM,
	ButtonClick,
	PopUp,
	Event,
	NextDay,
    TitleSound,
}

public delegate bool MethodPlaySound(iSound stat);
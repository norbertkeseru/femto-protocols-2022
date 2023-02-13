using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

        Application.targetFrameRate = 600;

        for (int i = 1; i < Display.displays.Length; i++)
		{
		    Display.displays[i].Activate();
			//tovabbi info; https://docs.unity3d.com/Manual/MultiDisplay.html
        }
    }
}
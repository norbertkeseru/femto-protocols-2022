using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waterRewardOblique : MonoBehaviour {

    public GameObject Player;
    public GameObject teleportationTarget;

	public Text telemetry;
	public float licktreshold;
    public float timer;
    public float droptimer;
    public float tapOpentime;
	public float timerPufftime;
	public float licked;
    bool reduce = false;
	bool dropped = false;


    private GramophoneDevice device;


    void Start()
    {
        device = GramophoneDevice.Instance();
    }
		
		
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			Player.GetComponent<PositionTracking>().ResetLick();
            reduce = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce=false;
			dropped=false;
			timer=timerPufftime; 
			droptimer=tapOpentime;
			Player.GetComponent<PositionTracking>().ResetLick();
			device.CloseB();
        }
    }

    void Update()
    {
        if (reduce == true)
        {
		    LeftZone();
			licked=Player.GetComponent<PositionTracking>().LickDisp();

		    if (licked>=licktreshold)
            {
			    timer=0;
		    }
            
            timer -= 1 * Time.deltaTime;
            LeftZone();
        }
		
        if ((timer <= 0) & (dropped==false))
        {
				Reward();
				LeftZone();
        }
    }

    void Reward()
    {
       // reduce = false;
        device.OpenB();
        Player.GetComponent<PositionTracking>().RewardHappens();
		droptimer -= 1 * Time.deltaTime;
		if (droptimer <= 0) {
			device.CloseB ();
            dropped=true;
		}
			
    }

	void LeftZone()
    {
		Player.GetComponent<PositionTracking>().LeftZone();
	}

    public void RewardReset()
    {
        reduce = false;
        dropped = false;
        timer = timerPufftime;
        droptimer = tapOpentime;
        Player.GetComponent<PositionTracking>().ResetLick();
        device.CloseB();
        reduce = true;
    }
}
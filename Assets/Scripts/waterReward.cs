using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waterReward : MonoBehaviour {

    public GameObject Player;
    public GameObject teleportationTarget;
	public Text telemetry;
    public float timer;
    public float droptimer;
    public float tapOpentime;
	public float timerPufftime;
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
        }
    }

    void Update()
    {
        if (reduce == true)
        {
            timer -= Time.deltaTime;
            LeftZone();
        }
		
        if ((timer <= 0)& (dropped==false))
        {
            Reward();
        }
    }
	
    void Reward()
    {
        reduce = false;
        device.OpenB();
        Player.GetComponent<PositionTracking>().RewardHappens();
		droptimer -= Time.deltaTime;
		if (droptimer <= 0)
        {
			device.CloseB ();
            dropped=true;
		}
    }

	void LeftZone()
    {
        Player.GetComponent<PositionTracking>().LeftZone();
	}
}
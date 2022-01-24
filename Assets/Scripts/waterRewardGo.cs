using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waterRewardAversiveGo : MonoBehaviour {

    public GameObject Player;
	public bool stopInZone;
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
    
			if (stopInZone==true){
			Player.GetComponent<MiceMovement>().Stop();
			}

        if (other.tag == "Player")
        {
            reduce = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
    

        if (other.tag == "Player")
        {
			
			Player.GetComponent<MiceMovement>().Restart();
			
            reduce=false;
			dropped=false;
			timer=timerPufftime; 
			droptimer=tapOpentime;
			Player.GetComponent<PositionTrackingGo>().ResetLick();
            
        }
    }

    void Update()
    {
		
  
	 
        if (reduce == true)
        {
		
            timer -= 1 * Time.deltaTime;
            PuffZone();
        }
		
        if ((timer <= 0)& (dropped==false))
        {
			    
				Reward();
				
        }
		
	  
		
	 /*    if (dropreduce == true)
        {
            cleantimer -= 1 * Time.deltaTime;
        }
		
		if ((cleantimer<=0)& (vacuumed==false))
        {
				Vacuum();
				
		
        } 
		telemetry.text = "Lick: "+ Player.GetComponent<PositionTracking>().LickDisp();
		telemetry.color = Color.red; */
		

    }

		
/* 	void Vacuum() {
        dropreduce=false;
        device.OpenC();
		vacuumtimer -= 1 * Time.deltaTime;
		if (vacuumtimer<=0) {
			device.CloseC();
            vacuumed=true;
		}
		 */	
    //}	

	
    void Reward() {
       // reduce = false;
        device.OpenB();
        Player.GetComponent<PositionTrackingGo>().RewardHappens();
		droptimer -= 1 * Time.deltaTime;
		if (droptimer <= 0) {
			device.CloseB ();
            dropped=true;
		}
			
    }

	void PuffZone() {
		Player.GetComponent<PositionTrackingGo>().PuffZone();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class puff : MonoBehaviour {

    public GameObject Player;
    //public GameObject teleportationTarget;
	//public Text telemetry;
    public float timer;
	public float timerPufftime;
	public float velocity;
	public float puffTime;
	public float phasor;
	public float phasorTimer;
	public float phasorTime;
    bool reduce = false;
    bool reduce2 = false;
    bool phasorReduce = false;
    private GramophoneDevice device;
    bool puffed = false;

    void Start()
    {
        phasor = 1;
        phasorTimer = 4;
        phasorTime = 4f;
        device = GramophoneDevice.Instance();
    }
		
	public void inputPuffValue (string stringTimer)
	{
        timerPufftime = puffTime;
		timer = timerPufftime;
	}
		
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce = true;
            reduce2 = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce = false;
            reduce2 = false;
			timer=timerPufftime;
			AntiPuff();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce = true;
            reduce2 = true;
        }
    }

    void Update()
    {
        phasorTimer -= Time.deltaTime;

        if (phasorTimer <= 0)
        {

//Eszter tól
            if ((Player.GetComponent<PositionTracking>().VelocityIntegral() < 1) ) // & (Player.GetComponent<PositionTracking> ().VelocityIntegral() > 0.5) 
            {
                phasorTimer = 1f;
                phasorTime = 1f;
                phasorTimer -= Time.deltaTime;
            }
            else if (((Player.GetComponent<PositionTracking>().VelocityIntegral() > 1) ) & (Player.GetComponent<PositionTracking>().VelocityIntegral() < 2))
            {
                phasorTimer = 0.4f;
                phasorTime = 0.4f;
                phasorTimer -= Time.deltaTime;
            }
            else if (((Player.GetComponent<PositionTracking>().VelocityIntegral() > 2) ) & (Player.GetComponent<PositionTracking>().VelocityIntegral() <=3))
            {
                phasorTimer = 0.2f;
                phasorTime = 0.2f;
                phasorTimer -= Time.deltaTime;
            }
//Eszter ig


            phasor = -phasor;
		    phasorTimer=phasorTime;
		}

		if (reduce == true)
        {
            timer -= Time.deltaTime;
        }

        if (reduce2 == true)
        {
            PuffZone();
        }
        
        if ((timer <= 0))
        {   //ha 1 akkor épp, hogy csak mozog
         	if ((Player.GetComponent<PositionTracking> ().VelocityIntegral() <=3) & phasor==1)/*||(((Player.GetComponent<PositionTracking>().VelocityIntegral() > 1) & phasor == 1)
                & (Player.GetComponent<PositionTracking>().VelocityIntegral() < 2)) ||  (((Player.GetComponent<PositionTracking>().VelocityIntegral() > 2) & phasor == 1) &
                (Player.GetComponent<PositionTracking>().VelocityIntegral() < 3)))*/
            {
                Puff ();
                puffed = true;

            }
            else if ((Player.GetComponent<PositionTracking>().VelocityIntegral() > 3) && puffed == true)
            {
                Player.GetComponent<PositionTracking>().TeleportAfterPuff();
                puffed = false;
            }
            else
            {
				AntiPuff ();
			}
        }

		//telemetry.text = timer+";";
		//telemetry.color = Color.red;
    }

    void Puff()
    {
        reduce = false;// false volt
        
        /* you can do some control here

        device.OpenA();
        device.CloseA();
        device.ControlServo(2);

        */
        device.OpenA();
		//yield return new WaitForSeconds (0.5f);
		//device.CloseA ();

        Player.GetComponent<PositionTracking>().PuffHappens();

    }
	void AntiPuff()
    {
		device.CloseA ();
        //Player.GetComponent<PositionTracking>().antiPuff = 1;
    }

    void PuffZone()
    {
        Player.GetComponent<PositionTracking>().PuffZone();
    }

    public void PuffReset()
    {
        reduce = false;
        reduce2 = false;
        timer = timerPufftime;
        AntiPuff();
    }
}
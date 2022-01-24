using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class puff_antas : MonoBehaviour {

    public GameObject Player;
    public GameObject teleportationTarget;
	public Text telemetry;
    public float timer;
	public float timerPufftime;
	public float puffTime=1f;
    private GramophoneDevice device;
    bool reduce = false;

    void Start()
    {
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce = false;
			timer=timerPufftime;
			AntiPuff();
        }
    }

    void Update()
    {
        if (reduce == true)
        {
            timer -= 1 * Time.deltaTime;
            PuffZone();
        }

        if (timer <= 0) 
        {
            Puff();
        }

		telemetry.text = timer+";";
		telemetry.color = Color.red;
    }

	void Puff_ogtatas()
    {
		device.OpenA();
		device.CloseA();
	}
    void Puff()
    {
        reduce = false;

         /* you can do some control here

        device.OpenA();
        device.CloseA();
        device.ControlServo(2);

        */

        device.OpenA();
		//device.CloseA ();
        Player.GetComponent<PositionTracking>().PuffHappens();
    }
	void AntiPuff()
    {
		device.CloseA ();
	}

    void PuffZone()
    {
        Player.GetComponent<PositionTracking>().PuffZone();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;


public class PositionTrackingGo : MonoBehaviour {

	private StreamWriter writer;
	private GramophoneDevice device;
	public GameObject Player;
	public float teleportDelta;
	public bool teleporting;
	public bool velocityDepend;
	public bool teleportEvent=false;
	public GameObject teleportationTarget1;
	public GameObject teleportationTarget2;
	public GameObject teleportationTarget3;
	public GameObject teleportationTarget4;
	public GameObject teleportationTarget5;
	public GameObject teleportationTarget6;
	public GameObject teleportationTarget7;
	public GameObject teleportationTarget8;


	[SerializeField]
	private GameObject go;
	public DateTime localDate = DateTime.Now;
	private bool puffHappened = false;
	
	private bool rewardHappened = false;
	
	private bool puffZone = false;
    private bool leftZone = false;
	private bool rightZone = false;
	private bool blackZone = false;
	private bool cloudZone = false;
	
	
	//	private float[] positionbuffer;
	public float teleporttimer=0;
	public int rng=0;
	public int aversive=0;

	private Vector3 startPosition;
	[SerializeField]
	private float distance; //táv
	[SerializeField]
	private float slidingSpeed;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float slowdownPercentage;
	private bool slowDown = false;
	[SerializeField]
	private bool sliding = false;
	
	public float lickpuffer=0;
	public float lickdelta=0;
	public float licklock=0;
	
	public List <float> puffer = new List <float>();
	public List <int> p = new List <int>();
	

	// Use this for initialization
	void Start () {
		rng=UnityEngine.Random.Range (1,7);
		speed = slidingSpeed;
		device = GramophoneDevice.Instance();
		
		if (velocityDepend==true){
		puffer.Add(-1*device.GetVelocity());
		}
		
		string clean=Regex.Replace(localDate+";" , @"[. :;]","");
		writer = new StreamWriter(clean + "training" + ".csv", append: false);
		
		
/* 		string log = GetTimeStamp() + ";" + 
		go.transform.position.z + ";"  + 
		device.GetVelocity () + ";" +  
		puffZoneInt + ";" + 
		leftZoneInt + ";" +
		rightZoneInt + ";" +
		blackZoneInt + ";" +
		cloudZoneInt + ";" + 
		teleportEventInt + ";" +
		puffHappenedInt +";" +
		reward +";" + 
		0  +";" +
		device.GetInputVal()  +";" +
		device.GetInputVal2()  +";" +
		licklockInt +";" +  
		Lick()+ ";" +
		lickdelta  +";" + 
		device.GetSystime() ; */
		
		writer.WriteLine("time;position;velocity;aversive;left;right;black;cloud;teleport;port_A;port_B;port_C;Trigger;Input2;Licklock;Lick;Lickdelta;Systime;");

	}
	
	

	// Update is called once per frame
	void LateUpdate () {
		
		if (teleporting==true){
		TeleportToTarget ();
        } else{
			
			if ((puffZone==true)||(leftZone==true)||(rightZone==true))  {
				TeleportToTarget ();
			}
		}
		
		
		
		if (sliding==true){
			Slide(distance);
		}

	 
	 	  if (Input.GetKey("escape"))
        {
            Application.Quit();
			device.ClosePort();
         
        }
	    WritePositionToCSV(go);
	}
	
		public void LeftZone() {
		leftZone = true;
	}

	public void RightZone() {
		rightZone = true;
	}
	
	public void BlackZone() {
		blackZone = true;
	}

	public void CloudZone() {
		cloudZone = true;
	}

	public void ResetLick() {
		lickpuffer = 0;
	}
	
	public void RewardHappens(){
		rewardHappened = true; 
	}
	
	public float Lick() {
		
	if (licklock != device.GetInputVal2()){
	lickpuffer=lickpuffer+ 1;
	lickdelta=lickdelta+1;
	}
	
	return lickpuffer;
	}
	
	public float LickSimple() {
	return device.GetInputVal2();
	}
	

	public float LickDisp() {
		return lickpuffer;
	}


	public void PuffZone() {
		TeleportToTarget();
		puffZone = true;
	}

	public void PuffHappens() {
		puffZone = true;
		puffHappened = true; 
	}

	public string GetTimeStamp() {
		return "" + Time.time;
	}

	public float VelocityIntegral() {
		float sum=0;
		int pufferlength = puffer.Count;
		int skipped = 0;
		int window = 100;
		if (pufferlength > window)
			pufferlength = window;
		skipped=puffer.Count-window;

		sum = puffer.Skip(skipped).Take (pufferlength).Sum ()/window;
		return sum;
	}

	public void Slide(float distance) {
		if (speed > 0.0001f) {
			if ((Vector3.Distance(startPosition, Player.transform.position)) > distance) {
				speed *= (100 - slowdownPercentage) / 100;
	
			}

			Player.transform.Translate(new Vector3(0, 0, speed));

		}
		else {

			speed = 0;
			sliding = false;
		}
	}
	
	public void TeleportToDefined(){
			if (velocityDepend==true){
		if ((VelocityIntegral () < 1))
		{
			teleporttimer += 1 * Time.deltaTime;
		} else {
			teleporttimer = 0;

		}
		
		} else {
			teleporttimer += 1 * Time.deltaTime;
		}

		rng=1;

		if (teleporttimer >= teleportDelta) {


		
			switch (rng) {

			case 1:
				Player.transform.position = teleportationTarget1.transform.position;
		
				break;

			case 2:
				Player.transform.position = teleportationTarget2.transform.position;

				break;

			case 3:
				Player.transform.position = teleportationTarget3.transform.position;

				break;

			case 4:
				Player.transform.position = teleportationTarget4.transform.position;
			
				break;

			case 5:
				Player.transform.position = teleportationTarget5.transform.position;

				break;

			case 6:
				Player.transform.position = teleportationTarget6.transform.position;

				break;
				
				case 7:
				Player.transform.position = teleportationTarget7.transform.position;

				break;
				
				case 8:
				Player.transform.position = teleportationTarget8.transform.position;

				break;
			}
			teleportEvent = true;
			sliding = true;
			startPosition = Player.transform.position;
			teleporttimer = 0;
			speed = slidingSpeed;
		} 
	}

	public void TeleportToTarget(){
		
		if (velocityDepend==true){
		if ((VelocityIntegral () < 1))
		{
			teleporttimer += 1 * Time.deltaTime;
		} else {
			teleporttimer = 0;

		}
		
		} else {
			teleporttimer += 1 * Time.deltaTime;
		}

		rng=UnityEngine.Random.Range (1,9);

		if (teleporttimer >= teleportDelta) {


		
			switch (rng) {

			case 1:
				Player.transform.position = teleportationTarget1.transform.position;
		
				break;

			case 2:
				Player.transform.position = teleportationTarget2.transform.position;

				break;

			case 3:
				Player.transform.position = teleportationTarget3.transform.position;

				break;

			case 4:
				Player.transform.position = teleportationTarget4.transform.position;
			
				break;

			case 5:
				Player.transform.position = teleportationTarget5.transform.position;

				break;

			case 6:
				Player.transform.position = teleportationTarget6.transform.position;

				break;
				
				case 7:
				Player.transform.position = teleportationTarget7.transform.position;

				break;
				
				case 8:
				Player.transform.position = teleportationTarget8.transform.position;

				break;
			}
			teleportEvent = true;
			sliding = true;
			startPosition = Player.transform.position;
			teleporttimer = 0;
			speed = slidingSpeed;
		} 
	
	}

	public void WritePositionToCSV(GameObject go) {
		
        if (velocityDepend== true) {
		puffer.Add (Mathf.Abs (device.GetVelocity ()));
		}
	
		int puffZoneInt = Convert.ToInt16 (puffZone);
		int leftZoneInt = Convert.ToInt16 (leftZone);
		int rightZoneInt = Convert.ToInt16 (rightZone);
		int blackZoneInt = Convert.ToInt16 (blackZone);
		int cloudZoneInt = Convert.ToInt16 (cloudZone);
		
		int puffHappenedInt = Convert.ToInt16 (puffHappened);
		int teleportEventInt = Convert.ToInt16 (teleportEvent);
		
		int rewardHappenedInt = Convert.ToInt16 (rewardHappened);
		int licklockInt = Convert.ToInt16 (licklock);
        
		
		string log = GetTimeStamp() + ";" + 
		go.transform.position.z + ";"  + 
		device.GetVelocity () + ";" +  
		puffZoneInt + ";" + 
		leftZoneInt + ";" +
		rightZoneInt + ";" +
		blackZoneInt + ";" +
		cloudZoneInt + ";" + 
		teleportEventInt + ";" +
		puffHappenedInt +";" +
		rewardHappenedInt +";" + 
		"0"  +";" +
		device.GetInputVal()  +";" +
		device.GetInputVal2()  +";" +
		licklockInt +";" +  
		Lick()+ ";" +
		lickdelta  +";" + 
		device.GetSystime() ;
		
		
		
	 //	string log = device.GetInputVal() + ";" + device.GetInputVal2() + ";" + puffZoneInt + ";" + leftZoneInt;
		licklock=device.GetInputVal2();
		teleportEvent = false; 
		Debug.Log(log);


		writer.WriteLine(log);
		if (puffHappened)
			puffHappened = false;
		
		if (puffZone)
			puffZone = false;
		
		if (leftZone)
			leftZone = false;

		if (rightZone)
			rightZone = false;
	
		if (blackZone)
		blackZone = false;
		
		if (cloudZone)
		cloudZone = false;
	
			if (rewardHappened)
			rewardHappened = false;
	
	}
}

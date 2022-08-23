using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class PositionTracking : MonoBehaviour
{

	//jatekos
	public GameObject Player;

	//kiiro cucc
	private StreamWriter writer;

	//gramofon
	private GramophoneDevice device;

	//teleport stuff
	public float teleportDelta; //ennyi idot var ha all az eger, mielott teleportal
	public float teleportTimer = 0; //szamol teleportDelta-ig, majd teleport tortenik
	public bool teleporting; //checkbox, ha pipa, akkor van teleport
	public bool velocityDepend; //sebessegfuggo teleport
	public GameObject teleportationTarget1; //1. teleporthely
	public float probability1;
	public GameObject teleportationTarget2; //2. teleporthely
	public float probability2;
	public GameObject teleportationTarget3; //3. teleporthely
	public float probability3;
	public GameObject teleportationTarget4; //4. teleporthely
	public float probability4;
	public GameObject teleportationTarget5; //5. teleporthely
	public float probability5;
	public GameObject teleportationTarget6; //6. teleporthely
	public float probability6;
	public GameObject teleportationTarget7; //7. teleporthely
	public float probability7;
	public GameObject teleportationTarget8; //8. teleporthely
	public float probability8;
	public int rng = 0; //1-9 kozott vesz fel random integert
	[HideInInspector] public bool teleportEvent = false; //logolasnal 1-ha teleport van eppen es 0 egyebkent
	public float lickPuffer = 0; // a zonaban hanyszor nyalogatott, amig bent volt
	public float lickDelta = 0; //osszesen hanyszor nyalogatott
	public float lickLock = 0; //0, ha nincs nem nyalogat, 1 ha igen
	public float lickTime = 0; //nyalogatás óta eltelt idõ
	public float teleportAfterLick = 0; //ennyi idõ után teleportál nyalogatás után
	public float moveTime = 0; //futás óta eltelt idõ
	public float teleportAfterNoMove = 0; //ennyi idõ után teleportál futás után
	private bool puffZone = false; //0, ha nincs puff zonaban, 1 ha igen
	private bool leftZone = false; //0, ha nincs bal zonaban, 1 ha igen
	public GameObject RewardZone;
	private bool rightZone = false; //0, ha nincs jobb zonaban, 1 ha igen
	private bool blackZone = false; //0, ha nincs fekete zonaban, 1 ha igen
	private bool cloudZone = false; //0, ha nincs felho zonaban, 1 ha igen
	private bool mismatchZone = false; //0, ha nincs mismatch zonaban, 1 ha igen
	[HideInInspector] public bool puffHappened = false;  //0, ha nem kap puffot, 1 ha igen
	[HideInInspector] public bool rewardHappened = false;  //0, ha nem kap jutalmat, 1 ha igen
	public DateTime localDate = DateTime.Now;
	private Vector3 startPosition;
	public bool sliding = false; //0, ha eppen nem csuszik, 1, ha igen
	[SerializeField] private float slideTime; //csuszasi tavolsag
	private float slideTimer;
	[SerializeField] private float slidingSpeed; //csuszasi sebesseg
	private float speed; //karakter sebessege
	[HideInInspector] public List<float> puffer = new List<float>();
	private Scene scene;
	bool rewardScene;
	bool punishmentScene;
	bool mismatchScene;

	void Start()
	{
        rng = UnityEngine.Random.Range(1, 9);
        speed = slidingSpeed;
        slideTimer = 0;
        device = GramophoneDevice.Instance();
        //if (velocityDepend == true)
        //{
        puffer.Add(device.GetVelocity());
        //}
        string clean = Regex.Replace(localDate + ";", @"[. :;]", "");
        writer = new StreamWriter(clean + "training" + ".csv", append: false);
        writer.WriteLine("time;position;velocity;aversive;left;right;black;cloud;mismatch;teleport;port_A;port_B;port_C;Trigger;Input2;lickLock;Lick;lickDelta;Systime;");
        probability8 = 100 - (probability1 + probability2 + probability3 + probability4 + probability5 + probability6 + probability7);
        scene = SceneManager.GetActiveScene();
	}

	void LateUpdate()
	{
		if (teleporting == true)
		{
			TeleportToTarget();
		}
		else
		{
			if ((puffZone == true) || (leftZone == true) || (rightZone == true) || (cloudZone == true) || (mismatchZone == true))
			{
				TeleportToTarget();
			}
		}

		if (sliding == true)
		{
			Slide();
		}

		if (Input.GetKey("escape"))
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
            device.ClosePort();
        }

		WritePositionToCSV(Player);
	}

	public void LeftZone()
	{
		leftZone = true;
	}

	public void RightZone()
	{
		rightZone = true;
	}

	public void BlackZone()
	{
		blackZone = true;
	}

	public void CloudZone()
	{
		cloudZone = true;
	}
	public void MismatchZone()
	{
		mismatchZone = true;
	}

	public void ResetLick()
	{
		lickPuffer = 0;
	}

	public void RewardHappens()
	{
		rewardHappened = true;
	}

	public float Lick()
	{
		if (lickLock < device.GetInputVal2())
		{
			lickPuffer++;
			lickDelta++;
			if (scene.name == "Reward")
			{
				lickTime = 0;
			}
		}
		return lickPuffer;
	}

	public float LickSimple()
	{
		return device.GetInputVal2();
	}

	public float LickDisp()
	{
		return lickPuffer;
	}

	public void PuffZone()
	{
		//TeleportToTarget();
		puffZone = true;
	}

	public void PuffHappens()
	{
		puffHappened = true;
	}

	public string GetTimeStamp()
	{
		return "" + Time.time;
	}

	public float VelocityIntegral() //megnezi, hogy az utolso 100 sebessegerteknek mi az atlaga
	{
		float sum = 0;
		int pufferlength = puffer.Count;
		int skipped = 0;
		int window = 100;
		if (pufferlength > window)
		{
			pufferlength = window;
		}
		skipped = puffer.Count - window;
		sum = puffer.Skip(skipped).Take(pufferlength).Sum() / window;
		return sum;
	}

	//public void Slide(float slideDistance) //slideot ad az egernek
	//{
	//	if (speed > 0.0001f)
	//	{
	//		if ((Vector3.Distance(startPosition, Player.transform.position)) > slideDistance)
	//		{
	//			speed *= (100 - slowdownPercentage) / 100;
	//		}
	//		Player.transform.Translate(new Vector3(0, 0, speed));
	//	}
	//	else
	//	{
	//		speed = 0;
	//		sliding = false;
	//	}
	//}



	public void Slide() //slideot ad az egernek
	{
		if (slideTimer < slideTime)
		{
			speed = slidingSpeed - (slidingSpeed * slideTimer / slideTime);
			Player.transform.Translate(new Vector3(0, 0, speed));
			slideTimer += Time.deltaTime;
		}
		else
		{
			speed = 0;
			sliding = false;
			slideTimer = 0;
		}

	}


	int GetRandomValue()
	{
		float rand = UnityEngine.Random.value;
		if (rand <= probability1 / 100)
		{
			return 1;
		}
		if (rand <= (probability1 + probability2) / 100)
		{
			return 2;
		}
		if (rand <= (probability1 + probability2 + probability3) / 100)
		{
			return 3;
		}
		if (rand <= (probability1 + probability2 + probability3 + probability4) / 100)
		{
			return 4;
		}
		if (rand <= (probability1 + probability2 + probability3 + probability4 + probability5) / 100)
		{
			return 5;
		}
		if (rand <= (probability1 + probability2 + probability3 + probability4 + probability5 + probability6) / 100)
		{
			return 6;
		}
		if (rand <= (probability1 + probability2 + probability3 + probability4 + probability5 + probability6 + probability7) / 100)
		{
			return 7;
		}

		return 8;
	}



	//public void TeleportToDefined()
	//{
	//	if (velocityDepend == true)
	//	{
	//		if (VelocityIntegral() < 1)
	//		{
	//			teleportTimer += Time.deltaTime;
	//		}
	//		else
	//		{
	//			teleportTimer = 0;
	//		}

	//	}
	//	else
	//	{
	//		teleportTimer += Time.deltaTime;
	//	}

	//	if (teleportTimer >= teleportDelta)
	//	{
	//		Player.transform.position = teleportationTarget1.transform.position;
	//		teleportEvent = true;
	//		sliding = true;
	//		startPosition = Player.transform.position;
	//		teleportTimer = 0;
	//		speed = slidingSpeed;
	//	}
	//}

	public void TeleportToTarget()
	{
		if (velocityDepend == true)
		{
			if ((VelocityIntegral() < 1))
			{
				teleportTimer += Time.deltaTime;
			}
			else
			{
				teleportTimer = 0;
			}

		}
		else
		{
			teleportTimer += Time.deltaTime;

			if (scene.name == "Punishment")
			{
				if (VelocityIntegral() > 10)
				{
					moveTime = 0;
				}
				else
				{
					moveTime += Time.deltaTime;
				}

			}
            if (scene.name == "Reward")
            {
                lickTime += Time.deltaTime;
            }

        }

		rng = GetRandomValue();
		if (scene.name == "Reward" && lickTime >= teleportAfterLick) rewardScene = true;
		if (scene.name == "Punishment" && moveTime >= teleportAfterNoMove) punishmentScene = true;
		if (teleportTimer >= teleportDelta && (rewardScene || punishmentScene))
		{
			switch (rng)
			{
				case 1:
					Player.transform.position = teleportationTarget1.transform.position;
					break;

				case 2:
					Player.transform.position = teleportationTarget2.transform.position;
					if (scene.name == "Punishment") RewardZone.GetComponent<waterRewardOblique>().RewardReset();
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
			teleportTimer = 0;
			speed = slidingSpeed;
		}

	}

	public void WritePositionToCSV(GameObject Player)
	{
		//if (velocityDepend == true)
		//{
			puffer.Add(Mathf.Abs(device.GetVelocity()));
		//}
		int puffZoneInt = Convert.ToInt16(puffZone);
		int leftZoneInt = Convert.ToInt16(leftZone);
		int rightZoneInt = Convert.ToInt16(rightZone);
		int blackZoneInt = Convert.ToInt16(blackZone);
		int cloudZoneInt = Convert.ToInt16(cloudZone);
		int mismatchZoneInt = Convert.ToInt16(mismatchZone);
		int puffHappenedInt = Convert.ToInt16(puffHappened);
		int teleportEventInt = Convert.ToInt16(teleportEvent);
		int rewardHappenedInt = Convert.ToInt16(rewardHappened);
		int lickLockInt = Convert.ToInt16(lickLock);
		double playerPosition = Convert.ToDouble(Player.transform.localPosition.z);

		//log
		string log = GetTimeStamp() + ";" +
		playerPosition + ";" +
		device.GetVelocity() + ";" +
		puffZoneInt + ";" +
		leftZoneInt + ";" +
		rightZoneInt + ";" +
		blackZoneInt + ";" +
		cloudZoneInt + ";" +
		mismatchZoneInt + ";" +
		teleportEventInt + ";" +
		puffHappenedInt + ";" +
		rewardHappenedInt + ";" +
		0 + ";" +
		device.GetInputVal() + ";" +
		device.GetInputVal2() + ";" +
		lickLockInt + ";" +
		Lick() + ";" +
		lickDelta + ";" +
		device.GetSystime();

		lickLock = device.GetInputVal2();
		teleportEvent = false;
        Debug.Log(log);
        writer.WriteLine(log);
		puffHappened = false;
		puffZone = false;
		leftZone = false;
		rightZone = false;
		blackZone = false;
		cloudZone = false;
		mismatchZone = false;
		rewardHappened = false;
	}
}
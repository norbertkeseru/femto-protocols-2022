using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class left : MonoBehaviour {

	public GameObject Player;
	public GameObject teleportationTarget;
	public Text telemetry;
	private GramophoneDevice device;
	bool reduce = false;

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
			reduce = false;
			Player.GetComponent<PositionTracking>().ResetLick();
		}
	}

	void Update()
	{
		if (reduce == true)
		{
	
			LeftZone();
		}
	}

	void LeftZone()
	{
		Player.GetComponent<PositionTracking>().LeftZone();
	}
}
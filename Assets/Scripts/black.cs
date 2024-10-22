using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class black : MonoBehaviour {

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
		}
	}

	void Update()
	{
		if (reduce == true)
		{
			BlackZone();
		}
	}

	void BlackZone()
	{
		Player.GetComponent<PositionTracking>().BlackZone();
	}
}
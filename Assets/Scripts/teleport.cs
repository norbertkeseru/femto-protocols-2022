using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
	public GameObject Player;
	public GameObject teleportationTarget;
	private GameObject PositionTracking;

	bool reduce = false;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter_state:");

		if (other.tag == "Player")
		{
			reduce = true;

			Debug.Log("TELEPORT");

			//teleport
			PositionTracking.GetComponent<PositionTracking>().TeleportToDefined();
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("exit_state:");

		if (other.tag == "Player")
		{
			reduce = false;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
	public GameObject Player;
	public GameObject teleportationTarget;

	bool reduce = false;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter_state:");

		if (other.tag == "Player")
		{
			reduce = true;

			Debug.Log("TELEPORT");

			//teleport
			//PositionTracking.GetComponent<PositionTracking>().TeleportToDefined();
			Player.transform.position = teleportationTarget.transform.position;
			
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("exit_state:");

		if (other.tag == "Player")
		{
			reduce = false;
			if(SceneManager.GetActiveScene().name != "Mismatch and reward")
            {
				Player.GetComponent<PositionTracking>().sliding = true;
			}
		}
	}
}
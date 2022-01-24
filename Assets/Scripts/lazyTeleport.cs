using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lazyTeleport : MonoBehaviour {
    
		public GameObject Player;
		public GameObject teleportationTarget;
	    public Text telemetry;

		void Update()
		{
			if (Input.GetKeyDown("space"))
			{
				//teleport
				//Player.transform.position = teleportationTarget.transform.position;
				telemetry.text = "Telepoooot";
				telemetry.color = Color.white;
				Player.GetComponent<Movements> ();
			}
		}
	}
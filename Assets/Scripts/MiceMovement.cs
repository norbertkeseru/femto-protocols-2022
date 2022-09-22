using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceMovement : MonoBehaviour {

	public bool moving; //bool, if true, then the character can be moved by Gramophone
    private GramophoneDevice device;
	Rigidbody m_Rigidbody;

	void Start ()
	{
        device = GramophoneDevice.Instance();
        m_Rigidbody = GetComponent<Rigidbody>();
	}

	void Update ()
	{
        if (moving==true)
		{
			m_Rigidbody.velocity = transform.forward * device.GetVelocity();
		}
		else
		{
			m_Rigidbody.velocity = transform.forward * 0;
		}

	}
	
	public void Stop()
	{
		moving=false;
	}
	
	public void Restart()
	{
		moving=true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mismatch : MonoBehaviour
{

	public GameObject Player;
	public GameObject teleportationTarget;
	private GramophoneDevice device;
	public Text telemetry;
	bool reduce = false;
	[SerializeField] private float mismatchAfterMin;
	[SerializeField] private float mismatchAfterMax;
	[SerializeField] private float mismatchAfterMoving;
	private float movingTime;
	[SerializeField] public float eventTime;
	[SerializeField] public float rewardTime;
	[SerializeField] public float dropTimer;
	private bool eventCounter;

	void Start()
	{
		device = GramophoneDevice.Instance();
		eventCounter = false;
		mismatchAfterMoving = Random.Range(mismatchAfterMin, mismatchAfterMax);
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
			MismatchZone();

			if (Player.GetComponent<PositionTracking>().VelocityIntegral() > 5)
            {
				movingTime += Time.deltaTime;
            }
			else
            {
				movingTime = 0;
            }

			if (movingTime >= mismatchAfterMoving && eventCounter == false)
            {
				Mismatch();
				eventCounter = true;
            }
		}
        else
        {
			eventCounter = false;
        }
	}

	void MismatchZone()
	{
		Player.GetComponent<PositionTracking>().MismatchZone();
	}

	public void Mismatch()
    {
		StartCoroutine(MismatchEvent());
	}

	IEnumerator MismatchEvent()
    {
		Player.GetComponent<MiceMovement>().Stop();
		yield return new WaitForSecondsRealtime(eventTime);
		Player.GetComponent<MiceMovement>().Restart();
		yield return new WaitForSecondsRealtime(rewardTime);
        device.OpenB();
        yield return new WaitForSecondsRealtime(dropTimer);
		device.CloseB();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	[SerializeField] public float endTrial;
	[SerializeField] public int trialNumber;
	private bool eventCounter;

	void Start()
	{
		device = GramophoneDevice.Instance();
		eventCounter = false;
		mismatchAfterMoving = Random.Range(mismatchAfterMin, mismatchAfterMax);
		if (!PlayerPrefs.HasKey("trialNumber"))
        {
			PlayerPrefs.SetInt("trialNumber", 0);
		}
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

			if (Player.GetComponent<PositionTracking>().VelocityIntegral() > 2)
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
	public void IncreaseTrialNumber()
    {
		PlayerPrefs.SetInt("trialNumber", PlayerPrefs.GetInt("trialNumber") + 1);
    }
	IEnumerator MismatchEvent()
    {
		Player.GetComponent<MiceMovement>().Stop();
		yield return new WaitForSecondsRealtime(eventTime);
		Player.GetComponent<MiceMovement>().Restart();
		yield return new WaitForSecondsRealtime(rewardTime);
        device.OpenB();
		Player.GetComponent<PositionTracking>().RewardHappens();
		yield return new WaitForSecondsRealtime(dropTimer);
		device.CloseB();
		yield return new WaitForSecondsRealtime(endTrial);
		IncreaseTrialNumber();
		if (PlayerPrefs.GetInt("trialNumber") == trialNumber)
        {
			Application.Quit();
			PlayerPrefs.DeleteAll();
		}
		SceneManager.LoadScene("Mismatch and reward");
	}
}
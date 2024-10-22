using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mismatch : MonoBehaviour
{

	public GameObject Player;
	public GameObject teleportationTarget;
    public GameObject startLocation;
    [SerializeField]private GramophoneDevice device;
	public Text telemetry;
	bool reduce = false;
	[SerializeField] private float mismatchAfterMin;
	[SerializeField] private float mismatchAfterMax;
	[SerializeField] private float mismatchAfterMoving;
    [SerializeField] private float movingTime;
	[SerializeField] public float eventTime;
	[SerializeField] public float rewardTime;
	[SerializeField] public float dropTimer;
	[SerializeField] public float endTrial;
    [SerializeField] public int trialSum;
    private int trialNum;
    private bool eventCounter;
    private Coroutine coroutine;

    void Start()
    {
        eventCounter = false;
        mismatchAfterMoving = Random.Range(mismatchAfterMin, mismatchAfterMax);
        //if (!PlayerPrefs.HasKey("trialNumber"))
        //{
        //  PlayerPrefs.SetInt("trialNumber", 0);
        trialNum = 0;
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

			if (Player.GetComponent<PositionTracking>().VelocityIntegral() > 3)
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
            movingTime = 0;
            StopCoroutine(coroutine);
        }
	}

	void MismatchZone()
	{
		Player.GetComponent<PositionTracking>().MismatchZone();
	}

	public void Mismatch()

    {
		coroutine = StartCoroutine(MismatchEvent());
	}
    //public void IncreaseTrialNumber()
    //{
    //	PlayerPrefs.SetInt("trialNumber", PlayerPrefs.GetInt("trialNumber") + 1);
    //}
    IEnumerator MismatchEvent()
    {
        trialNum++;
        Player.GetComponent<MiceMovement>().Stop();
        yield return new WaitForSecondsRealtime(eventTime);
        Player.GetComponent<MiceMovement>().Restart();
        yield return new WaitForSecondsRealtime(rewardTime);
        device.OpenB();
        Player.GetComponent<PositionTracking>().RewardHappens();
        yield return new WaitForSecondsRealtime(dropTimer);
        device.CloseB();
        yield return new WaitForSecondsRealtime(endTrial);
        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, startLocation.transform.position.z + 6.5f);
        mismatchAfterMoving = Random.Range(mismatchAfterMin, mismatchAfterMax);
        //IncreaseTrialNumber();
        if (trialNum == trialSum)
        {
            Application.Quit();
            PlayerPrefs.DeleteAll();
        }
        //SceneManager.LoadScene("Mismatch and reward");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterRewardTraining : MonoBehaviour
{
    public GameObject Player;
    private GramophoneDevice device;
    public float droptimer;
    public float tapOpentime;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            reduce = false;
            droptimer = tapOpentime;
        }
    }

    void Update()
    {
        if (reduce == true)
        {
            device.OpenB();
            droptimer -= Time.deltaTime;
            if(droptimer <= 0)
            {
                device.CloseB();
            }

        }
    }
}

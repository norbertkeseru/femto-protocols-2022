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
    bool dropped = false;

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
            dropped = false;
            droptimer = tapOpentime;
        }
    }

    void Update()
    {
        if (reduce == true && dropped == false)
        {
            Reward();
        }
        
    }

    void Reward()
    {
        device.OpenB();
        droptimer -= Time.deltaTime;
        if (droptimer <= 0)
        {
            device.CloseB();
            dropped = true;
        }
    }
}

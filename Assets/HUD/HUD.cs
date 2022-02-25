using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private PositionTracking PT;
    private GramophoneDevice GD;
    public Image Puff;
    public Image Puff_icon;
    public Image Lick;
    public Image Lick_icon;
    public Image Drop;
    public Image Drop_icon;
    public Image Recording;
    public Image Recording_circle;
    Color32 trueColor = new Color32(0, 255, 0, 100);
    [HideInInspector] public bool puff;
    [HideInInspector] public bool lick;
    [HideInInspector] public bool drop;
    [HideInInspector] public bool recording;

    void Start()
    {
        Puff.color = trueColor;
        Lick.color = trueColor;
        Drop.color = trueColor;
    }

    void Update()
    {
        //puff = PT.puffHappened;
        //drop = PT.rewardHappened;
        if (GD.GetInputVal2() != 0)
        {
            Lick.enabled = true;
            Lick_icon.enabled = true;
        }
        else
        {
            Lick.enabled = false;
            Lick_icon.enabled = false;
        }

        if (GD.OpenedA)
        {
            Puff.enabled = true;
            Puff_icon.enabled = true;
        }
        else
        {
            Puff.enabled = false;
            Puff_icon.enabled = false;
        }

        if(GD.OpenedB)
        {
            Drop.enabled = true;
            Drop_icon.enabled = true;
        }
        else
        {
            Drop.enabled = false;
            Drop_icon.enabled = false;
        }
        
        if(GD.GetInputVal() != 0)
        {
            Recording.enabled = true;
            Recording_circle.enabled = true;
        }
        else
        {
            Recording.enabled = false;
            Recording_circle.enabled = false;
        }

        //if (puff == true)
        //{
        //    Puff.enabled = true;
        //    Puff.color = trueColor;
        //}
        //else
        //{
        //    Puff.enabled = false;
        //}

        //if (drop == true)
        //{
        //    Drop.color = trueColor;
        //}
        //else
        //{
        //    Drop.color = defaultColor;
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//puff 79 line!!!!!!!!!!

public class feedback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(float [] MovementVector)
    {
        //Determines the movement value before the airpuff arrival.
        //If the value is bigger, than:............... - value=0; - airpuff s not required in the next trial at the critical zone.
        //If ................< value <................ - value=0.5; - half time of the airpuff is needed in the next trial
        //if the value is less, than: ................ - value=1; - airpuff arrives in the next trial

        //examine the previous period (0,0167 s 100as átlagra a lépésköz)

        //device.GetVelocity 

        //if (Player.GetComponent<PositionTracking>().VelocityIntegral() > 2)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

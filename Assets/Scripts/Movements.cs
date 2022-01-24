using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour {

    private Vector3 startPosition;
	public GameObject Player;
    [SerializeField] private Transform startFrom;
    [SerializeField] private float distance; //táv
    [SerializeField] private float speed;
    [SerializeField] private float slowdownPercentage;
    [SerializeField] private GameObject distanceMarker;
    private bool slowDown = false;

	void Start ()
    {
        this.transform.position = startFrom.position;
        startPosition = this.transform.position;
	}
	
	void Update ()
    {
        if (speed != 0)
        {
            Go(distance);
        }
    }

    public void Go(float distance)
    {
        if (speed > 0.0001f)
        {
            if ((Vector3.Distance(startPosition, this.transform.position)) > distance)
            {
                speed *= (100 - slowdownPercentage) / 100;
            }
            this.transform.Translate(new Vector3(0, 0, speed));
        }
        else
        {
            speed = 0;
        }
    }
}

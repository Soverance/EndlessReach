// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessMissile : MonoBehaviour {

    private float MissleSpeed = 200;  // reference for speed
    private bool CanMove;
    //private Transform MissleTransform;  // reference for transform, performance optimization technique

	// Use this for initialization
	void Start () {
        //MissleTransform = transform;
        CanMove = true;
	}

	// Update is called once per frame
	void Update () {
        // calculate speed
        float MissileMoveDistance = MissleSpeed * Time.deltaTime;
        // fire the missile
        if (CanMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, VR_CamRaycast._LookTarget.transform.position, MissileMoveDistance);
        }        
	}
}

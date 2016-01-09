// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessExplosion : MonoBehaviour {

    private float StartTimer;
    private float ExplosionLife = 2f;

	// Use this for initialization
	void Start () 
    {
        StartTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (StartTimer + ExplosionLife < Time.time)
        {
            Destroy(this.gameObject);
        }
	}
}

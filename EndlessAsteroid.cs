// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessAsteroid : MonoBehaviour {

    private float StartTimer = 0f;
    private float AsteroidLife = 20f;
    public float AsteroidSpeedMin;
    public float AsteroidSpeedMax;
    private float AsteroidSpeed;
    
    //private float x = Random.Range(-1f, 1f);
    private float y = Random.Range(0, 2f);
    
    private Vector3 RotationDirection;

    void Start()
    {
        RotationDirection = new Vector3(0, y, 0);
        AsteroidSpeed = Random.Range(AsteroidSpeedMin, AsteroidSpeedMax);
        StartTimer = Time.time;
    }
    
	// Update is called once per frame
	void Update () 
    {
        this.transform.Rotate(RotationDirection);  // rotate
        float MoveDistance = AsteroidSpeed * Time.deltaTime;  // get speed
        transform.Translate(Vector3.down * MoveDistance);  // translate downward every frame

        // destroy if over time limit
        if (StartTimer + AsteroidLife < Time.time)
        {
            Destroy(this.gameObject);
        }
	}
}

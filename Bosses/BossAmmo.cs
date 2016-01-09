// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class BossAmmo : MonoBehaviour {

    private float ProjectileSpeed = 225;  // reference for speed
    private Transform ProjectileTransform;  // reference for transform, performance optimization technique
    private float _ProjectileLife = 3f;


    // Use this for initialization
    void Start()
    {
        // Gather transform information
        ProjectileTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate laser speed
        float ProjectileMoveDistance = ProjectileSpeed * Time.deltaTime;
        // fire the laser
        ProjectileTransform.Translate(Vector3.down * ProjectileMoveDistance);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); // new position locks on X and Z
	}
}

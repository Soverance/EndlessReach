// Endless Reach
// Soverance Studios
// www.soverance.com
// The EndlessProjectile class handles player projectile events.

using UnityEngine;
using System.Collections;

public class EndlessProjectile : MonoBehaviour {

    private float ProjectileSpeed = 225;  // reference for speed
    private Transform ProjectileTransform;  // reference for transform, performance optimization technique
    private float _ProjectileLife = 3f;


	// Use this for initialization
	void Start () 
    {
        // Gather transform information
        ProjectileTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        // calculate laser speed
        float ProjectileMoveDistance = ProjectileSpeed * Time.deltaTime;
        // fire the laser
        ProjectileTransform.Translate(Vector3.up * ProjectileMoveDistance);

        if (this.transform.position.y > (EndlessGameInfo.TopBlockPosition + 500))
        {
            Destroy(this.gameObject);
        }
   	}

    // function called when projectile collides with an object
    void OnTriggerEnter(Collider OtherObject)
    {
        Destroy(this.gameObject);
    }
}

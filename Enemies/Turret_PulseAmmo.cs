// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Turret_PulseAmmo : MonoBehaviour {

    public GameObject LightningBall;
    public GameObject ArcaneExplosion;
    //private GameObject PlayerDrone;
    //private float CurrentSpeedDown;
    //private float CurrentSpeedX;
    //private float MinSpeed = 2;  // min speed
    //private float MaxSpeed = 5;  // max speed

	// Use this for initialization
	void Start () 
    {
        //CurrentSpeedDown = Random.Range(MinSpeed, MaxSpeed);
        //PlayerDrone = GameObject.Find("VR_Player2D");
        StartCoroutine(LightningPop());
	}
	
    private IEnumerator LightningPop()
    {
        yield return new WaitForSeconds(3f);
        ArcaneExplosion.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
	}
}

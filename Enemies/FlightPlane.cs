// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class FlightPlane : MonoBehaviour {

    public GameObject PlateEffect;

	// Use this for initialization
	void Start () {
	    
	}

    //void OnTriggerEnter(Collider OtherObject)
    //{
    //    if (OtherObject.gameObject.layer == 10)
    //    {
    //        Debug.Log("COLLISION ON PLANE");
    //        PlateEffect.SetActive(true);
    //        PlateEffect.transform.position = new Vector3(OtherObject.transform.position.x, OtherObject.transform.position.y, 0);
    //    }        
    //}

    //void OnTriggerExit(Collider OtherObject)
    //{
    //    if (OtherObject.gameObject.layer == 10)
    //    {
    //        PlateEffect.SetActive(false);
    //    }
    //}
	
	// Update is called once per frame
	void Update () {
	
	}
}

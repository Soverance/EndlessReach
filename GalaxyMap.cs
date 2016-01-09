// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class GalaxyMap : MonoBehaviour {

    public static bool _InRange;
    public static bool _Load;

	// Use this for initialization
	void Start () {
        _InRange = false;
        _Load = false;
	}

    void OnTriggerEnter(Collider Other)
    {
        if (this.gameObject.tag == "GalaxyMap_Object")
        {
            _InRange = true;
        }
        if (this.gameObject.tag == "StagingArea")
        {
            _Load = true;
        }
        
    }

    void OnTriggerExit(Collider Other)
    {
        _InRange = false;
        _Load = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

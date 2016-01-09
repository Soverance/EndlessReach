// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Turret_MissileTarget : MonoBehaviour {

    private TurretInstallation MainTurret;

	// Use this for initialization
	void Start () {
        MainTurret = this.transform.parent.transform.parent.gameObject.GetComponent<TurretInstallation>();
	}

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "missile")
        {
            //Debug.Log("TARGET HIT");
            EndlessPlayerController._MissileActive = false;
            MainTurret.MissileConnect();
            Destroy(Other.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

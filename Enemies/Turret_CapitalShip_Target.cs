// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Turret_CapitalShip_Target : MonoBehaviour {

    private TurretInstallation MainTurret;
    public GameObject CapitalShipExplosion;

    // Use this for initialization
    void Start()
    {
        MainTurret = this.transform.parent.gameObject.GetComponent<TurretInstallation>();
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "missile")
        {
            //Debug.Log("TARGET HIT");
            Instantiate(CapitalShipExplosion, this.transform.position, Quaternion.identity);
            EndlessPlayerController._MissileActive = false;
            Destroy(Other.gameObject);
            MainTurret.MissileConnect();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

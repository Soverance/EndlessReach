// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class CentipedeSegment : MonoBehaviour {

    public GameObject CentipedeExplosion;

	void Awake () 
    {

	}

    private void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.tag == "Player_Laser")
        {
            gameObject.transform.parent.gameObject.GetComponent<EndlessCentipede>().SegmentCount--;
            gameObject.transform.parent.gameObject.GetComponent<EndlessCentipede>().AddScore();
            CentipedeExplosion = Instantiate(CentipedeExplosion, this.transform.position, Quaternion.identity) as GameObject;            
            Destroy(gameObject);
        }

        if (OtherObject.tag == "Purge")
        {
            gameObject.transform.parent.gameObject.GetComponent<EndlessCentipede>()._IsPurged = true;
            gameObject.transform.parent.gameObject.GetComponent<EndlessCentipede>().AddScore();
            CentipedeExplosion = Instantiate(CentipedeExplosion, this.transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {

	}
}

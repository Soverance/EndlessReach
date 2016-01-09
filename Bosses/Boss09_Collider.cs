// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Boss09_Collider : MonoBehaviour {

    private EndlessBoss09 Boss09;

    void Start()
    {
        Boss09 = gameObject.transform.parent.GetComponent<EndlessBoss09>();
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.tag == "Player_Laser")
        {
            Boss09.health = (Boss09.health - 100);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)Boss09.health / Boss09.maxHP);  // update health bar with HP percentage
        }
        if (Other.tag == "Purge" && !Boss09._WasPurged)
        {
            Boss09._WasPurged = true;
            Boss09.health = (Boss09.health - 12500);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)Boss09.health / Boss09.maxHP);  // update health bar with HP percentage
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Boss09.health <= 0 && Boss09._AddingScore == false)
        {
            StartCoroutine(Boss09.AddScore());
        }	
	}
}

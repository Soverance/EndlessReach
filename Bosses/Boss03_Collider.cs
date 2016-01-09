// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Boss03_Collider : MonoBehaviour
{

    public EndlessBoss03 CurrentBoss;
    public GameObject BossTargetedExplosionEffect;

    // Use this for initialization
    void Start()
    {
        //CurrentBoss = this.gameObject.transform.parent.GetComponent<EndlessBoss03>();
    }



    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "missile")
        {
            Debug.Log("TARGET HIT");
            CurrentBoss.StartCoroutine("BossHit");
            Instantiate(BossTargetedExplosionEffect, transform.position, Quaternion.identity);
            EndlessPlayerController._MissileActive = false;
            Destroy(Other.gameObject);  // destroy missile
            Destroy(this.gameObject);  // destroy this collider
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

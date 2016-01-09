// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss05 : MonoBehaviour {

    public GameObject PreDeathEffect;
    public GameObject Explosion;
    public GameObject Ammo05;
    private int health;
    private int maxHP;
    private bool _AddingScore = false;
    private bool _WasPurged = false;

	// Use this for initialization
	void Start () 
    {
        health = 30000;
        maxHP = 30000;
	}

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.tag == "Player_Laser")
        {
            health = (health - 100);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        }
        if (Other.tag == "Purge" && !_WasPurged)
        {
            _WasPurged = true;
            health = (health - 7500);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        }
    }

    void Fire()
    {
        // called from animation
        Ammo05.SetActive(true);
        Debug.Log("FIRE!");
    }

    void StopFire()
    {
        // called from animation
        Ammo05.SetActive(false);
        Debug.Log("Stop fire");
    }

    IEnumerator AddScore()
    {
        _AddingScore = true;
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        EndlessEnemySystem.BossDying = true;
        EndlessPlayerController.Score += 80000;
        PreDeathEffect = Instantiate(PreDeathEffect, transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(2f);
        Instantiate(Explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3(0, (EndlessEnemySystem.TopBlock.transform.position.y - 45), 0); // new position

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }	
	}
}

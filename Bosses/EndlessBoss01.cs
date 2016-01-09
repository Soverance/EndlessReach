// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss01: MonoBehaviour {

    public GameObject BulletObject;
    public GameObject PreDeathEffect;
    public GameObject ExplosionBoss;
    public GameObject ExplosionDamage;   
    private float TopBlockPosition;
    private float amplitude = 50.0f;  // size
    private float wavelength = 1.0f;  // speed
    private float index;
    public int health;
    private int maxHP;
    private bool _AddingScore = false;
    private bool _WasPurged = false;
    
	// Use this for initialization
	void Start () {
        health = 20000;
        maxHP = 20000;
        StartCoroutine(Fire());
	}

    private IEnumerator Fire()
    {
        while (EndlessEnemySystem._GameInfo._IsBossDefeated == false)
        {
            Vector3 BulletPos = new Vector3(transform.position.x, (transform.position.y - 12), transform.position.z);
            Instantiate(BulletObject, BulletPos, Quaternion.Euler(0, 0, 0));
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator AddScore()
    {
        _AddingScore = true;
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        EndlessEnemySystem.BossDying = true;
        BulletObject.SetActive(false);
        renderer.enabled = false;
        EndlessPlayerController.Score += 80000;
        PreDeathEffect = Instantiate(PreDeathEffect, transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(2f);
        Instantiate(ExplosionBoss, transform.position, Quaternion.identity);
        MasterAudio.PlaySoundAndForget("Explosion_Boss01", 1);
        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);        
    }

    public void TakeGunDamage()
    {
        health = (health - 100);
        EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        Instantiate(ExplosionDamage, transform.position, Quaternion.identity);
        MasterAudio.PlaySoundAndForget("Hit", 1);
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player_Laser")
        {
            TakeGunDamage();
        }
        if (Other.tag == "Purge" && !_WasPurged)
        {
            _WasPurged = true;
            Debug.Log("PURGED ON BOSS");
            health = (health - 6250);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Set default values each frame this boss is alive
        index += Time.deltaTime;
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        float x = amplitude * Mathf.Sin (wavelength * index);  // sine wave for difficulty increase
        transform.position = new Vector3(x, (TopBlockPosition - 100), 0); // new position
        BulletObject.transform.rotation = Quaternion.Euler(90,90,0); // always make BulletObject on stationary plane so it can collide with the player

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

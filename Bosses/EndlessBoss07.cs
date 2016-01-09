// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss07 : MonoBehaviour
{
    public GameObject PreDeathEffect;
    public GameObject Explosion;
    private float TopBlockPosition;
    private float amplitude = 25.0f;  // size
    private float wavelength = 1.0f;  // speed
    private float index;
    private int health;
    private int maxHP;
    private bool _AddingScore = false;
    private bool _WasPurged = false;

    // Use this for initialization
    void Start()
    {
        health = 30000;
        maxHP = 30000;
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

    // Update is called once per frame
    void Update()
    {
        // Set default values each frame this boss is alive
        index += Time.deltaTime;
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        float x = amplitude * Mathf.Sin(wavelength * index);  // sine wave for difficulty increase
        transform.position = new Vector3(x, (TopBlockPosition - 35), 0); // new position
        
        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

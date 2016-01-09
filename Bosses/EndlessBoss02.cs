// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss02 : MonoBehaviour
{
    public GameObject CenterCannon;
    public GameObject OuterCannon;
    public GameObject PreDeathEffect;
    public GameObject Explosion;
    public GameObject ExplosionDamage;   
    public GameObject icegolem;
    private float TopBlockPosition;
    private float amplitude = 50.0f;  // size
    private float wavelength = 1.0f;  // speed
    private float index;
    public int health;
    private int maxHP;
    private bool _AddingScore = false;
    private bool _WasPurged = false;

    // Use this for initialization
    void Start()
    {
        health = 25000;
        maxHP = 25000;
        StartCoroutine("Fire");

        animation["idle"].layer = 0;
        animation["idle"].wrapMode = WrapMode.Once;
        animation["idle"].speed = 0.4f;

        animation["punch"].layer = 1;
        animation["punch"].wrapMode = WrapMode.Once;
        animation["punch"].speed = 0.2f;

        animation["hpunch"].layer = 1;
        animation["hpunch"].wrapMode = WrapMode.Once;
        animation["hpunch"].speed = 0.2f;

        animation["hit"].layer = 2;
        animation["hit"].wrapMode = WrapMode.Once;
        animation["hit"].speed = 0.5f;

        animation["death"].layer = 2;
        animation["death"].wrapMode = WrapMode.Once;
        animation["death"].speed = 0.2f;
    }

    private IEnumerator Fire()
    {
        while (EndlessEnemySystem._GameInfo._IsBossDefeated == false)
        {
            MasterAudio.PlaySoundAndForget("boss02_punch", 1);
            animation.CrossFade("punch");
            yield return new WaitForSeconds(0.5f);
            OuterCannon.SetActive(true);
            yield return new WaitForSeconds(animation.clip.length);            
            animation.CrossFade("idle");
            OuterCannon.SetActive(false);
            yield return new WaitForSeconds(animation.clip.length);
            MasterAudio.PlaySoundAndForget("boss02_punch", 1);
            animation.CrossFade("punch");
            yield return new WaitForSeconds(0.5f);
            OuterCannon.SetActive(true);
            yield return new WaitForSeconds(animation.clip.length);            
            animation.CrossFade("idle");
            OuterCannon.SetActive(false);
            yield return new WaitForSeconds(animation.clip.length);
            MasterAudio.PlaySoundAndForget("boss02_jump", 1);
            animation.CrossFade("hpunch");
            CenterCannon.SetActive(true);
            OuterCannon.SetActive(true);
            yield return new WaitForSeconds(animation.clip.length);            
                       
            yield return new WaitForSeconds(animation.clip.length);
            OuterCannon.SetActive(false);
            CenterCannon.SetActive(false);
            animation.CrossFade("idle"); 
            yield return new WaitForSeconds(animation.clip.length);
        }
    }

    public IEnumerator BossHit()
    {
        //Debug.Log("starting hit");
        StopCoroutine("Fire");
        yield return new WaitForSeconds(0.1f);
        animation.CrossFade("hit");
        health = (health - 2500);  // -10% of Boss01 health
        MasterAudio.PlaySoundAndForget("boss02_hit", 1);
        yield return new WaitForSeconds(animation.clip.length);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine("Fire");
        //Debug.Log("finished hit");
    }

    IEnumerator AddScore()
    {
        StopCoroutine("Fire");
        MasterAudio.PlaySoundAndForget("boss02_death", 1);
        animation.CrossFade("death");
        _AddingScore = true;
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        EndlessEnemySystem.BossDying = true;
        EndlessPlayerController.Score += 80000;
        PreDeathEffect = Instantiate(PreDeathEffect, transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(2f);        
        Instantiate(Explosion, transform.position, Quaternion.identity);
        MasterAudio.PlaySoundAndForget("Explosion_Boss01", 1);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player_Laser")
        {
            health = (health - 100);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
            Instantiate(ExplosionDamage, Other.transform.position, Quaternion.identity);
        }
        if (Other.tag == "Purge" && !_WasPurged)
        {
            _WasPurged = true;
            health = (health - 6250);
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
        transform.position = new Vector3(x, (TopBlockPosition + 200), 600); // new position
        transform.rotation = Quaternion.Euler(90, 0, 180);
        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

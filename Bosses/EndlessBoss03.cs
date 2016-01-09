// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss03 : MonoBehaviour
{

    public GameObject Cannon_Center;
    public GameObject Cannon_Left;
    public GameObject Cannon_Right;
    public GameObject PreDeathEffect;
    public GameObject Explosion;
    public GameObject HitEffect1;
    private float TopBlockPosition;
    private float amplitude = 50.0f;  // size
    private float wavelength = 1.0f;  // speed
    private float index;
    private int health;
    private int maxHP;
    private bool _AddingScore = false;
    private bool _WasPurged = false;
    private bool _IsHit = false;

    // Use this for initialization
    void Start()
    {
        health = 25000;
        maxHP = 25000;
        StartCoroutine("Fire");

        animation["idle"].layer = 0;
        animation["idle"].wrapMode = WrapMode.Once;
        animation["idle"].speed = 0.8f;

        animation["attack 1"].layer = 1;
        animation["attack 1"].wrapMode = WrapMode.Once;
        animation["attack 1"].speed = 0.5f;

        animation["attack 2"].layer = 1;
        animation["attack 2"].wrapMode = WrapMode.Once;
        animation["attack 2"].speed = 0.5f;

        animation["magic"].layer = 1;
        animation["magic"].wrapMode = WrapMode.Once;
        animation["magic"].speed = 0.8f;

        animation["hit 1"].layer = 2;
        animation["hit 1"].wrapMode = WrapMode.Once;
        animation["hit 1"].speed = 1f;

        animation["blink"].layer = 2;
        animation["blink"].wrapMode = WrapMode.Once;
        animation["blink"].speed = 0.5f;

        animation["death"].layer = 2;
        animation["death"].wrapMode = WrapMode.Once;
        animation["death"].speed = 0.5f;
    }

    IEnumerator AddScore()
    {
        StopCoroutine("Fire");
        _AddingScore = true;        
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        EndlessEnemySystem.BossDying = true;
        animation.CrossFade("death");
        MasterAudio.PlaySoundAndForget("boss03_death", 1);
        EndlessPlayerController.Score += 80000;
        PreDeathEffect = Instantiate(PreDeathEffect, new Vector3(transform.position.x, (transform.position.y + 4), transform.position.x), Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(3.5f);
        Instantiate(Explosion, transform.position, Quaternion.identity);
        MasterAudio.PlaySoundAndForget("Explosion_Boss01", 1);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    public IEnumerator BossHit()
    {
        _IsHit = true;
        StopCoroutine("Fire");
        Cannon_Center.SetActive(false);
        Cannon_Left.SetActive(false);
        Cannon_Right.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        animation.CrossFade("hit 1");
        health = (health - 2500);  // -10% of Boss03 health
        MasterAudio.PlaySoundAndForget("boss03_hit", 1);
        yield return new WaitForSeconds(animation.clip.length);
        animation.CrossFade("blink");
        yield return new WaitForSeconds(1f);
        _IsHit = false;
        StartCoroutine("Fire");        
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player_Laser")
        {
            health = (health - 100);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
            Instantiate(HitEffect1, new Vector3(transform.position.x, (transform.position.y + 10), transform.position.x), Quaternion.identity);      
        }
        if (Other.tag == "Purge" && !_WasPurged)
        {
            _WasPurged = true;
            health = (health - 6250);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        }
    }

    private IEnumerator Fire()
    {
        while (EndlessEnemySystem._GameInfo._IsBossDefeated == false && _IsHit == false)
        {
            animation.Stop();
            animation.CrossFade("idle");
            yield return new WaitForSeconds(1f);
            Cannon_Left.SetActive(true);
            MasterAudio.PlaySoundAndForget("boss03_cannon_light", 1);
            yield return new WaitForSeconds(0.5f);
            Cannon_Left.SetActive(false);
            yield return new WaitForSeconds(2f);
            Cannon_Right.SetActive(true);
            MasterAudio.PlaySoundAndForget("boss03_cannon_light", 1);
            yield return new WaitForSeconds(0.5f);
            Cannon_Right.SetActive(false);
            yield return new WaitForSeconds(1f);
            animation.CrossFade("magic");
            yield return new WaitForSeconds(2f);
            MasterAudio.PlaySoundAndForget("boss03_cannon", 1);
            Cannon_Center.SetActive(true);
            yield return new WaitForSeconds(2f);
            Cannon_Center.SetActive(false);
            animation.CrossFade("magic");
            yield return new WaitForSeconds(2f);
            MasterAudio.PlaySoundAndForget("boss03_cannon_heavy", 1);
            Cannon_Center.SetActive(true);
            Cannon_Left.SetActive(true);
            Cannon_Right.SetActive(true);
            yield return new WaitForSeconds(2f);
            Cannon_Center.SetActive(false);
            Cannon_Left.SetActive(false);
            Cannon_Right.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Set default values each frame this boss is alive
        index += Time.deltaTime;
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        float x = amplitude * Mathf.Sin(wavelength * index);  // sine wave for difficulty increase
        transform.position = new Vector3(x, (TopBlockPosition), 250); // new position

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

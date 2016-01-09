// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss04 : MonoBehaviour
{

    public GameObject Cannon_Center;
    public GameObject HandL;    
    public GameObject HandR;
    public GameObject HandEffectL;
    public GameObject HandEffectR;
    public GameObject ATK_L;
    public GameObject ATK_R;
    public GameObject HitEffect;
    public GameObject PreDeathEffect;
    public GameObject Explosion;    
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
        health = 35000;
        maxHP = 35000;
        StartCoroutine(Fire());

        animation["Stand"].layer = 0;
        animation["Stand"].wrapMode = WrapMode.Once;
        animation["Stand"].speed = 1f;

        animation["Idle"].layer = 0;
        animation["Idle"].wrapMode = WrapMode.Once;
        animation["Idle"].speed = 1f;

        animation["Attack 1"].layer = 1;
        animation["Attack 1"].wrapMode = WrapMode.Once;
        animation["Attack 1"].speed = 0.5f;

        animation["Roar"].layer = 1;
        animation["Roar"].wrapMode = WrapMode.Once;
        animation["Roar"].speed = 0.5f;

        animation["Cast Magic"].layer = 1;
        animation["Cast Magic"].wrapMode = WrapMode.Once;
        animation["Cast Magic"].speed = 0.5f;

        animation["Hit 1"].layer = 2;
        animation["Hit 1"].wrapMode = WrapMode.Once;
        animation["Hit 1"].speed = 0.5f;

        animation["Death 2"].layer = 2;
        animation["Death 2"].wrapMode = WrapMode.Once;
        animation["Death 2"].speed = 0.5f;
    }

    IEnumerator AddScore()
    {
        _AddingScore = true;
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        animation.CrossFade("Death 2");
        MasterAudio.PlaySoundAndForget("boss04_atk2", 1);
        EndlessPlayerController.Score += 80000;
        EndlessEnemySystem.BossDying = true;
        Instantiate(PreDeathEffect, transform.position, Quaternion.identity);
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
            health = (health - 500);
            MasterAudio.PlaySoundAndForget("boss03_hit1", 1);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
            Instantiate(HitEffect, new Vector3(transform.position.x, (transform.position.y + 10), transform.position.x), Quaternion.identity);
        }
        if (Other.tag == "Purge" && !_WasPurged)
        {
            _WasPurged = true;
            health = (health - 3750);
            EndlessEnemySystem._HUD.UpdateBossHealth((float)health / maxHP);  // update health bar with HP percentage
        }
    }

    public IEnumerator BossHit()
    {
        _IsHit = true;
        StopCoroutine("Fire");
        Cannon_Center.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        animation.CrossFade("Hit 1");
        MasterAudio.PlaySoundAndForget("boss04_hit", 1);
        health = (health - 2500);  // -10% of Boss03 health
        yield return new WaitForSeconds(animation.clip.length);
        animation.CrossFade("Stand");
        yield return new WaitForSeconds(1f);
        _IsHit = false;
        StartCoroutine("Fire");
    }

    private IEnumerator Fire()
    {
        while (EndlessEnemySystem._GameInfo._IsBossDefeated == false && _IsHit == false)
        {
            animation.CrossFade("Stand");
            yield return new WaitForSeconds(1f);
            animation.CrossFade("Idle");
            yield return new WaitForSeconds(3);
            animation.CrossFade("Roar");
            MasterAudio.PlaySoundAndForget("boss04_center", 1);
            yield return new WaitForSeconds(2);
            Cannon_Center.SetActive(true);
            MasterAudio.PlaySoundAndForget("boss03_cannon", 1);
            yield return new WaitForSeconds(4);
            Cannon_Center.SetActive(false);
            HandEffectL.SetActive(true);
            HandEffectR.SetActive(true);
            MasterAudio.PlaySoundAndForget("boss04_atk1", 1);
            animation.CrossFade("Cast Magic");
            yield return new WaitForSeconds(2.5f);
            MasterAudio.PlaySoundAndForget("boss03_cannon_light", 1);
            Instantiate(ATK_L, HandL.transform.position, Quaternion.identity);
            Instantiate(ATK_R, HandR.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            HandEffectL.SetActive(false);
            HandEffectR.SetActive(false);
            animation.CrossFade("Attack 1");
            yield return new WaitForSeconds(2);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        // Set default values each frame this boss is alive
        index += Time.deltaTime;
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        float x = amplitude * Mathf.Sin(wavelength * index);  // sine wave for difficulty increase
        transform.position = new Vector3(x, (TopBlockPosition + 100), 350); // new position

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

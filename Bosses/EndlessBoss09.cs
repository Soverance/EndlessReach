// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss09 : MonoBehaviour
{
    public GameObject AmmoEyes;
    public GameObject AmmoLeft;
    public GameObject AmmoRight;
    public GameObject Pentagram;
    public AudioClip GuardianTaunt;
    public AudioClip GuardianDeath;
    public int health;
    public int maxHP;
    public bool _AddingScore = false;
    public bool _WasPurged = false;
    private float TopBlockPosition;
    private Animator _Anim;
    public GameObject PreDeathEffect;
    public GameObject Explosion;
    private Boss09_Collider Guardian;

    // Use this for initialization
    void Start()
    {
        health = 50000;
        maxHP = 50000;
        _Anim = gameObject.GetComponent<Animator>();
        Guardian = gameObject.GetComponentInChildren<Boss09_Collider>();
    }

    void FireCenter()
    {
        AmmoEyes.SetActive(true);
    }

    void FireLeft()
    {
        AmmoLeft.SetActive(true);
    }
    void FireRight()
    {
        AmmoRight.SetActive(true);
    }
    void StopCenter()
    {
        AmmoEyes.SetActive(false);
    }
    void StopLeft()
    {
        AmmoLeft.SetActive(false);
    }
    void StopRight()
    {
        AmmoRight.SetActive(false);
    }
    void StartPentagram()
    {
        audio.PlayOneShot(GuardianTaunt);
        Pentagram.SetActive(true);
    }
    void StopPentagram()
    {
        Pentagram.SetActive(false);
    }

    public IEnumerator AddScore()
    {
        EndlessPlayerController.Score += 100000;
        EndlessEnemySystem.BossDying = true;
        _Anim.enabled = false;
        _AddingScore = true;
        audio.PlayOneShot(GuardianDeath);
        yield return new WaitForSeconds(6f);
        Vector3 effectsPos = new Vector3(Guardian.transform.position.x, (Guardian.transform.position.y + 15), transform.position.x);
        PreDeathEffect = Instantiate(PreDeathEffect, effectsPos, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(2f);
        Instantiate(Explosion, effectsPos, Quaternion.identity);
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        transform.position = new Vector3(0, (TopBlockPosition - 75), 0); // new position

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}

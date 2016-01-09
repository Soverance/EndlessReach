// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessBoss10 : MonoBehaviour
{
    public GameObject AmmoEyes;
    public GameObject AmmoMouth;
    public GameObject Pentagram;
    public AudioClip OmegaTaunt;
    public AudioClip OmegaDeath;
    public SpriteRenderer OmegaFace;
    public Sprite OpenFace;
    public Sprite ClosedFace;
    public int health;
    public int maxHP;
    public bool _AddingScore = false;
    public bool _WasPurged = false;
    private float TopBlockPosition;
    private Animator _Anim;
    public GameObject PreDeathEffect;
    public GameObject Explosion;

    private float amplitude = 25.0f;  // size
    private float wavelength = 1.0f;  // speed
    private float index;

    // Use this for initialization
    void Start()
    {
        health = 50000;
        maxHP = 50000;
        _Anim = gameObject.GetComponent<Animator>();
    }
    void Shake()
    {
        Vector3 Amount = new Vector3(10, 10, 0);
        iTween.ShakePosition(gameObject, Amount, 1f);
    }
    void FireEyes()
    {
        AmmoEyes.SetActive(true);
    }
    void StopEyes()
    {
        AmmoEyes.SetActive(false);
    }
    void StartPentagram()
    {
        OmegaFace.sprite = OpenFace;
        audio.PlayOneShot(OmegaTaunt);
        Pentagram.SetActive(true);
    }
    void StopPentagram()
    {
        Pentagram.SetActive(false);
    }
    void FireMouth()
    {
        AmmoMouth.SetActive(true);
    }
    void StopMouth()
    {
        OmegaFace.sprite = ClosedFace;
        AmmoMouth.SetActive(false);
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

    public IEnumerator AddScore()
    {
        EndlessPlayerController.Score += 100000;
        EndlessEnemySystem.BossDying = true;
        _Anim.enabled = false;
        _AddingScore = true;
        audio.PlayOneShot(OmegaDeath);
        yield return new WaitForSeconds(6f);
        Vector3 effectsPos = new Vector3(transform.position.x, (transform.position.y + 15), transform.position.x);
        PreDeathEffect = Instantiate(PreDeathEffect, effectsPos, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(2f);
        Instantiate(Explosion, effectsPos, Quaternion.identity);
        EndlessEnemySystem._GameInfo._IsBossDefeated = true;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        index += Time.deltaTime;
        float x = amplitude * Mathf.Sin(wavelength * index);  // sine wave for difficulty increase
        TopBlockPosition = EndlessEnemySystem.TopBlock.transform.position.y;
        transform.position = new Vector3(x, (TopBlockPosition - 40), 0); // new position

        if (health <= 0 && _AddingScore == false)
        {
            StartCoroutine(AddScore());
        }
    }
}
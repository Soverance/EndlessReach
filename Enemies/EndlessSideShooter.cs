// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessSideShooter: MonoBehaviour
{
    private int Health = 200;
    private float MinSpeed = 100;  // min speed
    private float MaxSpeed = 200;  // max speed
    private float CurrentSpeed;  // current speed
    private float Timer = 0f;
    private float FireTimer;
    private float Lifetime = 10f;
    private bool _IsDiving;
    private bool _IsFiring;
    private bool _IsAlive;
    private bool _IsPurged;

    private EndlessEnemySystem _EES;
    public GameObject SideShooterExplosion;
    public GameObject SideBullet;
    public GameObject SideShield;

    void Awake()
    {
        _IsDiving = true;
        _IsFiring = false;
        _IsAlive = true;
        _IsPurged = false;
    }

    void Start()
    {
        // randomly generate a speed based on Min and Max set in editor
        CurrentSpeed = Random.Range(MinSpeed, MaxSpeed);
        FireTimer = Random.Range(0.5f, 1f);
        Timer = Time.time;
        _EES = EndlessEnemySystem._EES;
    }

    public void AddScore()
    {
        _IsAlive = false;
        EndlessPlayerController.Score += 2000;
        MasterAudio.PlaySoundAndForget("Explosion_SideShooter", 1);
        Instantiate(SideShooterExplosion, this.transform.position, Quaternion.identity);        
        // if enemy was NOT purged
        if (!_IsPurged)
        {
            switch (Application.loadedLevel)
            {
                case 7:
                case 8:
                case 9:
                    _EES.SetDefaultProperties_Random();
                    break;
                default:
                    _EES.SetDefaultProperties_SideShooter();
                    break;
            }   
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider OtherObject)
    {
        if (OtherObject.transform.tag == "Player_Laser")
        {
            Health -= 100; 
            if (Health > 0)
            {
                SideShield.SetActive(false);
            }
        }

        if (OtherObject.tag == "Purge")
        {
            _IsPurged = true;
            AddScore();
        }
    }

    void OnTriggerExit(Collider OtherObject)
    {
        SideShield.SetActive(false);
    }

    private IEnumerator Fire()
    {        
        //int Side = Random.Range(1, 3);
        _IsFiring = true;

        //if (_IsAlive)
        //{
        //    switch (Side)
        //    {
        //        case 1:
        //            Vector3 MoveRight = new Vector3((transform.position.x + 20), transform.position.y, transform.position.z);
        //            iTween.MoveTo(this.gameObject, iTween.Hash("position", MoveRight, "time", 1, "easetype", iTween.EaseType.easeInOutQuad));
        //            break;
        //        case 2:
        //            Vector3 MoveLeft = new Vector3((transform.position.x - 20), transform.position.y, transform.position.z);
        //            iTween.MoveTo(this.gameObject, iTween.Hash("position", MoveLeft, "time", 1, "easetype", iTween.EaseType.easeInOutQuad));
        //            break;
        //    }
        //}        
        if (_IsAlive)
        {
            Instantiate(SideBullet, transform.position, Quaternion.identity);
        }        
        yield return new WaitForSeconds(1.5f);
        _IsDiving = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (_IsDiving == true)
        {
            // move the fighter across the screen
            float MoveDistance = CurrentSpeed * Time.deltaTime;
            transform.Translate(Vector3.right * MoveDistance);
        }

        if ((Timer + FireTimer) < Time.time && _IsFiring == false)
        {
            _IsDiving = false;
            StartCoroutine(Fire());
        }

        if (Timer + Lifetime < Time.time)
        {
            Destroy(this.gameObject);
        }

        if (Health <= 0)
        {
            AddScore();   
        }
    }
}

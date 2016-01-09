// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessFighter: MonoBehaviour
{
    private float MinSpeed = 90;  // min speed, set in editor
    private float MaxSpeed = 160;  // max speed, set in editor
    private float CurrentSpeed;  // current speed
    private float Timer = 0f;
    private float Lifetime = 7f;
    private bool _IsPurged;

    private EndlessEnemySystem _EES;
    public GameObject FighterExplosion;

    void Start()
    {
        // randomly generate a speed based on Min and Max set in editor
        CurrentSpeed = Random.Range(MinSpeed, MaxSpeed);
        Timer = Time.time;
        _EES = EndlessEnemySystem._EES;
        _IsPurged = false;
    }

    public void AddScore()
    {
        MasterAudio.PlaySoundAndForget("Explosion_Fighter", 1);
        Instantiate(FighterExplosion, this.transform.position, Quaternion.identity);
        EndlessPlayerController.Score += 1500;
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
                    _EES.SetDefaultProperties_Fighter();
                    break;
            }
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider OtherObject)
    {
        if (OtherObject.transform.tag == "Player_Laser")
        {
            AddScore();            
        }

        if (OtherObject.tag == "Purge")
        {
            _IsPurged = true;
            AddScore();
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!EndlessPlayerController._Overloading)
        {
            // move the fighter across the screen
            float MoveDistance = CurrentSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * MoveDistance);
        }
        else
        {
            float OverloadingSpeed = 10;
            float MoveDistance = OverloadingSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * MoveDistance);            
        }        

        if (Timer + Lifetime < Time.time)
        {
            Destroy(this.gameObject);
        }
	}
}

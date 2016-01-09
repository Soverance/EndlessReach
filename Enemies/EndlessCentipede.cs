// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessCentipede : MonoBehaviour
{
    private float MinSpeed = 10;  // min speed, set in editor
    private float MaxSpeed = 30;  // max speed, set in editor
    private float CurrentSpeed;  // current speed
    private float Timer = 0f;
    private float Lifetime = 10f;

    public int SegmentCount;
    public bool _IsPurged;

    void Start()
    {        
        CurrentSpeed = Random.Range(MinSpeed, MaxSpeed);
        Timer = Time.time;
        _IsPurged = false;
        CountSegments();        
    }

    void CountSegments()
    {
        SegmentCount = 0;
        switch (gameObject.tag)
        {
            case "3Seg":
                SegmentCount = 3;
                break;
            case "4Seg":
                SegmentCount = 4;
                break;
            case "5Seg":
                SegmentCount = 5;
                break;
        }
    }

    public void AddScore()
    {
        EndlessPlayerController.Score += 1000;
        if (SegmentCount == 0)
        {
            if (!_IsPurged)
            {
                switch (Application.loadedLevel)
                {
                    case 7:
                    case 8:
                    case 9:
                        EndlessEnemySystem._EES.SetDefaultProperties_Random();
                        break;
                    default:
                        EndlessEnemySystem._EES.SetDefaultProperties_Centipede();
                        break;
                }
            }          
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!EndlessPlayerController._Overloading)
        {
            float MoveDistance = CurrentSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * MoveDistance);
        }
        else
        {
            float OverloadingSpeed = 10;
            float MoveDistance = OverloadingSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * MoveDistance);
        }

        if (Timer + Lifetime < Time.time)
        {
            Destroy(this.gameObject);
        }
    }
}

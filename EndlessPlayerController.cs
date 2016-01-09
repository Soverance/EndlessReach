// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class EndlessPlayerController : MonoBehaviour 
{
    private EndlessGameInfo GameInfo;
    // XInput stuff
    //bool playerIndexSet = false;
    //PlayerIndex playerIndex;
    //GamePadState state;
    //GamePadState prevState;
    private PlayerIndex playerIndex;
    private GamePadState state;
    
    // objects and components
    private GameObject FPSCounter;
    private GameObject LimitBreak_Text;
    private GameObject PurgeButton;
    public GameObject Missile_Basic;
    public GameObject BlueLaser;  
    public GameObject GreenLaser;
    public GameObject YellowLaser;
    public GameObject RedLaser;
    public GameObject LimitBreak_Shield;
    public GameObject LimitBreak_Laser;
    private GameObject LimitCounter;
    public GameObject OverloadEffect;
    public GameObject PurgeSuccess;
    public GameObject PurgeFail;
    public GameObject Explosion_Player; 
    public GameObject StandardEngine;  
    public GameObject BoostEngine;
    public GameObject BoostShockwave;
    private BoxCollider PlayerCollider;
    private Vector2 MaxSpeed;
    private Vector2 CamSpeed;
    Rigidbody _rigidbody;
    
    // define some default properties
    private bool _Paused;
    public static int Score;
    public static int Lives;
    public static int _FireRate;
    public static int _BoostRate;
    public static float _InitialRate;
    public static float _NewRate;
    public static bool _IsDestroyed;
    public static bool _IsHidden;
    public static bool _Overloading;
    public static bool _LockedOn;
    public static bool _MissileActive;
    private bool _CanFireMissile;
    private bool _CanFire;
    private bool _CanBoost;
    private bool _IsBoosting;
    private float _CurrentBoost;
    private float _MaxBoost;
    //private float _CourseCorrection;
    //private bool _Bounding;
    private static bool _ManuallyPurged;
    private bool _ReadyToPurge;
    private bool _VibLow;
    private bool _VibHigh;
    private bool _FPSActive;

    void Awake()
    {
        // sets default properties and calls fire control
        PlayerCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        PurgeButton = GameObject.Find("ButtonX");
        PurgeButton.SetActive(false);        
        MaxSpeed = new Vector2(150.0f, 150.0f);
        _CurrentBoost = 1000f;
        _MaxBoost = 1000f;
        //_CourseCorrection = 20f;
        Score = 0;
        Lives = 1;
        _FireRate = 1;
        _BoostRate = 1;
        _IsDestroyed = false;
        _IsHidden = false;
        _Overloading = false;
        _LockedOn = false;
        _ManuallyPurged = false;
        _ReadyToPurge = false;
        //_Bounding = false;
        _MissileActive = false;
        _CanFireMissile = true;
        _CanFire = true;
        _CanBoost = true;
        _IsBoosting = false;
        _VibLow = false;
        _VibHigh = false;
        renderer.enabled = true;
        PlayerCollider.enabled = true;
        StandardEngine.SetActive(true);
        _FPSActive = false;
        _Paused = false;
    }

    void Start()
    {
        GameInfo = EndlessEnemySystem._GameInfo;
        LimitBreak_Text = GameObject.Find("LimitBreakText");
        LimitCounter = GameObject.Find("LimitCounter");
        LimitCounter.SetActive(false);
        FPSCounter = GameObject.Find("FPSCounter");
        FPSCounter.SetActive(false);
        _NewRate = _InitialRate;  // Set Default powerup respawn rate  (used for limit break timer)
    }

    #region Collision and Destruction

    // Coroutine to enable ship respawn properties
    IEnumerator DestroyShip()
    {
        GamePad.SetVibration(playerIndex, 1f, 1f);
        MasterAudio.PlaySoundAndForget("Player_Explosion", 1);
        Lives--;  // decrement lives counter
        _FireRate = 1;
        _IsDestroyed = true;
        Explosion_Player = Instantiate(Explosion_Player, this.transform.position, Random.rotation) as GameObject;
        HidePlayer();
        yield return new WaitForSeconds(0.3f);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    // handles anything colliding with the player
    void OnTriggerEnter(Collider OtherObject)
    {
            // if the collider is in the enemy or bullet layer  (Layer 8)
        if (OtherObject.transform.gameObject.layer == 8 || OtherObject.transform.gameObject.layer == 10)
            {
                if (_FireRate == 5)
                {
                    // Do Nothing while Limit Breaking                
                }
                else
                {
                    //Debug.Log("Enemy Collision");
                    StartCoroutine(DestroyShip());  // Call to destroy ship
                }
            }

            // if the collider is in the bounding layer  (Layer 14)
            if (OtherObject.transform.gameObject.layer == 14)
            {
                Debug.Log("Bounding Collision");
                //_Bounding = true;
                
            }

            //// bullet layer (Layer 10)
            //if (OtherObject.transform.gameObject.layer == 10)
            //{
            //    if (_FireRate == 5)
            //    {
            //        // Do Nothing while Limit Breaking                
            //    }
            //    else
            //    {
            //        //Debug.Log("Enemy Collision");
            //        StartCoroutine(DestroyShip());  // Call to destroy ship
            //    }
            //}

            // PowerUp layer (Layer 9)  CAN ONLY OBTAIN POWERUPS WHEN NOT OVERLOADING
            if (OtherObject.transform.gameObject.layer == 9 && !_Overloading)
            {
                OtherObject.transform.gameObject.SetActive(false);
                MasterAudio.PlaySoundAndForget("Player_Pickup", 1);

                if (OtherObject.transform.gameObject.tag == "attack1")
                {
                    //Debug.Log(_InitialRate);
                    switch (_FireRate)
                    {
                        case 1:
                            _FireRate = 2;
                            break;
                        case 2:
                            _FireRate = 3;
                            break;
                        case 3:
                            _FireRate = 4;
                            break;
                        case 4:
                            _FireRate = 5;
                            StartCoroutine("LimitBreak");
                            break;
                        case 5:
                            Score += 25000;
                            _NewRate = _InitialRate;  // Reset Limit Timer
                            StopCoroutine("LimitBreak");
                            StartCoroutine("LimitBreak");
                            break;
                    }
                }

                if (OtherObject.transform.tag == "defender1")
                {
                    // ADD TO BOOST METER IF NOT AT CAPACITY
                    if (_CurrentBoost < _MaxBoost)
                    {
                        _CurrentBoost = _CurrentBoost + 200;
                        EndlessEnemySystem._HUD.UpdateBoostMeter((float)_CurrentBoost / _MaxBoost);  // update health bar with HP percentage
                    }                    
                }
            }
    }

    void OnTriggerStay(Collider OtherObject)
    {
        // if the collider is in the bounding layer  (Layer 14)
        if (OtherObject.transform.gameObject.layer == 14)
        {
            Debug.Log("Bounding Stay");
            //Vector3 forceVec = -_rigidbody.velocity.normalized * _CourseCorrection;
            //_rigidbody.AddForce(forceVec, ForceMode.Acceleration);
        }
    }

    #endregion

    #region Boost Control Group

    void BoostControl()
    {
        GamePad.SetVibration(playerIndex, 1, 0);
        MasterAudio.PlaySoundAndForget("Player_Boost", 1);
        Instantiate(BoostShockwave, transform.position, Quaternion.identity);
        StandardEngine.SetActive(false);
        BoostEngine.SetActive(true);
        MaxSpeed = new Vector2(300.0f, 300.0f);
        
    }
    void CancelBoost()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
        StandardEngine.SetActive(true);
        BoostEngine.SetActive(false);
        MaxSpeed = new Vector2(150.0f, 150.0f);
    }

    #endregion

    #region Fire Control Group

    private IEnumerator MissileControl()
    {
        _MissileActive = true;
        _CanFireMissile = false;
        MasterAudio.PlaySoundAndForget("Player_Missile", 1);
        Vector3 MissilePosition = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(Missile_Basic, MissilePosition, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(1f);
        _CanFireMissile = true;
    }

    public void FireControl()
    {
        switch (_FireRate)
        {
            case 1:
                StartCoroutine(FireOne());
                break;
            case 2:
                StartCoroutine(FireTwo());
                break;
            case 3:
                StartCoroutine(FireThree());
                break;
            case 4:
                StartCoroutine(FireFour());
                break;
            case 5:
                StartCoroutine(FireFive());
                break;
            default :
                break;
        }
    }

    IEnumerator FireOne()
    {
        _CanFire = false;
        Vector3 positionBlue = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(BlueLaser, positionBlue, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.4f);
        _CanFire = true;
    }

    IEnumerator FireTwo()
    {
        _CanFire = false;
        Vector3 positionGreen = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(GreenLaser, positionGreen, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.2f);
        _CanFire = true;
    }

    IEnumerator FireThree()
    {
        _CanFire = false;
        Vector3 positionRed = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(YellowLaser, positionRed, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.1f);
        _CanFire = true;
    }

    IEnumerator FireFour()
    {
        _CanFire = false;
        Vector3 positionMulti = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(RedLaser, positionMulti, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.06f);
        _CanFire = true;
    }

    IEnumerator FireFive()
    {
        _CanFire = false;
        Vector3 positionLimit = new Vector3(transform.position.x, (transform.position.y - 10), transform.position.z);
        Instantiate(LimitBreak_Laser, positionLimit, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.03f);
        _CanFire = true;
    }

    // Limit Break is 20 seconds
    IEnumerator LimitBreak()
    {
        TweenAlpha.Begin(LimitBreak_Text, 0.5f, 1);
        LimitBreak_Shield.SetActive(true);
        LimitCounter.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        TweenAlpha.Begin(LimitBreak_Text, 0.5f, 0);
        
        yield return new WaitForSeconds(_NewRate + 0.5f);
        //Debug.Log("Begin Overload");        
        Overload();
        yield return new WaitForSeconds(4.5f);
        
        if (!_ManuallyPurged)
        {
            GamePad.SetVibration(playerIndex, 1, 1);
            //Debug.Log("Auto-purge initiated");
            _Overloading = false;
            MasterAudio.PlaySoundAndForget("Purge_Failure", 1);
            Vector3 PurgeEffectPosition = new Vector3(0, this.transform.position.y, 0);
            Instantiate(PurgeFail, PurgeEffectPosition, Quaternion.Euler(0, 0, 0));
            StartCoroutine(DestroyShip());  // Call to destroy ship
        }
        
        //Debug.Log("Limit Break Complete");
        
        yield return new WaitForSeconds(0.5f);
        _VibLow = false;
        _VibHigh = false;
        GamePad.SetVibration(playerIndex, 0, 0);
        _ManuallyPurged = false;
        LimitBreak_Shield.SetActive(false);
        LimitCounter.SetActive(false);
        _FireRate = 1;
    }

    private void Overload()
    {
        MasterAudio.PlaySoundAndForget("Ethereal_Purge", 1);
        StartCoroutine(OverloadSlow());
        StartCoroutine(RunOverloadEffect());
        _Overloading = true;
        _VibLow = true;
    }

    private IEnumerator OverloadSlow()
    {
        float OverloadEndTime = Time.realtimeSinceStartup + 4f;

        while (Time.realtimeSinceStartup < OverloadEndTime)
        {
            if (Time.timeScale > 0.1f)
            {
                Time.timeScale -= 0.005f;
            }
            yield return null;
        }
        Time.timeScale = 1;
    }    

    // RunOverloadEffect takes 3.5 seconds to run
    private IEnumerator RunOverloadEffect()
    {
        Vector3 OverloadEffectPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);  // set new position for overload effect
        GameObject Effect = Instantiate(OverloadEffect, OverloadEffectPosition, Quaternion.Euler(0, 0, 0)) as GameObject; // spawn overload effect
        Effect.transform.parent = this.transform;
        yield return new WaitForSeconds(0.1f);  
        //BoxCollider PurgeCollider = Effect.GetComponent<BoxCollider>(); // get purge button collider
        yield return new WaitForSeconds(3.1f);
        _ReadyToPurge = true;
        PurgeButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        _ReadyToPurge = false;
        PurgeButton.SetActive(false);
    }    
    
    private void Purge()
    {
        GamePad.SetVibration(playerIndex, 1, 1);
        Debug.Log("Purging Activated");
        _ManuallyPurged = true;
        _ReadyToPurge = false;
        _Overloading = false;
        MasterAudio.PlaySoundAndForget("Purge_Success", 1);
        Vector3 PurgeEffectPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        Instantiate(PurgeSuccess, PurgeEffectPosition, Quaternion.Euler(0, 0, 0));
    }  

    #endregion

    #region Hide & FPS Label Control

    IEnumerator ToggleFPS()
    {
        if (FPSCounter.activeInHierarchy == false)
        {
            FPSCounter.SetActive(true);
        }
        else
        {
            FPSCounter.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        _FPSActive = false;
    }

    public void HidePlayer()
    {
        // off
        renderer.enabled = false;
        PlayerCollider.enabled = false;
        StandardEngine.SetActive(false);
        BoostEngine.SetActive(false);
        LimitBreak_Shield.SetActive(false);
    }

    public void ShowPlayer()
    {
        renderer.enabled = true;
        PlayerCollider.enabled = true;
        StandardEngine.SetActive(true);
    }
    #endregion   
 
    

    void Update()
    {
        playerIndex = XInputState.playerIndex;
        state = XInputState.state;

        // VIBRATION CONTROL
        if (_VibLow && _Overloading)
        {
            GamePad.SetVibration(playerIndex, 1f, 0f);
        }
        if (_VibHigh && _Overloading)
        {
            GamePad.SetVibration(playerIndex, 0f, 1f);
        }

        // MOVEMENT DEFINITION
        Vector2 velocity = new Vector2();  // New velocity vector each frame

        // MOVEMENT!
        if (!_Overloading)
        {
            velocity.x = state.ThumbSticks.Left.X * MaxSpeed.x;
            velocity.y = state.ThumbSticks.Left.Y * MaxSpeed.y;
            _rigidbody.velocity = velocity;  // apply velocity
        }

        // STOP PLAYER WHEN OVERLOADING OR HITTING A BOUNDS
        if (_Overloading)
        {
            _rigidbody.velocity = new Vector2(0f, 0f);
        }
        
        // DO STUFF DURING LIMIT BREAK
        if (_FireRate == 5)
        {
            _NewRate -= Time.deltaTime;  // subtract time from limit break
        }

        // BOOST ON
        if (_CanBoost && state.Triggers.Left > 0.5f && !_IsHidden && EndlessGameInfo._GameState == 1 && _CurrentBoost > 0)
        {
            _IsBoosting = true;
            _CanBoost = false;
            BoostControl();
            
        }
        // UPDATE BOOST METER
        if (_IsBoosting)
        {
            _CurrentBoost = _CurrentBoost - 20;
            EndlessEnemySystem._HUD.UpdateBoostMeter((float)_CurrentBoost / _MaxBoost);  // update health bar with HP percentage
        }
        // BOOST OFF
        if (!_CanBoost && state.Triggers.Left < 0.5f || _CurrentBoost <= 0)
        {
            _IsBoosting = false;
            _CanBoost = true;
            CancelBoost();
        }

        // MANUAL PURGE with X BUTTON
        if (_ReadyToPurge && state.Buttons.X == ButtonState.Pressed)
        {
            Purge();
        }

        // STANDARD FIRING with A BUTTON
        if (_CanFire && _IsHidden == false && _IsDestroyed == false && state.Buttons.A == ButtonState.Pressed)
        {
            FireControl();
        }

        // MISSILE FIRING with B BUTTON
        if (_CanFire && _IsHidden == false && _IsDestroyed == false && _LockedOn == true && _CanFireMissile == true && _MissileActive == false && state.Buttons.B == ButtonState.Pressed)
        {
            if (GameObject.Find("missile") == false)
            {
                StartCoroutine(MissileControl());
            }
            //Debug.Log("FIRE MISSILE!");
        }

        // TOGGLE FPS COUNTER
        if (state.Buttons.LeftShoulder == ButtonState.Pressed && _FPSActive == false)
        {
            StartCoroutine(ToggleFPS());
            _FPSActive = true;
        }

        //if (state.Buttons.LeftStick == ButtonState.Pressed)
        //{
        //    Application.CaptureScreenshot("EndlessReach_screen.png", 2);
        //}

        // MANUAL HMD Orientation Rest
        //if (state.Buttons.Back == ButtonState.Pressed)
        //{
        //    OVRManager.display.RecenterPose();  // OVR Reset function for 0.4.3
        //}

        #region Pause Control
        if (state.Buttons.Start == ButtonState.Pressed && GameInfo.Paused == false && _Paused == false)
        {
            GameInfo.DoPause();
            _Paused = true;
            HidePlayer();
            _IsHidden = true;            
        }
        #endregion

        #region Unpause Control
        if (state.Buttons.Start == ButtonState.Pressed && GameInfo.Paused == true && _Paused == true)
        {
            GameInfo.UnDoPause();
            _Paused = false;
            ShowPlayer();
            _IsHidden = false;            
        }
        #endregion

        #region Hide Control
        // HIDE PLAYER WHEN BOSS DIES
        if (EndlessEnemySystem.BossDying && Application.loadedLevel != 10)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            HidePlayer();
            StopCoroutine("LimitBreak");
            _IsHidden = true;
        }

        // HIDE PLAYER IF GAME ON STATUS SCREEN
        if (EndlessGameInfo._GameState == 2)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            HidePlayer();
            StopCoroutine("LimitBreak");
            // TURN OFF LIMIT BREAK SHIELD IF ACTIVE
            if (LimitBreak_Shield.activeInHierarchy)
            {
                LimitBreak_Shield.SetActive(false);
            }
        }

        // HIDE PLAYER DURING STATUS SCREEN
        if (EndlessGameInfo.StatusScreen == true && _IsHidden == false && EndlessGameInfo._GameState == 1)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            HidePlayer();
            _IsHidden = true;
        }
        #endregion
    }
}
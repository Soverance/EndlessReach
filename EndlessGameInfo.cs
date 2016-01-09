// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com
// The EndlessGameInfo class handles the general function of the main game loop

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class EndlessGameInfo : MonoBehaviour {

    // Systems and Components
    public static List<int> GlobalHighScores = new List<int>();
    private EndlessEnemySystem EnemySystem;

    // basic objects
    public GameObject PlayerControllerPrefab;  // reference to PlayerShip prefab
    public GameObject FireRatePickup;
    public GameObject BoostPickup;
    public GameObject BlackHoleEffect;  //black hole particle effect

    // HUD and GUI stuff
    public GameObject HUDRoot;
    public GameObject BossHealthBar;
    public GameObject MainRoot;
    public GameObject StatusRoot;
    public GameObject PauseRoot;
    public GameObject ScoreTimer;
    public GameObject _Complete;
    public GameObject _Failure;
    public GameObject _Warning;
    public GameObject _Bronze;
    public GameObject _Silver;
    public GameObject _Gold;
    

    // Master Spawn Volume
    public GameObject TopBlock;  // a reference to the right blocking volume, where enemies will originate
    public static float TopBlockPosition;

    // properties
    public static int _GameState;  // _GameState = 1 when playing, 0 when paused or on a menu screen, and 2 when boss defeated
    private int _Medal;  // 0 = None, 1 = Bronze, 2 = Silver, 3 = Gold
    private int CurrentScore;
    private int _CurrentHighScore;
    private float _PowerupTimer;
    private float _BoostTimer;
    private bool ReadyBoss;
    public bool Paused;
    public static bool StatusScreen;    
    public bool _IsBossDefeated;
    private bool _AllowMedal;
    public static bool _AllowGlobalHighScore;

            
    void Awake ()
    {
        Time.timeScale = 1;  // Reset Time
        EnemySystem = this.GetComponent<EndlessEnemySystem>();
        //LevelAudio = this.GetComponent<AudioSource>();
        CurrentScore = 0;
        _GameState = 1;
        _Medal = 0;
        ReadyBoss = false;
        Paused = false;
        StatusScreen = false;
        _IsBossDefeated = false;
        _AllowMedal = false;
        _AllowGlobalHighScore = false;
    }

	// Use this for initialization
	void Start ()
    {
        TopBlock = GameObject.Find("TopBlock");
        _PowerupTimer = Random.Range(8, 16);  // powerup timer 
        _BoostTimer = Random.Range(10, 20);
        EndlessPlayerController._InitialRate = _PowerupTimer;
        SetDefaultProperties_Player();  // spawn the player
        Invoke("SetDefaultProperties_DoubleFireRate",_PowerupTimer);
        Invoke("SetDefaultProperties_AddBoost", _BoostTimer);
        InvokeRepeating("SetDefaultRespawn_Attack", _PowerupTimer, _PowerupTimer);
        InvokeRepeating("SetDefaultRespawn_Boost", _BoostTimer, _BoostTimer);

        //StartCoroutine(DEBUG_DisplayGlobalScores());
	}


    //IEnumerator DEBUG_DisplayGlobalScores()
    //{
    //    yield return new WaitForSeconds(4f);
    //    foreach (int score in GlobalHighScores)
    //    {
    //        Debug.Log(score.ToString());
    //    }
    //}

    #region Enumerators
    // used to spawn bosses
    public IEnumerator SpawnBoss()
    {
        // this enumerator takes 4.5 seconds to complete
        MasterAudio.PlaySoundAndForget("Ethereal_Warning", 1);
        MasterAudio.PlaySoundAndForget("Alarm_BossWarning", 0.5f);
        _Warning.SetActive(true);
        TweenAlpha.Begin(_Warning, 1.5f, 1);
        Vector3 _BlackHolePos = new Vector3(0, (TopBlockPosition - 30), 0);
        Instantiate(BlackHoleEffect, _BlackHolePos, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        TweenAlpha.Begin(_Warning, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        TweenAlpha.Begin(_Warning, 1.5f, 1);
        yield return new WaitForSeconds(1.5f);
        TweenAlpha.Begin(_Warning, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        TweenAlpha.Begin(_Warning, 1.5f, 1);
        yield return new WaitForSeconds(1.5f);
        TweenAlpha.Begin(_Warning, 0.5f, 0);
        _Warning.SetActive(false);
        //ScoreTimer.SetActive(false);
        BossHealthBar.SetActive(true);

        EnemySystem.SetDefaultProperties_Bosses();
    }

    IEnumerator WinCycle()
    {
        ScoreTimer.SetActive(false);
        BossHealthBar.SetActive(false);
        yield return new WaitForSeconds(5f);
        _AllowMedal = true;
        EnemySystem.CancelEnemyInvokes();
        _GameState = 2;
        MainRoot.SetActive(false);
        StatusRoot.SetActive(true);
        StatusScreen = true;
        _Complete.SetActive(true);
        Scoring();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator FailCycle()
    {
        EnemySystem.CancelEnemyInvokes();
        _GameState = 2;
        ScoreTimer.SetActive(false);
        BossHealthBar.SetActive(false);
        MainRoot.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        StatusRoot.SetActive(true);
        StatusScreen = true;
        _Failure.SetActive(true);
        Scoring();
        if (_AllowGlobalHighScore)
        {
            StatusRoot.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
    }    

    #endregion
 
    #region Scoring
    void SetHighScore()
    {
        switch (Application.loadedLevel)
        {
            case 1:
                PlayerPrefs.SetInt("HS_1", CurrentScore);
                PlayerPrefs.SetInt("M_1", _Medal);
                break;
            case 2:
                PlayerPrefs.SetInt("HS_2", CurrentScore);
                PlayerPrefs.SetInt("M_2", _Medal);
                break;
            case 3:
                PlayerPrefs.SetInt("HS_3", CurrentScore);
                PlayerPrefs.SetInt("M_3", _Medal);
                break;
            case 4:
                PlayerPrefs.SetInt("HS_4", CurrentScore);
                PlayerPrefs.SetInt("M_4", _Medal);
                break;
            case 5:
                PlayerPrefs.SetInt("HS_5", CurrentScore);
                PlayerPrefs.SetInt("M_5", _Medal);
                break;
            case 6:
                PlayerPrefs.SetInt("HS_6", CurrentScore);
                PlayerPrefs.SetInt("M_6", _Medal);
                break;
            case 7:
                PlayerPrefs.SetInt("HS_7", CurrentScore);
                PlayerPrefs.SetInt("M_7", _Medal);
                break;
            case 8:
                PlayerPrefs.SetInt("HS_8", CurrentScore);
                PlayerPrefs.SetInt("M_8", _Medal);
                break;
            case 9:
                PlayerPrefs.SetInt("HS_9", CurrentScore);
                PlayerPrefs.SetInt("M_9", _Medal);
                break;
            case 10:
                PlayerPrefs.SetInt("HS_10", CurrentScore);
                PlayerPrefs.SetInt("M_10", _Medal);
                break;
        }
    }

    void MedalOnly()
    {
        switch (Application.loadedLevel)
        {
            case 1:
                PlayerPrefs.SetInt("M_1", _Medal);
                break;
            case 2:
                PlayerPrefs.SetInt("M_2", _Medal);
                break;
            case 3:
                PlayerPrefs.SetInt("M_3", _Medal);
                break;
            case 4:
                PlayerPrefs.SetInt("M_4", _Medal);
                break;
            case 5:
                PlayerPrefs.SetInt("M_5", _Medal);
                break;
            case 6:
                PlayerPrefs.SetInt("M_6", _Medal);
                break;
            case 7:
                PlayerPrefs.SetInt("M_7", _Medal);
                break;
            case 8:
                PlayerPrefs.SetInt("M_8", _Medal);
                break;
            case 9:
                PlayerPrefs.SetInt("M_9", _Medal);
                break;
            case 10:
                PlayerPrefs.SetInt("M_10", _Medal);
                break;
        }
    }

    void SetMedal()
    {
        if (_AllowMedal == true )
        {
            // DERTERMINE MEDALS
            if (CurrentScore < 300000)
            {
                // No Medals
                _Medal = 0;
            }

            if (CurrentScore >= 300000 && CurrentScore <= 600000)
            {
                // Bronze medal
                _Bronze.SetActive(true);
                _Medal = 1;
            }

            if (CurrentScore >= 600001 && CurrentScore <= 999999)
            {
                // Silver medal
                _Silver.SetActive(true);
                _Medal = 2;
            }

            if (CurrentScore >= 1000000)
            {
                // Gold medal
                _Gold.SetActive(true);
                _Medal = 3;
            }
        }
        else
        {
            _Medal = 0;
        }
        
    }

    void Scoring()
    {
        _CurrentHighScore = Menu_HUD.CurrentLevelHighScore;  // Get previous high score

        SetMedal();  // determine whether or not a medal was awarded

        // check for info to save : if current high score is greater than zero
        if (_CurrentHighScore > 0)
        {
            // check if current score is greater than high score
            if (CurrentScore > _CurrentHighScore)
            {
                SetHighScore();  
            }
            if (CurrentScore <= _CurrentHighScore)
            {
                //Debug.Log("MEDAL ONLY");
                MedalOnly();
            }
        }
        else
        {
            SetHighScore();  // saves info on default scoring (i.e., the level has never been played so high score is 0)
        }

        PlayerPrefs.Save();
        //Debug.Log("GAME INFO CHECK SCORE");
        // Check Against Global High Scores
        Menu_HUD.DataConnector.CheckScores();        
    }
    #endregion

    #region Pause Functions
    public void DoPause()
    {
        // called from PlayerController
        StartCoroutine(PauseGame());
    }
    public void UnDoPause()
    {
        // called from PlayerController
        StartCoroutine(ResumeGame());
    }

    private IEnumerator PauseGame()
    {
        // Pause function Time.timeScale cannot be set to zero, else this enumerator will not complete.
        Debug.Log("Paused");
        _GameState = 0;
        StatusScreen = true;
        MainRoot.SetActive(false);
        PauseRoot.SetActive(true);
        Time.timeScale = 0.0001f;  // pause

        yield return new WaitForSeconds(0.0001f);
        Paused = true;
    }

    private IEnumerator ResumeGame()
    {
        Debug.Log("Resume");
        _GameState = 1;
        StatusScreen = false;
        MainRoot.SetActive(true);
        PauseRoot.SetActive(false);
        Time.timeScale = 1;  // resume

        yield return new WaitForSeconds(0.1f);
        Paused = false;
    }
    #endregion

    #region Set Default Properties
    void SetDefaultProperties_Player()
    {
        Vector3 PlayerStart = new Vector3(0, -1400, 0);  // position to start the player
        PlayerControllerPrefab = Instantiate(PlayerControllerPrefab, PlayerStart, Quaternion.Euler(180,0,0)) as GameObject;  //instantiate Player
    }

    void SetDefaultProperties_DoubleFireRate()
    {
        float xRandom = Random.Range(35f, -35f);
        Vector3 PickupStart = new Vector3(xRandom, TopBlockPosition, 0);
        FireRatePickup = Instantiate(FireRatePickup, PickupStart, Quaternion.identity) as GameObject;
    }

    void SetDefaultProperties_AddBoost()
    {
        float xRandom = Random.Range(35f, -35f);
        Vector3 PickupStart = new Vector3(xRandom, TopBlockPosition, 0);
        BoostPickup = Instantiate(BoostPickup, PickupStart, Quaternion.identity) as GameObject;
    }

    void SetDefaultRespawn_Attack()
    {
        float xRandom = Random.Range(35f, -35f);
        Vector3 PickupStart = new Vector3(xRandom, TopBlockPosition, 0);

        if (!FireRatePickup.activeInHierarchy)
        {
            FireRatePickup.SetActive(true);
            FireRatePickup.transform.position = PickupStart;
        }
    }

    void SetDefaultRespawn_Boost()
    {
        float xRandom = Random.Range(35f, -35f);
        Vector3 PickupStart = new Vector3(xRandom, TopBlockPosition, 0);

        if (!BoostPickup.activeInHierarchy)
        {
            BoostPickup.SetActive(true);
            BoostPickup.transform.position = PickupStart;
        }
    }    
    #endregion

    void Update ()
    { 
        TopBlockPosition = TopBlock.transform.position.y;  // obtain blocking volume position
        CurrentScore = EndlessPlayerController.Score;  // update score
        
        #region Boss Spawn Conditions
        if (!EnemySystem._Gauntlet)
        {
            if (CurrentScore >= 300000 && !ReadyBoss)
            {
                StartCoroutine(SpawnBoss());
                ReadyBoss = true;
            }
        }
        #endregion

        #region Win Condition
        // IF IN ANY LEVEL OTHER THAN 10
        if (_IsBossDefeated == true && !EnemySystem._Gauntlet)
        {
            Debug.Log("Win Condition");
            StartCoroutine(WinCycle());
            _IsBossDefeated = false;
        }

        // IF IN THE LEVEL 10 GAUNTLET
        if (_IsBossDefeated == true && EnemySystem._Gauntlet == true)
        {
            BossHealthBar.SetActive(false);
            Debug.Log("Gauntlet Respawn");
            StartCoroutine(EnemySystem.Gauntlet());
            _IsBossDefeated = false;
        }
        #endregion

        #region Lose Condition
        if (EndlessPlayerController.Lives == 0 && _GameState != 2)
        {
            StartCoroutine(FailCycle());
        }
        #endregion

        #region Pause Conditions
        //if (Paused == false && StatusScreen == false)
        //{
        //    StartCoroutine(PauseGame());
        //}
        //if (Paused == true && StatusScreen == true)
        //{
        //    StartCoroutine(ResumeGame());
        //}
        #endregion
    }
}

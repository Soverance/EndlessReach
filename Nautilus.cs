// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XInputDotNetPure;

public class Nautilus : MonoBehaviour
{
    // XInput stuff
    private PlayerIndex playerIndex;
    private GamePadState state;

    public GameObject Player_UICam;
    public GameObject Player_OVRPC;
    private CharacterController OVRCharacter;
    private OVRGamepadController OVRGamepad;
    private OVRPlayerController OVRPlayer;
    public Transform _MainMenuLoc;
    public Transform _StartLoc;
    public Transform _CryoLoc;
    public GameObject _CryoEffect;
    public GameObject _CryoGlass;
    public GameObject _AurgusDrone;
    //public GameObject _AurgusEngine;
    public GameObject _AurgusBoost;
    public GameObject _DroneDestination;
    

    #region UI Objects
    // The following objects are for UI specific use
    public GameObject _MainMenuRoot;
    public GameObject _GalaxyMapRoot;
    private bool _GalaxyMapIsOpen = false;
    private bool _IsLoading = false;
    private bool _OnMenu = false;
    public GameObject _FX_MapActive;
    public GameObject _FX_MapReady;
    public GameObject _FX_MapBlackHole;
    public GameObject _FX_StagingArea;
    public GameObject _FX_Loading;
    public GameObject _MainPanel;
    public GameObject _CreditsPanel;
    public GameObject _CreditsBack;
    private bool LoadingUnlocked;
    private int LevelToLoad = 0;

    public UISprite Status_LevelName;
    public UILabel Status_LevelNumber;
    public UILabel Status_HighScore;
    public UISprite Status_Medal;

    public GameObject L1_Particle;
    public GameObject L2_Particle;
    public GameObject L3_Particle;
    public GameObject L4_Particle;
    public GameObject L5_Particle;
    public GameObject L6_Particle;
    public GameObject L7_Particle;
    public GameObject L8_Particle;
    public GameObject L9_Particle;
    public GameObject L10_Particle;

    #endregion

    #region Lock Bools

    // LEVEL UNLOCK BOOLS
    private bool L_1UNLOCK = true;  // true as Level 1 should always be unlocked
    private bool L_2UNLOCK = false;
    private bool L_3UNLOCK = false;
    private bool L_4UNLOCK = false;
    private bool L_5UNLOCK = false;
    private bool L_6UNLOCK = false;
    private bool L_7UNLOCK = false;
    private bool L_8UNLOCK = false;
    private bool L_9UNLOCK = false;
    private bool L_10UNLOCK = false;

    #endregion

    private GameObject[] _GalaxyMapEffects = new GameObject[10];
    private int[] _Medals = new int[10];
    private string[] _Scores = new string[10];
    private Queue<bool> _Unlocks = new Queue<bool>();
    private Queue<bool> _ParsedUnlocks = new Queue<bool>();
    private bool[] _UnlockLevel = new bool[10];

    void Awake()
    {
        iTween.CameraFadeAdd(); // add a camera fade texture
        
        // TURN OFF OVR CHARACTER STUFF
        OVRCharacter = Player_OVRPC.GetComponent<CharacterController>();        
        OVRGamepad = Player_OVRPC.GetComponent<OVRGamepadController>();        
        OVRPlayer = Player_OVRPC.GetComponent<OVRPlayerController>();
        OVRCharacter.enabled = false;
        OVRGamepad.enabled = false;
        OVRPlayer.enabled = false;
    }
    void Start()
    {
        LoadingUnlocked = false;
        GetHighScores(); // gets high scores from player prefs
        GetMedals();  // gets medal info from player prefs
        SetLocks();  // sets medals for level select menu

        if (EndlessPersistance._CryoCount == 0)
        {
            // If the player has just initialized the game
            MasterAudio.StartPlaylist("Menu Music");
            Player_OVRPC.transform.position = _MainMenuLoc.position;  // move player to the main menu 
            iTween.CameraFadeFrom(1, 1);
            _OnMenu = true;
        }
        else
        {
            _MainMenuRoot.SetActive(false);
            Player_OVRPC.transform.position = _CryoLoc.position;  // move player into the Nautilus
            Player_OVRPC.transform.rotation = Quaternion.Euler(0, -90, 0);  // correct rotation
            StartCoroutine("InitCryoChamber");
        }
    }

    // Play Button
    void Play()
    {
        StartCoroutine(InitNautilus());
    }

    // Exit Button
    void Exit()
    {
        Debug.Log("Exiting application");
        Application.Quit();
    }

    IEnumerator InitMenu()
    {
        StopCoroutine("InitCryoChamber");
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
        OVRCharacter.enabled = false;
        OVRGamepad.enabled = false;
        OVRPlayer.enabled = false;
        MasterAudio.StopPlaylist("Nautilus_Atmo");
        StartCoroutine(EndlessPersistance.FadeControl(1));
        _MainMenuRoot.SetActive(true);
        yield return new WaitForSeconds(1f);
        Player_OVRPC.transform.position = _MainMenuLoc.position;  // move player to the main menu
        StartCoroutine(EndlessPersistance.FadeControl(0));
        MasterAudio.StartPlaylist("Menu Music");
        
        yield return new WaitForSeconds(3f);
    }

    IEnumerator InitNautilus()
    {
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(EndlessPersistance.FadeControl(1));
        _MainMenuRoot.SetActive(false);
        MasterAudio.StopPlaylist("Menu Music");
        yield return new WaitForSeconds(3f);
        Player_OVRPC.transform.position = _CryoLoc.position;  // move player into the Nautilus    
        Player_OVRPC.transform.rotation = Quaternion.Euler(0,-90,0);  // reset rotation
        StartCoroutine(InitCryoChamber());
	}

    IEnumerator InitCryoChamber()
    {
        MasterAudio.StartPlaylist("Nautilus Atmo");
        MasterAudio.PlaylistMasterVolume = 0.3f;
        _OnMenu = false;

        if (EndlessPersistance._CryoCount == 0)
        {
            // If _CryoCount = 0, do the extended intro
            StartCoroutine(EndlessPersistance.FadeControl(0));
            yield return new WaitForSeconds(10f);
            MasterAudio.PlaySound("Ethereal_Welcome01", 1);
            yield return new WaitForSeconds(5f);
            MasterAudio.PlaySound("Ethereal_Intro01", 1);
            yield return new WaitForSeconds(5f);
        }
        else
        {
            // If _CryoCount is any other number, do the short intro
            StartCoroutine(EndlessPersistance.FadeControl(0));
            yield return new WaitForSeconds(3f);
            MasterAudio.PlaySound("Ethereal_Welcome02", 1);
            yield return new WaitForSeconds(3f);
        }

        MasterAudio.PlaySoundAndForget("cryo_release", 1);
        StartCoroutine(EndlessPersistance.FadeControl(1));
        yield return new WaitForSeconds(2f);
        Player_OVRPC.transform.position = _StartLoc.position;
        Player_OVRPC.transform.rotation = Quaternion.Euler(0, 0, 0);  // reset rotation
        _CryoEffect.SetActive(false);  // turn off cryo chamber
        StartCoroutine(EndlessPersistance.FadeControl(0));
        yield return new WaitForSeconds(3f);
        OVRCharacter.enabled = true;
        OVRGamepad.enabled = true;
        OVRPlayer.enabled = true;

        if (EndlessPersistance._CryoCount == 0)
        {
            MasterAudio.PlaySound("Ethereal_Intro02", 1);
            yield return new WaitForSeconds(3.8f);
            MasterAudio.PlaySound("Ethereal_Intro03", 1);
            yield return new WaitForSeconds(5f);
            MasterAudio.PlaySound("Ethereal_Background01", 1);
            yield return new WaitForSeconds(20f);

        }
        MasterAudio.PlaySound("Ethereal_Prompt01", 1);
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
    }

    #region Scoring and Medals
    void GetHighScores()
    {
        // LEVEL ONE
        if (PlayerPrefs.HasKey("HS_1"))
        {
            _Scores[0] = PlayerPrefs.GetInt("HS_1").ToString();
        }
        else 
        {
            _Scores[0] = "NONE";
        }
        // LEVEL TWO
        if (PlayerPrefs.HasKey("HS_2"))
        {
            _Scores[1] = PlayerPrefs.GetInt("HS_2").ToString();
        }
        else
        {
            _Scores[1] = "NONE";
        }
        // LEVEL THREE
        if (PlayerPrefs.HasKey("HS_3"))
        {
            _Scores[2] = PlayerPrefs.GetInt("HS_3").ToString();
        }
        else
        {
            _Scores[2] = "NONE";
        }
        // LEVEL FOUR
        if (PlayerPrefs.HasKey("HS_4"))
        {
            _Scores[3] = PlayerPrefs.GetInt("HS_4").ToString();
        }
        else
        {
            _Scores[3] = "NONE";
        }
        // LEVEL FIVE
        if (PlayerPrefs.HasKey("HS_5"))
        {
            _Scores[4] = PlayerPrefs.GetInt("HS_5").ToString();
        }
        else
        {
            _Scores[4] = "NONE";
        }
        // LEVEL SIX
        if (PlayerPrefs.HasKey("HS_6"))
        {
            _Scores[5] = PlayerPrefs.GetInt("HS_6").ToString();
        }
        else
        {
            _Scores[5] = "NONE";
        }
        // LEVEL SEVEN
        if (PlayerPrefs.HasKey("HS_7"))
        {
            _Scores[6] = PlayerPrefs.GetInt("HS_7").ToString();
        }
        else
        {
            _Scores[6] = "NONE";
        }
        // LEVEL EIGHT
        if (PlayerPrefs.HasKey("HS_8"))
        {
            _Scores[7] = PlayerPrefs.GetInt("HS_8").ToString();
        }
        else
        {
            _Scores[7] = "NONE";
        }
        // LEVEL NINE
        if (PlayerPrefs.HasKey("HS_9"))
        {
            _Scores[8] = PlayerPrefs.GetInt("HS_9").ToString();
        }
        else
        {
            _Scores[8] = "NONE";
        }
        // LEVEL TEN
        if (PlayerPrefs.HasKey("HS_10"))
        {
            _Scores[9] = PlayerPrefs.GetInt("HS_10").ToString();
        }
        else
        {
            _Scores[9] = "NONE";
        }
    }

    void GetMedals()
    {
        foreach (int Medal in _Medals)
        {
            // Default all medals to ZERO
            _Medals[Medal] = 0;
        }

        // THEN GET ALL ACTUAL MEDALS FROM PLAYER PREFS
        // LEVEL ONE
        if (PlayerPrefs.HasKey("M_1"))
        {
            _Medals[0] = PlayerPrefs.GetInt("M_1");            
        }
        // LEVEL TWO
        if (PlayerPrefs.HasKey("M_2"))
        {
            _Medals[1] = PlayerPrefs.GetInt("M_2");
        }
        // LEVEL THREE
        if (PlayerPrefs.HasKey("M_3"))
        {
            _Medals[2] = PlayerPrefs.GetInt("M_3");
        }
        // LEVEL FOUR
        if (PlayerPrefs.HasKey("M_4"))
        {
            _Medals[3] = 0;  // MODDED FOR DEMO VERSION SO AS NOT TO ALLOW THE LEVEL BELOW TO UNLOCK
            //_Medals[3] = PlayerPrefs.GetInt("M_4");
        }
        // LEVEL FIVE
        if (PlayerPrefs.HasKey("M_5"))
        {
            _Medals[4] = PlayerPrefs.GetInt("M_5");
        }
        // LEVEL SIX
        if (PlayerPrefs.HasKey("M_6"))
        {
            _Medals[5] = PlayerPrefs.GetInt("M_6");
        }
        // LEVEL SEVEN
        if (PlayerPrefs.HasKey("M_7"))
        {
            _Medals[6] = PlayerPrefs.GetInt("M_7");
        }
        // LEVEL EIGHT
        if (PlayerPrefs.HasKey("M_8"))
        {
            _Medals[7] = PlayerPrefs.GetInt("M_8");
        }
        // LEVEL NINE
        if (PlayerPrefs.HasKey("M_9"))
        {
            _Medals[8] = PlayerPrefs.GetInt("M_9");
        }
        // LEVEL TEN
        if (PlayerPrefs.HasKey("M_10"))
        {
            _Medals[9] = PlayerPrefs.GetInt("M_10");
        }

        //// PRINT ALL MEDAL NUMBERS TO DEBUG CONSOLE
        //foreach (int Medal in _Medals)
        //{
        //    Debug.Log(Medal.ToString());
        //}
    }

    void SetLocks()
    {
        // QUEUE STARTING UNLOCK BOOLS
        _ParsedUnlocks.Enqueue(L_1UNLOCK);  // Level One is always unlocked, and therefore is already true and added to the parsed unlock queue
        _Unlocks.Enqueue(L_2UNLOCK);
        _Unlocks.Enqueue(L_3UNLOCK);
        _Unlocks.Enqueue(L_4UNLOCK);
        _Unlocks.Enqueue(L_5UNLOCK);
        _Unlocks.Enqueue(L_6UNLOCK);
        _Unlocks.Enqueue(L_7UNLOCK);
        _Unlocks.Enqueue(L_8UNLOCK);
        _Unlocks.Enqueue(L_9UNLOCK);
        _Unlocks.Enqueue(L_10UNLOCK);

        // create a new medal array, excluding the medal for level 10, because level 10 does not unlock anything
        int[] _TruncatedMedalArray = _Medals.Take(_Medals.Length - 1).ToArray();

        foreach (int Medal in _TruncatedMedalArray)
        {
            // DEQUEUE Unlock Bool
            bool Unlocked = _Unlocks.Dequeue();

            // PARSE DEFAULTS FOR MEDAL AND UNLOCK INFO ON EACH LEVEL
            switch (Medal)
            {
                case 1:
                case 2:
                case 3:
                    Unlocked = true;
                    break;
                default:
                    Unlocked = false;
                    break;
            }
            _ParsedUnlocks.Enqueue(Unlocked);  // add the unlock bool to it's new parsed array
        }

        // Add locking bools to their parsed array
        // ADD THE LAST NINE UNLOCK BOOLS
        for (int i = 0; i < 9; i++)
        {
            bool IsLocked = _ParsedUnlocks.Dequeue();  // dequeue first parsed unlock bool
            _UnlockLevel[i] = IsLocked;  // Add to array
        }
    }
    #endregion

    #region Galaxy Map Statuses

    void KillEffect()
    {
        GameObject EffectToKill = GameObject.FindGameObjectWithTag("GalaxyMap_Effect");
        EffectToKill.SetActive(false);
    }

    void Status_L1()
    {
        L1_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_1";
        Status_HighScore.text = _Scores[0].ToString();
        Status_LevelNumber.text = "Level One";
        
        switch(_Medals[0])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L2()
    {
        L2_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_2";
        Status_HighScore.text = _Scores[1].ToString();
        Status_LevelNumber.text = "Level Two";

        switch (_Medals[1])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L3()
    {
        L3_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_3";
        Status_HighScore.text = _Scores[2].ToString();
        Status_LevelNumber.text = "Level Three";

        switch (_Medals[2])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L4()
    {
        L4_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_4";
        Status_HighScore.text = _Scores[3].ToString();
        Status_LevelNumber.text = "Level Four";

        switch (_Medals[3])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L5()
    {
        L5_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_5";
        Status_HighScore.text = _Scores[4].ToString();
        Status_LevelNumber.text = "Level Five";

        switch (_Medals[4])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L6()
    {
        L6_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_6";
        Status_HighScore.text = _Scores[5].ToString();
        Status_LevelNumber.text = "Level Six";

        switch (_Medals[5])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L7()
    {
        L7_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_7";
        Status_HighScore.text = _Scores[6].ToString();
        Status_LevelNumber.text = "Level Seven";

        switch (_Medals[6])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L8()
    {
        L8_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_8";
        Status_HighScore.text = _Scores[7].ToString();
        Status_LevelNumber.text = "Level Eight";

        switch (_Medals[7])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L9()
    {
        L9_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_9";
        Status_HighScore.text = _Scores[8].ToString();
        Status_LevelNumber.text = "Level Nine";

        switch (_Medals[8])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    void Status_L10()
    {
        L10_Particle.SetActive(true);
        Status_LevelName.spriteName = "name_10";
        Status_HighScore.text = _Scores[9].ToString();
        Status_LevelNumber.text = "Level Ten";

        switch (_Medals[9])
        {
            case 0:
                Status_Medal.spriteName = "medal_null";
                break;
            case 1:
                Status_Medal.spriteName = "medal_bronze";
                break;
            case 2:
                Status_Medal.spriteName = "medal_silver";
                break;
            case 3:
                Status_Medal.spriteName = "medal_gold";
                break;
        }
    }

    #endregion

    IEnumerator OpenGalaxyMap()
    {
        _FX_MapReady.SetActive(false);
        MasterAudio.PlaySoundAndForget("Nautilus_GalaxyMap", 1);
        _FX_MapActive.SetActive(true);
        OVRCharacter.enabled = false;
        OVRGamepad.enabled = false;
        OVRPlayer.enabled = false;
        yield return new WaitForSeconds(1f);
        _GalaxyMapRoot.SetActive(true);
    }

    IEnumerator CloseGalaxyMap()
    {
        KillEffect();
        MasterAudio.PlaySoundAndForget("Nautilus_GalaxyMap", 1);
        Instantiate(_FX_MapBlackHole, _FX_MapReady.transform.position, Quaternion.identity);
        _FX_MapActive.SetActive(false);
        yield return new WaitForSeconds(1f);
        _GalaxyMapRoot.SetActive(false);
        OVRCharacter.enabled = true;
        OVRGamepad.enabled = true;
        OVRPlayer.enabled = true;
        GalaxyMap._InRange = false;
    }

    IEnumerator PrepareForLoading()
    {
        if (LoadingUnlocked)
        {
            _FX_StagingArea.SetActive(true);
            MasterAudio.PlaySoundAndForget("Nautilus_GalaxyMap", 1);
            yield return new WaitForSeconds(4f);
            MasterAudio.PlaySound("Ethereal_Prompt03", 1);
            yield return new WaitForSeconds(2f);
            MasterAudio.PlaySound("Ethereal_Prompt04", 1);
            OVRCharacter.enabled = true;
            OVRGamepad.enabled = true;
            OVRPlayer.enabled = true;
        }
    }

    IEnumerator LoadLevel()
    {
        _FX_Loading.SetActive(true);
        _FX_StagingArea.SetActive(false);
        iTween.RotateAdd(_AurgusDrone, new Vector3(0, 0, 100), 8);  // rotate out
        iTween.MoveAdd(_AurgusDrone, new Vector3(40, -35, 0), 8);  // move up slightly
        float OverloadEndTime = Time.realtimeSinceStartup + 4f;
        
        while (Time.realtimeSinceStartup < OverloadEndTime)
        {
            if (Time.timeScale > 0.1f)
            {
                Time.timeScale -= 0.01f;
            }
            yield return null;
        }
        Instantiate(_AurgusBoost, _AurgusDrone.transform.position, Quaternion.identity);
        iTween.MoveTo(_AurgusDrone, iTween.Hash("position", _DroneDestination.transform.position, "time", 4, "easetype", iTween.EaseType.linear, "ignoretimescale", true));
        StartCoroutine(EndlessPersistance.FadeControl(1));
        yield return new WaitForSeconds(1f);

        // DESTROY ALL ITWEENS
        iTween.Destructor();

        // LOAD A LEVEL
        switch (LevelToLoad)
        {
            case 1:
                Application.LoadLevel(1);
                break;
            case 2:
                Application.LoadLevel(2);
                break;
            case 3:
                Application.LoadLevel(3);
                break;
            case 4:
                Application.LoadLevel(4);
                break;
            case 5:
                //Application.LoadLevel(5);
                break;
            case 6:
                //Application.LoadLevel(6);
                break;
            case 7:
                //Application.LoadLevel(7);
                break;
            case 8:
                //Application.LoadLevel(8);
                break;
            case 9:
                //Application.LoadLevel(9);
                break;
            case 10:
                //Application.LoadLevel(10);
                break;
            default:
                //_LoadingBack.SendMessage("OnClick");  // return to main menu if no level to load
                break;
        }        
    }

    #region Level Defaults
    // These functions are called by NGUI button presses in game, and set the default properties for which level to load
    void StartLevelOne()
    {
        StopCoroutine("InitCryoChamber");
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
        switch (_UnlockLevel[0])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                MasterAudio.PlaySoundAndForget("Ethereal_Locked", 1);
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 1;
                MasterAudio.PlaySoundAndForget("Ethereal_Level01", 1);
                StartCoroutine(CloseGalaxyMap());
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelTwo()
    {
        StopCoroutine("InitCryoChamber");
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
        switch (_UnlockLevel[1])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                MasterAudio.PlaySoundAndForget("Ethereal_Locked", 1);
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 2;
                MasterAudio.PlaySoundAndForget("Ethereal_Level02", 1);
                StartCoroutine(CloseGalaxyMap());
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelThree()
    {
        StopCoroutine("InitCryoChamber");
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
        switch (_UnlockLevel[2])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                MasterAudio.PlaySoundAndForget("Ethereal_Locked", 1);
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 3;
                MasterAudio.PlaySoundAndForget("Ethereal_Level03", 1);
                StartCoroutine(CloseGalaxyMap());
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelFour()
    {
        StopCoroutine("InitCryoChamber");
        EndlessPersistance._CryoCount++;  // add one to the cryo counter
        switch (_UnlockLevel[3])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                MasterAudio.PlaySoundAndForget("Ethereal_Locked", 1);
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 4;
                MasterAudio.PlaySoundAndForget("Ethereal_Level04", 1);
                StartCoroutine(CloseGalaxyMap());
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelFive()
    {
        switch (_UnlockLevel[4])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 5;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelSix()
    {
        switch (_UnlockLevel[5])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 6;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelSeven()
    {
        switch (_UnlockLevel[6])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 7;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelEight()
    {
        switch (_UnlockLevel[7])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 8;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelNine()
    {
        switch (_UnlockLevel[8])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 9;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    void StartLevelTen()
    {
        switch (_UnlockLevel[9])
        {
            case false:
                LoadingUnlocked = false;
                LevelToLoad = 0;
                break;
            default:
                LoadingUnlocked = true;
                LevelToLoad = 10;
                break;
        }
        StartCoroutine(PrepareForLoading());
    }
    #endregion
    
    void Update()
    {
        playerIndex = XInputState.playerIndex;
        state = XInputState.state;

        if (_MainPanel.activeInHierarchy == false && _CreditsPanel.activeInHierarchy == true && state.Buttons.B == ButtonState.Pressed)
        {
            _CreditsBack.SendMessage("OnClick");
        }

        //if (state.Buttons.Start == ButtonState.Pressed && _OnMenu == false)
        //{
        //    StartCoroutine(InitMenu());
        //    _OnMenu = true;
        //}

        // GALAXY MAP IS IN RANGE WHEN PLAYER STANDS NEAR THE PEDESTAL
        if (GalaxyMap._InRange == true && _GalaxyMapIsOpen == false)
        {
            _FX_MapReady.SetActive(true);  // spawn attention orb

            //  PRESS "A" TO ACTIVATE
            if (state.Buttons.A == ButtonState.Pressed)
            {
                StartCoroutine(OpenGalaxyMap());
                _GalaxyMapIsOpen = true;
            }
        }

        // GALAXY MAP IS OPEN
        if (GalaxyMap._InRange == true && _GalaxyMapIsOpen == true)
        {
            //  PRESS "B" TO CLOSE
            if (state.Buttons.B == ButtonState.Pressed)
            {
                StartCoroutine(CloseGalaxyMap());
                _GalaxyMapIsOpen = false;
            }
        }

        if (GalaxyMap._Load == true && _IsLoading == false)
        {
            StartCoroutine(LoadLevel());
            _IsLoading = true;
        }
    }
}

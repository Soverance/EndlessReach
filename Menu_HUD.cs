// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Menu_HUD : MonoBehaviour {

    public UISlider _BoostMeter;
    public UISlider _BossHealthBar;
    public UILabel _Score;
    public UILabel _FPSVariable;
    public UILabel _PowerupRate;
    //private float _InitialRate;
    //private float _NewRate;
    public UILabel _StatusScore;
    public UILabel _UniversalScore;
    public UILabel _CurrentHighScore;
    public static int CurrentLevelHighScore;
    public static ERDataConnector DataConnector;

    void Start ()
    {
        GetHighScore();
        StartCoroutine(FadeIn());
        DataConnector = GameObject.Find("ERDataConnector").GetComponent<ERDataConnector>();
        
    }

    //  Exit
    IEnumerator ExitToMenu()
    {
        Time.timeScale = 1;  // unpause
        Debug.Log("Returning To Main Menu");
        iTween.Destructor();
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1.0f);
        Application.LoadLevel(11);
    }

    IEnumerator Retry_Level()
    {
        Time.timeScale = 1;  // unpause
        iTween.Destructor();
        PlayerPrefs.Save();
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1.0f);
        switch (Application.loadedLevel)
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
                Application.LoadLevel(5);
                break;
            case 6:
                Application.LoadLevel(6);
                break;
            case 7:
                Application.LoadLevel(7);
                break;
            case 8:
                Application.LoadLevel(8);
                break;
            case 9:
                Application.LoadLevel(9);
                break;
            case 10:
                Application.LoadLevel(10);
                break;
        }
    }

    IEnumerator FadeIn()
    {
        iTween.CameraFadeAdd();
        iTween.CameraFadeFrom(1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        //_InitialRate = EndlessPlayerController._InitialRate;
        //_NewRate = _InitialRate;
    }

    IEnumerator FadeOut()
    {
        iTween.CameraFadeAdd();
        iTween.CameraFadeTo(1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
    }

    // REFRESH LEADERBOARD
    IEnumerator RefreshLeaderboard()
    {
        yield return new WaitForSeconds(0.1f);
        DataConnector.Connect();
    }

    void GetHighScore()
    {
        switch (Application.loadedLevel)
        {
            case 1:
                if (PlayerPrefs.HasKey("HS_1"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_1");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 2:
                if (PlayerPrefs.HasKey("HS_2"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_2");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 3:
                if (PlayerPrefs.HasKey("HS_3"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_3");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 4:
                if (PlayerPrefs.HasKey("HS_4"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_4");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 5:
                if (PlayerPrefs.HasKey("HS_5"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_5");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 6:
                if (PlayerPrefs.HasKey("HS_6"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_6");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 7:
                if (PlayerPrefs.HasKey("HS_7"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_7");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 8:
                if (PlayerPrefs.HasKey("HS_8"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_8");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 9:
                if (PlayerPrefs.HasKey("HS_9"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_9");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
            case 10:
                if (PlayerPrefs.HasKey("HS_10"))
                {
                    CurrentLevelHighScore = PlayerPrefs.GetInt("HS_10");  // get high score
                    _CurrentHighScore.text = CurrentLevelHighScore.ToString();
                }
                else
                {
                    CurrentLevelHighScore = 0;
                }
                break;
        }
    }

    // called to update boss health bar, passed a health percentage
    public void UpdateBossHealth(float _HealthPercentage)
    {
        _BossHealthBar.value = _HealthPercentage;
    }

    // UPDATE BOOST METER
    public void UpdateBoostMeter(float _BoostPercentage)
    {
        _BoostMeter.value = _BoostPercentage;
    }

    void Update()
    {
        // FPS COUNTER
        if (Time.timeScale == 1)
        {
            float count = (1 / Time.deltaTime);
            _FPSVariable.text = Mathf.Round(count).ToString();
        }
        else
        {
            _FPSVariable.text = "Pause";
        }

        // PICKUP RATE COUNTER
        float Ptext = Mathf.Round(EndlessPlayerController._NewRate * 10) / 10;  // set to HUD
        _PowerupRate.text = Ptext.ToString();
    }

    void FixedUpdate()
    {
        int ScoreVariable = EndlessPlayerController.Score;
        //float LivesVariable = Time.timeSinceLevelLoad;

        _Score.text = ScoreVariable.ToString();
        //_Time.text = LivesVariable.ToString();
        _StatusScore.text = ScoreVariable.ToString();
        _UniversalScore.text = ScoreVariable.ToString();

        

    }
}

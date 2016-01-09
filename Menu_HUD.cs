// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Menu_HUD : MonoBehaviour {

	private const int MinIndexSceneLevelPlayable = 1;
	private const int MaxIndexSceneLevelPlayable = 10;

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

        if (IsPlayableSceneLevel(Application.loadedLevel)) 
        {
        	Application.LoadLevel(Application.loadedLevel);
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

    bool IsPlayableSceneLevel(int levelIndex)
    {
    	return (levelIndex >= MinIndexSceneLevelPlayable && levelIndex <= MaxIndexSceneLevelPlayable);
    }

    void GetHighScore()
    {
    	if (IsPlayableSceneLevel(Application.loadedLevel)) 
    	{
    		var levelKey = string.Format("HS_{0}", Application.loadedLevel);
    		if (PlayerPrefs.HasKey(levelKey))
    		{
    			CurrentLevelHighScore = PlayerPrefs.GetInt(levelKey);  // get high score
                _CurrentHighScore.text = CurrentLevelHighScore.ToString();
    		}
    		else
            {
                CurrentLevelHighScore = 0;
            }
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

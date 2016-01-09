// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

    public GameObject _LogoEffect;
    public GameObject _PromptEffect;
    public GameObject _PromptText;
    public bool isReady;

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeIn());
        StartCoroutine(Calibrate());        
	}

    IEnumerator RunEffect()
    {
        _PromptEffect.SetActive(true);
        _PromptText.renderer.enabled = false;
        yield return new WaitForSeconds(1f);
        _LogoEffect.SetActive(true);
        MasterAudio.PlaySoundAndForget("Splash_Icon", 1);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        iTween.CameraFadeAdd();
        iTween.CameraFadeFrom(1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator FadeOut()
    {
        iTween.CameraFadeAdd();
        iTween.CameraFadeTo(1.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(11);
    }

    // RESET THE HMD ORIENTATION
    IEnumerator Calibrate()
    {
        isReady = false;
        yield return new WaitForSeconds(3f);
        OVRManager.display.RecenterPose();  // OVR Reset function for 0.4.3
        _PromptText.SetActive(true);        
        isReady = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.anyKeyDown && isReady == true)
        {
            StartCoroutine(RunEffect());
        }	
	}
}

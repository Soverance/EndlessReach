// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessPersistance : MonoBehaviour {

    public static int _CryoCount;

    void Awake()
    {
        _CryoCount = 0;
    }

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this.gameObject);  // do not destroy this object
	}

    public static IEnumerator FadeControl(int Fade)
    {
        // int Fade = 0  :  Fade In
        // int Fade = 1  :  Fade Out
        iTween.CameraFadeAdd();

        switch (Fade)
        {
            case 0:
                iTween.CameraFadeTo(0f, 1.5f);
                break;
            case 1:
                iTween.CameraFadeTo(1.0f, 1.5f);
                break;
        }
        yield return new WaitForSeconds(2f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

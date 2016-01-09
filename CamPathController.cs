// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class CamPathController : MonoBehaviour {

    public GameObject _CamStart;
    public GameObject _CamEnd;

    private bool _PathPaused;

	void Start() 
    {
        _PathPaused = false;
        iTween.MoveTo(gameObject, iTween.Hash("position", _CamEnd.transform.position, "time", 180, "easetype", iTween.EaseType.linear));
	}

    IEnumerator PauseCamera()
    {
        _PathPaused = true;
        iTween.Pause(gameObject);
        //Debug.Log("Pathing has paused");
        yield return new WaitForSeconds(6f);
        //Debug.Log("Pathing resumed");
        _PathPaused = false;
        iTween.Resume(gameObject);
    }

    void Update()
    {
        if (EndlessPlayerController._Overloading && _PathPaused == false)
        {
            StartCoroutine(PauseCamera());
        }

        if (EndlessPlayerController._IsDestroyed == true && _PathPaused == false)
        {
            StartCoroutine(PauseCamera());
        }

        if (EndlessEnemySystem.BossDying && !_PathPaused)
        {
            StartCoroutine(PauseCamera());
        }
    }
}

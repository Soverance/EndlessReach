// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class VR_CamRaycast : MonoBehaviour {

    Transform _OVRCameraTransform = null;
    private RaycastHit _LookHit;
    public static GameObject _LookTarget;
    float length = 800f;

    public static bool _HasTarget;
    public GameObject _TargetingEffect;
    private GameObject _CurrentTargetEffect;

	// Use this for initialization
	void Start () {
        _OVRCameraTransform = this.transform;
        _HasTarget = false;        
	}

    void AcquireTarget()
    {
        MasterAudio.PlaySoundAndForget("Player_Selection", 1);
        _CurrentTargetEffect = Instantiate(_TargetingEffect, _LookTarget.transform.position, Quaternion.identity) as GameObject;
        _CurrentTargetEffect.transform.parent = _LookTarget.transform;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 _LookRayDirection = _OVRCameraTransform.TransformDirection(Vector3.forward);
        Vector3 _LookRayStart = _OVRCameraTransform.position + _LookRayDirection;
        Debug.DrawRay(_LookRayStart, _LookRayDirection * length, Color.green);
        // CAST RAY AGAINST TARGETING LAYER
        if (Physics.Raycast(_LookRayStart, _LookRayDirection, out _LookHit, length, 1 << 16))
        {
            if (_LookHit.collider.gameObject.tag == "Targetable" && _HasTarget == false && EndlessGameInfo._GameState == 1)
            {
                //Debug.Log("LOOK RAY GO");
                _HasTarget = true;
                _LookTarget = _LookHit.collider.gameObject;
                EndlessPlayerController._LockedOn = true;
                AcquireTarget();
            }            
        }
        else
        {
            // REMOVE TARGETING
            _HasTarget = false;
            EndlessPlayerController._LockedOn = false;
            Destroy(_CurrentTargetEffect);
        }
	}
}

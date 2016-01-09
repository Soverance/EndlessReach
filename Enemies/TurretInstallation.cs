// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class TurretInstallation : MonoBehaviour {

    private int health = 1500;
    private int DamageCount = 0;
    private float _DistanceToPlayer;
    private bool _StillActive = true;
    //private bool StartPulse = true;
    private bool RotateGo = true;
    private GameObject TopBlock;
    private float TopBlockPos;
    public GameObject _TurretSphere;
    public GameObject _TurretBarrel1;
    public GameObject _TurretBarrel2;
    public GameObject _ParticleBeam;
    public GameObject _ParticlePulse;
    public GameObject _ParticleGrenade;
    public GameObject _TurretExplosion;
    public GameObject _TurretHitExplosion;
    //public AnimationClip _Turret02Anim;
    public GameObject _TurretShield;
    private Animation _TurretAnim;

	// Use this for initialization
	void Start () {
        _TurretAnim = this.gameObject.GetComponent<Animation>();
        RotateGo = true;
        TopBlock = GameObject.Find("TopBlock");
	}

    private IEnumerator RunAnim()
    {
        RotateGo = false;
        
        switch (this.gameObject.tag)
        {
            case "Turret_Beam":  // green pendulum
                _TurretAnim.Play();
                break;
            case "Turret_Pulse":  // capital ship
                //_TurretShield.SetActive(true);
                _TurretAnim.Play();
                break;
            case "Turret_Grenade":
                break;
        }
        yield return new WaitForSeconds(_TurretAnim.clip.length);
        RotateGo = true;
    }

    public void MissileConnect()
    {
        
        switch (this.gameObject.tag)
        {
            case "Turret_Beam":  // green pendulum
                StartCoroutine(AddScore());
                break;
            case "Turret_Pulse":  // capital ship
                DamageCount++;  // increment hit count
                EndlessPlayerController.Score += 1500;
                MasterAudio.PlaySoundAndForget("Turret_Explosion01", 1);
                if (DamageCount == 2)
                {
                    StartCoroutine(AddScore());
                }                
                break;
            case "Turret_Grenade":
                break;
        }
    }

    // ADD SCORE
    private IEnumerator AddScore()
    {
        _StillActive = false;
        _TurretAnim.Stop();
  
        switch (this.gameObject.tag)
        {
            case "Turret_Beam":  // green pendulum
                _ParticleBeam.SetActive(false);
                Destroy(_TurretSphere);
                _TurretBarrel1.renderer.enabled = false;
                MasterAudio.PlaySoundAndForget("Turret_Explosion01", 1);
                Instantiate(_TurretExplosion, _TurretSphere.transform.position, _TurretSphere.transform.rotation);
                EndlessPlayerController.Score += 10000;
                break;
            case "Turret_Pulse":  // capital ship
                _ParticleBeam.SetActive(false);
                MasterAudio.PlaySoundAndForget("Turret_Explosion02", 1);
                Instantiate(_TurretExplosion, this.transform.position, Quaternion.identity);
                EndlessPlayerController.Score += 15000;
                StartCoroutine("DestroyCapitalShip");
                break;
            case "Turret_Grenade":
                _TurretBarrel2.renderer.enabled = false;
                _ParticleGrenade.SetActive(false);
                break;
        }
        
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
        yield return new WaitForSeconds(0.4f);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    private IEnumerator DestroyCapitalShip()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.renderer.enabled = false;
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
        yield return new WaitForSeconds(0.4f);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    void FireBeam()
    {
        _ParticleBeam.SetActive(true);
    }
    void CancelBeam()
    {
        _ParticleBeam.SetActive(false);
    }
    // ON TRIGGER
    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player_Laser")
        {
            health = (health - 100);
            Instantiate(_TurretHitExplosion, _TurretSphere.transform.position, _TurretSphere.transform.rotation);
        }

        if (Other.tag == "Purge")
        {
            StartCoroutine(AddScore());
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        TopBlockPos = TopBlock.transform.position.y;  // reset TopBlock position each frame

        // DESTROY TURRET WHEN HEALTH = 0
        if (health <= 0 && _StillActive == true)
        {
            StartCoroutine(AddScore());
        }

         //START FIRING WHEN TOP BLOCK PASSES TURRET INSTALLATION
        if (_StillActive == true)
        {
            if (TopBlockPos > this.transform.position.y && RotateGo == true)
            {
                StartCoroutine("RunAnim");
            }
        }
	}
}

// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class EndlessEnemySystem : MonoBehaviour {

    public static EndlessEnemySystem _EES;
    public static EndlessGameInfo _GameInfo;
    public static Menu_HUD _HUD;
    public static GameObject TopBlock;
    public static bool BossDying;
    public static bool OmegaPop;
    public bool _Gauntlet;
    //private bool _PreparingBoss;
    private int BossNumber;
    private float TopBlockPosition;

    // ENEMY OBJECTS
    public GameObject Fighter;
    public GameObject SideShooter;
    public GameObject Centipede_3Seg;
    public GameObject Centipede_4Seg;
    public GameObject Centipede_5Seg;
    
    // BOSS LIST
    public GameObject Boss1;
    public GameObject Boss2;
    public GameObject Boss3;
    public GameObject Boss4;
    public GameObject Boss5;
    public GameObject Boss6;
    public GameObject Boss7;
    public GameObject Boss8;
    public GameObject Boss9;
    public GameObject Boss10;
    public GameObject CurrentBoss;

    void Awake ()
    {
        _GameInfo = GameObject.Find("VR_HUDCam").GetComponent<EndlessGameInfo>();
        _EES = GameObject.Find("VR_HUDCam").GetComponent<EndlessEnemySystem>();
        _HUD = GameObject.Find("HUD").GetComponent<Menu_HUD>();
        TopBlock = GameObject.Find("TopBlock");
        _Gauntlet = false;
        //_PreparingBoss = false;
        BossDying = false;
        OmegaPop = false;
    }

	void Start () 
    {
        switch (Application.loadedLevel)
        {
            case 1:
                InvokeRepeating("SetDefaultProperties_Fighter", 2.5f, 1);
                BossNumber = 1;
                break;
            case 2:
                InvokeRepeating("SetDefaultProperties_Fighter", 2.5f, 1);
                BossNumber = 2;
                break;
            case 3:
                InvokeRepeating("SetDefaultProperties_SideShooter", 2.5f, 1.5f);
                BossNumber = 3;
                break;
            case 4:
                InvokeRepeating("SetDefaultProperties_SideShooter", 2.5f, 1.5f);
                BossNumber = 4;
                break;
            case 5:
                InvokeRepeating("SetDefaultProperties_Centipede", 2.5f, 2);
                BossNumber = 5;
                break;
            case 6:
                InvokeRepeating("SetDefaultProperties_Centipede", 2.5f, 2);
                BossNumber = 6;
                break;
            case 7:
                InvokeRepeating("SetDefaultProperties_Random", 2.5f, 2);
                BossNumber = 7;
                break;
            case 8:
                InvokeRepeating("SetDefaultProperties_Random", 2.5f, 2);
                BossNumber = 8;
                break;
            case 9:
                InvokeRepeating("SetDefaultProperties_Random", 2.5f, 2);
                BossNumber = 9;
                break;
            case 10:
                _Gauntlet = true;
                BossNumber = 2;
                StartCoroutine(Gauntlet());
                break;
        }
	}

    public void CancelEnemyInvokes()
    {
        CancelInvoke();
    }

    //  ENEMIES
    public void SetDefaultProperties_Fighter()
    {
        float xRandom = Random.Range(80f, -80f);
        Vector3 FighterStart = new Vector3(xRandom, TopBlockPosition + 300, 0);
        Instantiate(Fighter, FighterStart, Quaternion.Euler(90,0,0));        
    }

    public void SetDefaultProperties_SideShooter()
    {
        float xRandom = Random.Range(80f, -80f);
        Vector3 SideShooterStart = new Vector3(xRandom, TopBlockPosition + 300, 0);
        Instantiate(SideShooter, SideShooterStart, Quaternion.Euler(0, 180, -90));
    }

    public void SetDefaultProperties_Centipede()
    {
        int SpawnIndex = Random.Range(3, 6);  // three to five
        float xRandom = Random.Range(80f, -80f);
        Vector3 CentipedeStart = new Vector3(xRandom, TopBlockPosition, 0);

        switch (SpawnIndex)
        {
            case 3:
                Instantiate(Centipede_3Seg, CentipedeStart, Quaternion.identity);
                Debug.Log("3 Seg");
                break;
            case 4:
                Instantiate(Centipede_4Seg, CentipedeStart, Quaternion.identity);
                Debug.Log("4 Seg");
                break;
            case 5:
                Instantiate(Centipede_5Seg, CentipedeStart, Quaternion.identity);
                Debug.Log("5 Seg");
                break;
            default:
                Debug.Log("Segments out of range");
                break;
        }
    }

    public void SetDefaultProperties_Random()
    {
        int SpawnIndex = Random.Range(1, 4);  // random 1 - 3
        switch (SpawnIndex)
        {
            case 1:
                SetDefaultProperties_Fighter();
                break;
            case 2:
                SetDefaultProperties_SideShooter();
                break;
            case 3:
                SetDefaultProperties_Centipede();
                break;
        }
    }

    public void SetDefaultProperties_Bosses()
    {
        CancelEnemyInvokes();
        //_PreparingBoss = false;
        Vector3 BossStart = new Vector3(0, TopBlockPosition, 0);

        switch (BossNumber)
        {
            case 1:
                Instantiate(Boss1, BossStart, Quaternion.Euler(90, 0, 180));
                BossNumber++;
                break;
            case 2:
                Instantiate(Boss2, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 3:
                Instantiate(Boss3, BossStart, Quaternion.Euler(90, 0, 180));
                BossNumber++;
                break;
            case 4:
                Instantiate(Boss4, BossStart, Quaternion.Euler(90, 0, 180));
                BossNumber++;
                break;
            case 5:
                Instantiate(Boss5, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 6:
                Instantiate(Boss6, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 7:
                Instantiate(Boss7, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 8:
                Instantiate(Boss8, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 9:
                Instantiate(Boss9, BossStart, Quaternion.identity);
                BossNumber++;
                break;
            case 10:
                Instantiate(Boss10, BossStart, Quaternion.identity);
                OmegaPop = true;
                BossNumber++;
                break;
            case 11:
                // DO SOMETHING WHEN BOSS TEN DIES?
                break;
        }
    }

    public IEnumerator Gauntlet()
    {
        Debug.Log("Starting Gauntlet");
        //_PreparingBoss = true;
        yield return new WaitForSeconds(5f);
        Debug.Log("POP");
        StartCoroutine(_GameInfo.SpawnBoss());
    }

	// Update is called once per frame
	void Update () 
    {
        TopBlockPosition = TopBlock.transform.position.y;

        //if (_Gauntlet && _PreparingBoss == false)
        //{
        //    StartCoroutine(Gauntlet());
        //}
	}
}

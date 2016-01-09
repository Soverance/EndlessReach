// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Boundings : MonoBehaviour {

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
            Debug.Log("FUCK YOU");
        }
    }
}

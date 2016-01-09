// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Turret_LaserLine : MonoBehaviour {

    public LineRenderer laser;
    public GameObject point;
 
    private Ray ray;
    private RaycastHit hit;
 
    void Start()
    {
        //point = GameObject.Find("TargetingPoint");
    }

    void Update()
    {
        int planeLayerMask = 1 << 17;

        ray = new Ray(laser.transform.position, laser.transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayerMask))
        {
            point.SetActive(true);
            point.transform.position = hit.point;
            //laser.SetPosition(0, transform.position);
            //laser.SetPosition(1, hit.point);
        }
        else
        {
            point.SetActive(false);
            //point.transform.position = ray.direction * 1000;
            //laser.SetPosition(0, transform.position);
            //laser.SetPosition(1, ray.direction * 1000);
        }
    }
}

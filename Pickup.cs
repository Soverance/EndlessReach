// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public float ItemMinSpeed;  // min speed, set in editor
    public float ItemMaxSpeed;  // max speed, set in editor
    private float ItemSpeed;  // current speed
    private GameObject BottomBlock;
    private GameObject TopBlock;

    void Start()
    {
        // randomly generate a speed based on Min and Max set in editor
        ItemSpeed = Random.Range(ItemMinSpeed, ItemMaxSpeed);
        BottomBlock = GameObject.Find("BottomBlock");
        TopBlock = GameObject.Find("TopBlock");
    }


    // Update is called once per frame
    void Update()
    {
        // move the object across the screen
        float MoveDistance = ItemSpeed * Time.deltaTime;
        transform.Translate(Vector3.down * MoveDistance);  // translate downward every frame
        transform.Rotate(0, 10, 0 * Time.deltaTime);  // rotate 10 degrees per second around z axis

        if (this.transform.position.y <= (BottomBlock.transform.position.y - 40))
        {
            this.gameObject.SetActive(false);
            Vector3 NewPos = new Vector3 (this.transform.position.x, TopBlock.transform.position.y, 0);
            this.transform.position = NewPos;
        }
    }
}
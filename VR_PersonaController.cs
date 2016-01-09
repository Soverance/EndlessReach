// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class VR_PersonaController : MonoBehaviour {

    // XInput stuff
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private Vector2 MaxSpeed;
    private Vector2 RotSpeed;
    Rigidbody _rigidbody;

	// Use this for initialization
	void Start () 
    {
        MaxSpeed = new Vector2(3.0f, 3.0f);
        RotSpeed = new Vector2(25.0f, 25.0f);
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // RESET VELOCITY
        Vector3 velocity = new Vector3();  // New velocity vector each frame
        Vector2 rotation = new Vector2();

        // update controller state
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        // LEFT STICK MOVEMENT
        velocity.z = -state.ThumbSticks.Left.X * MaxSpeed.x;  // left / right
        velocity.y = 0;  // up / down
        velocity.x = state.ThumbSticks.Left.Y * MaxSpeed.y;  // forward / backward
        _rigidbody.velocity = velocity;  // apply velocity

        // RIGHT STICK ROTATION
        rotation.x = -state.ThumbSticks.Right.X * RotSpeed.x;
        rotation.y = state.ThumbSticks.Right.Y * RotSpeed.y;
        _rigidbody.rotation = Quaternion.Euler(rotation);
	}
}

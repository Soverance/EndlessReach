// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XInputState : MonoBehaviour {

    // XInput stuff
    bool playerIndexSet = false;
    public static PlayerIndex playerIndex;
    public static GamePadState state;
    GamePadState prevState;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        #region Update Controller State
        // update controller state
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
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
        #endregion

        // OVR RESET
        if (state.Buttons.Back == ButtonState.Pressed)
        {
            OVRManager.display.RecenterPose();  // OVR Reset function for 0.4.3
        }
	
	}
}

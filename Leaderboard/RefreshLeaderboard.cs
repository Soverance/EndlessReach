// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System;
using System.Collections;
using LitJson;

public struct OptionalMiddleStruct
{
    public int score;
    public string name;
    public string place;
}

public class RefreshLeaderboard : MonoBehaviour
{
    public GameObject _UniversalScorePanel;
    public GameObject _LeaderboardButton;
    private GameObject _PanelToDeactivate;
    public GameObject DataConnector;
    private OptionalMiddleStruct containerToSend;
    public GameObject SubmitEffect;

	public void RefreshBoard(JsonData[] ssObjects)
	{
		OptionalMiddleStruct container = new OptionalMiddleStruct();
		
		for (int i = 0; i < ssObjects.Length; i++) 
		{
            container.place = ssObjects[i]["place"].ToString();
			container.name = ssObjects[i]["name"].ToString();
            container.score = int.Parse(ssObjects[i]["score"].ToString());
			
			UpdateBoardValues(container);
		}	
	}

	void UpdateBoardValues(OptionalMiddleStruct container)
	{
        //  Find appropriate container
		GameObject ScoreContainer = GameObject.Find(container.place);

        //  Collect appropriate child objects to update
        UILabel Score = ScoreContainer.GetComponentInChildren<LeaderboardScore>().gameObject.GetComponent<UILabel>();
        UILabel Name = ScoreContainer.GetComponentInChildren<LeaderboardName>().gameObject.GetComponent<UILabel>();

        // Apply values to objects in container
        Score.text = container.score.ToString();
        Name.text = container.name;
	}


    public void SortAndSend(JsonData[] ssObjects)
    {
        OptionalMiddleStruct container = new OptionalMiddleStruct();
        //int score = 0;

        // Break apart data and test it against current score
        for (int i = 0; i < ssObjects.Length; i++)
        {
            container.place = ssObjects[i]["place"].ToString();
            container.name = ssObjects[i]["name"].ToString();
            container.score = int.Parse(ssObjects[i]["score"].ToString());

            //Debug.Log("Checking.........");
            // if current score is higher than 
            if (EndlessPlayerController.Score >= container.score)
            {
                container.score = EndlessPlayerController.Score;
               // Debug.Log("You've beaten a global high score!");

                _PanelToDeactivate = GameObject.FindGameObjectWithTag("HUDPanel");
                _PanelToDeactivate.SetActive(false);
                _UniversalScorePanel.SetActive(true);
                containerToSend = container; // add to external container in preparation to send
                
                EndlessGameInfo._AllowGlobalHighScore = true;
                break;
            }                
        }        
    }

    public void OnSubmit()
    {
        SubmitEffect.SetActive(true);
        containerToSend.name = UIInput.current.value;  // collect Pilot Name
        DataConnector.SendMessage("SaveDataOnCloud", containerToSend);  // send data to the cloud
        StartCoroutine(OpenLeaderboard());
    }

    IEnumerator OpenLeaderboard()
    {
        yield return new WaitForSeconds(4f);
        _LeaderboardButton.SendMessage("OnClick");
    }
}


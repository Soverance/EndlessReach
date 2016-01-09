// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using System.Collections;
using UnityEngine;
using LitJson;
 
public class ERDataConnector : MonoBehaviour
{
	public string webServiceUrl = "";
	public string spreadsheetId = "";
	public string sheetName = "";
    public string newRecordName = "";
	public string password = "";
	public float maxWaitTime = 10f;
	public GameObject dataDestinationObject;
    public UILabel ConnectionString;

	bool updating;
	JsonData[] ssObjects;
	bool saveToGS; 
	
	void Start ()
	{
		updating = false;
		saveToGS = false;
	}

    public void CheckScores()
    {
        //Debug.Log("Checking Scores from data connector...");
        //ConnectionString.text = "Connecting to server....";
        if (updating)
            return;

        updating = true;
        StartCoroutine(GetAndSort());
    }
	
	public void Connect()
	{
        //ConnectionString.text = "Connecting to server....";
		if (updating)
			return;
		
		updating = true;
		StartCoroutine(GetAndRefresh());   
	}
	
    // This function will connect (and show on screen debug info), collect the data, and display it on the leaderboard
	IEnumerator GetAndRefresh()
	{
		string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + sheetName + "&pass=" + password + "&action=GetData";
		Debug.Log("Connecting to webservice on " + connectionString);

		WWW www = new WWW(connectionString);
		
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				ConnectionString.text = "Max wait time reached, connection aborted.";
                Debug.Log(ConnectionString.text);
				updating = false;
				break;
			}
			
			yield return null;  
		}
	
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			ConnectionString.text = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
            Debug.LogError(ConnectionString.text);
			updating = false;
			yield break;
		}
	
		string response = www.text;
		Debug.Log(elapsedTime + " : " + response);
		ConnectionString.text = "Connection stablished, parsing data...";

		if (response == "\"Incorrect Password.\"")
		{
			ConnectionString.text = "Connection error: Incorrect Password.";
            Debug.LogError(ConnectionString.text);
			updating = false;
			yield break;
		}

		try 
		{
			ssObjects = JsonMapper.ToObject<JsonData[]>(response);
		}
		catch
		{
			ConnectionString.text = "Data error: could not parse retrieved data as json.";
            Debug.LogError(ConnectionString.text);
			updating = false;
			yield break;
		}

		ConnectionString.text = "Data Successfully Retrieved!";
		updating = false;
		
		// Finally use the retrieved data as you wish.
		dataDestinationObject.SendMessage("RefreshBoard", ssObjects);
	}


    // This function will invisibly connect, collect the data, and send the high score data to the GameInfo class
    // in order to test it against the player's current score
    IEnumerator GetAndSort()
    {
        string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + sheetName + "&pass=" + password + "&action=GetData";
        //Debug.Log("Connecting to webservice on " + connectionString);

        WWW www = new WWW(connectionString);

        float elapsedTime = 0.0f;

        while (!www.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= maxWaitTime)
            {
                //ConnectionString.text = "Max wait time reached, connection aborted.";
                Debug.Log(ConnectionString.text);
                updating = false;
                break;
            }

            yield return null;
        }

        if (!www.isDone || !string.IsNullOrEmpty(www.error))
        {
            //ConnectionString.text = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
            Debug.LogError(ConnectionString.text);
            updating = false;
            yield break;
        }

        string response = www.text;
        Debug.Log(elapsedTime + " : " + response);
        //ConnectionString.text = "Connection stablished, parsing data...";

        if (response == "\"Incorrect Password.\"")
        {
            //ConnectionString.text = "Connection error: Incorrect Password.";
            Debug.LogError(ConnectionString.text);
            updating = false;
            yield break;
        }

        try
        {
            ssObjects = JsonMapper.ToObject<JsonData[]>(response);
        }
        catch
        {
            //ConnectionString.text = "Data error: could not parse retrieved data as json.";
            Debug.LogError(ConnectionString.text);
            updating = false;
            yield break;
        }

        //ConnectionString.text = "Data Successfully Retrieved!";
        updating = false;

        //Debug.Log("Sorting...");
        // Finally use the retrieved data as you wish.
        dataDestinationObject.SendMessage("SortAndSend", ssObjects);
    }




	public void SaveDataOnCloud(OptionalMiddleStruct container)
	{
        saveToGS = true;
		if (saveToGS)
			StartCoroutine(SendData(container)); 
	} 

	IEnumerator SendData(OptionalMiddleStruct container)
	{        
		if (!saveToGS)
			yield break;
        Debug.Log("SENDING DATA TO SPREADSHEET");
		string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + newRecordName + "&pass=" + password + "&val1=" + container.score + "&val2=" + container.name + "&action=SetData";
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;

		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
                Debug.Log("Max Upload Wait Time Reached");
				// Error handling here.
				break;
			}

			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
            Debug.Log("www.error");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;

		if (response.Contains("Incorrect Password"))
		{
            Debug.Log("Incorrect Password");
			// Error handling here.
			yield break;
		}

		if (response.Contains("RCVD OK"))
		{
            Debug.Log("Data RCVD OK!");
			// Data correctly sent!
			yield break;
		}
	}
}
	
	
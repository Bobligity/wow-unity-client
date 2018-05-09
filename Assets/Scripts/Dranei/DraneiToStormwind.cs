using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class DraneiToStormwind : MonoBehaviour {
    WowClient client;
    float delayTime = 2f;
    public bool startJourney = false;
    public bool reachedAzureMystIsle = false;

    int counter = 0;
    System.IO.StreamReader file;
    string pathFile = @"C:\Users\Meta\Desktop\RPGBox\WoW_Horizon\Paths\DraneiPathToStormwind1.txt";
    Vector3 lastTargetLocation;
    string[] readText;
	void Start () {
        readText = File.ReadAllLines(pathFile);
        string processLog = StartWoWClient();
        client = GetComponent<WowClient>();
        client.processHandle = processLog;
        delayTime += Time.time;
	}

    string StartWoWClient()
    { 
        string response = WebService.Get("http://localhost:12345/Login/");
        //Debug.Log(response);
        return response;
    }

    

	void Update () {
        if (Time.time > delayTime)
        {
            startJourney = true;
            WalkToAzureMystIsle();
            Debug.Log("journey started");
        }

        if (startJourney)
        {   
            if (!reachedAzureMystIsle)
            {
                string response = WebService.Get("http://localhost:12345/getLocalPlayerInfo/");
                CTM_Pos target = JsonUtility.FromJson<CTM_Pos>(response);

                Vector3 myLocation = new Vector3(target.X, target.Y, target.Z);
                if (Vector3.Distance(myLocation, lastTargetLocation) < 0.1f)
                {
                    Debug.Log("Sending move command");
                    WalkToAzureMystIsle();
                    counter++;
                }
                
            }
                
        }
	}

    void OnApplicationQuit()
    {
        file.Close();
    }

    private void WalkToAzureMystIsle()
    {
        string line = readText[counter];
        
        CTM_Pos target = JsonUtility.FromJson<CTM_Pos>(line);
        //Debug.Log(target.X.ToString());

        string url = String.Format("http://localhost:12345/MoveToPoint/{0}/{1}/{2}/", target.X.ToString(), target.Y.ToString(), target.Z.ToString());
        Debug.Log(url);
        WebService.AsyncGet(url);
        reachedAzureMystIsle = true;
        //lastTargetLocation = new Vector3(target.X, target.Y, target.Z);
        //counter++;

    }
}

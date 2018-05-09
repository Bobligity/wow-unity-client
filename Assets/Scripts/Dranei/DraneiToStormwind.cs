using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class DraneiToStormwind : MonoBehaviour {
    WowClient client;
    float delayTime = 5f;
    public bool startJourney = false;
    public bool reachedAzureMystIsle = false;
    public float distance = 0f;

    public int counter = 0;
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
        if (Time.time > delayTime && !startJourney)
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
                //Debug.Log("DISTANCE CHECK");
                distance = Vector3.Distance(myLocation, lastTargetLocation);
                if (distance < 10f)
                {
                    Debug.Log("Sending move command");
                    WalkToAzureMystIsle();
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

        if (counter >= readText.Length - 1)
        {
            reachedAzureMystIsle = true;
            return;
        }
            
        string line = readText[counter];
        
        CTM_Pos target = JsonUtility.FromJson<CTM_Pos>(line);
        //Debug.Log(target.X.ToString());

        string url = String.Format("http://localhost:12345/MoveToPoint/{0}/{1}/{2}/", target.X.ToString(), target.Y.ToString(), target.Z.ToString());
        Debug.Log(url);
        StartCoroutine(WebService.AsyncGet(url));
        lastTargetLocation = new Vector3(target.X, target.Y, target.Z);
        counter++;

    }
}

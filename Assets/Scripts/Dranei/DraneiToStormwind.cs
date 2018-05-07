using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DraneiToStormwind : MonoBehaviour {
    WowClient client;

	void Start () {
        string processLog = StartWoWClient();
        client = GetComponent<WowClient>();
        client.processHandle = processLog;
	}

    string StartWoWClient()
    { 
        string response = WebService.Get("http://localhost:12345/Login/");
        Debug.Log(response);
        return response;
    }

    


	void Update () {
		
	}
}

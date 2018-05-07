using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordMovement : MonoBehaviour {
    public bool record = false;
    float lastRecordTime = 0f;
    public float startRecord = 0f;

    string path = @"path_records.txt";
    void logPosition()
    {
        string response = WebService.Get("http://localhost:12345/getLocalPlayerInfo/");
        Debug.Log(response);
        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(response.Replace("\r\n"," "));
        }	
    }

	void Update()
    {
        if (record)
        {
            if (startRecord == 0f)
            {
                startRecord = Time.time;
                lastRecordTime = Time.time;
                logPosition();
            }
            if (Time.time - lastRecordTime > 0.5f)
            {
                lastRecordTime = Time.time;
                logPosition();
            }
                
        }
    }
}

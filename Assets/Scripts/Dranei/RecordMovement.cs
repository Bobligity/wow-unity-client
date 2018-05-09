using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordMovement : MonoBehaviour {
    public bool record = false;
    float lastRecordTime = 0f;
    public float startRecord = 0f;
    CTM_Pos lastTarget;
    public float last_mx = 0f;
    public float last_my = 0f;
    public float mx = 0f;
    public float my = 0f;
    public float diff_mx;
    public float diff_my;
    public float last_slope = 0f;
    public float slope = 0f;
    public Vector3 myLocation;


    string path = @"path_records.txt";
    void logPosition(string response)
    {
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
                Debug.Log("Starting Record");
                startRecord = Time.time;
                lastRecordTime = Time.time;
                string response = WebService.Get("http://localhost:12345/getLocalPlayerInfo/");
                lastTarget = JsonUtility.FromJson<CTM_Pos>(response);
                logPosition(response);
            }
            if (Time.time - lastRecordTime > 1f)
            {
                lastRecordTime = Time.time;
                string response = WebService.Get("http://localhost:12345/getLocalPlayerInfo/");
                CTM_Pos target = JsonUtility.FromJson<CTM_Pos>(response);
                
                myLocation = new Vector3(target.X, target.Y, target.Z); 
                mx = target.X - lastTarget.X;
                my = target.Y - lastTarget.Y;
                diff_mx = Mathf.Abs(last_mx - mx);
                diff_my = Mathf.Abs(last_my - my);
                slope = diff_mx / diff_my;

                if (Mathf.Abs(target.X - lastTarget.X) + Mathf.Abs(target.Y - lastTarget.Y) < 10f)
                {

                }
                else
                {
                    logPosition(response);
                    last_mx = mx;
                    last_my = my;
                    lastTarget = target;
                    last_slope = slope;
                }

            }
                
        }
    }

}

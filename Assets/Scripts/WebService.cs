using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

public class WebService : MonoBehaviour {
    public static string Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public static IEnumerator AsyncGet(string uri)
    {
        using (WWW www = new WWW(uri))
        {
            yield return www;
        }
    }
}

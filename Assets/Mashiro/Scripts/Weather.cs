using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mashiro;

public class Weather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IEAnime());
    }

    IEnumerator IEAnime()
    {
        using (UnityWebRequest www = new UnityWebRequest())
        {
            www.url = "http://www.weather.com.cn/data/cityinfo/101010100.html";
            www.method = UnityWebRequest.kHttpVerbGET;

            www.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log("aa");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                //var temp = JsonUtility.FromJson<WeatherInfo>("{\"city\":\"北京\",\"cityid\":\"101010100\",\"temp1\":\"18℃\",\"temp2\":\"31℃\",\"weather\":\"多云转阴\",\"img1\":\"n1.gif\",\"img2\":\"d2.gif\",\"ptime\":\"18:00\"}");

                //string json = "{\"city\":\"北京\",\"cityid\":\"101010100\"}";
                //var temp = JsonUtility.FromJson<WeatherInfo>(json);

                var temp = JsonUtility.FromJson<WeatherModel>(www.downloadHandler.text);

                Debug.Log(temp.weatherinfo.city);
                

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mashiro
{
    public class WeatherShow : MonoBehaviour
    {
        private Text txt;

        void Start()
        {
            txt = GetComponent<Text>();
        }

        private void OnEnable()
        {
            if (txt == null)
            {
                txt = GetComponent<Text>();
            }
            StartCoroutine(IEAnime());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator IEAnime()
        {
            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = "https://www.apiopen.top/weatherApi?city=成都";
                //www.url = "http://www.weather.com.cn/data/cityinfo/101010100.html";

                www.method = UnityWebRequest.kHttpVerbGET;

                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    txt.text = txt.text + "\r\n" + www.error;
                    txt.text = txt.text + "\r\n" + www.isNetworkError.ToString();
                    txt.text = txt.text + "\r\n" + www.isHttpError.ToString();

                }
                else
                {

                    //var temp = JsonUtility.FromJson<WeatherModel>(www.downloadHandler.text);

                    //txt.text = temp.weatherinfo.city + "\r\n"
                    //    + temp.weatherinfo.weather + "\r\n"
                    //    + temp.weatherinfo.temp1 + "-" + temp.weatherinfo.temp2 + "\r\n"
                    //    + temp.weatherinfo.ptime;

                    txt.text = www.downloadHandler.text;

                }
            }

        }
    }

}

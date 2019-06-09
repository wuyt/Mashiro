using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Mashiro
{
    /// <summary>
    /// 内容点网络内容显示
    /// </summary>
    [RequireComponent(typeof(POIController))]
    public class POIWebController : MonoBehaviour
    {
        /// <summary>
        /// 内容显示
        /// </summary>
        private POIController poi;
        /// <summary>
        /// 访问地址
        /// </summary>
        public string url;
        // Start is called before the first frame update
        void Awake()
        {
            poi = GetComponent<POIController>();
        }

        void OnEnable()
        {
            StartCoroutine(GetHttp());
        }

        void OnDisable()
        {
            StopCoroutine(GetHttp());
        }

        IEnumerator GetHttp()
        {
            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = url;
                www.method = UnityWebRequest.kHttpVerbGET;

                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    poi.txtTitle = "访问出错";
                    poi.txtContent = www.isNetworkError.ToString() + www.isHttpError.ToString();
                    poi.txtContent = www.error + "\r\n" + poi.txtContent;
                }
                else
                {
                    string info = Encoding.ASCII.GetString(www.downloadHandler.data);
                    info = info.Substring(info.IndexOf("\""));
                    info = info.Replace("\";", "");
                    info = info.Replace("\"", "");
                    var arrayInfo = info.Split(',');
                    
                    poi.txtContent = "date:" + arrayInfo[30] + "  time:" + arrayInfo[31];
                    poi.txtContent = "Yesterday's closing price:" + arrayInfo[2] + "\r\n" + poi.txtContent;
                    poi.txtContent = "Current price:" + arrayInfo[3] + "\r\n" + poi.txtContent;


                    poi.txtMore = "------------------------------" + "\r\n" ;
                    poi.txtMore = "date:" + arrayInfo[30] + "  time:" + arrayInfo[31] + "\r\n" + poi.txtMore;
                    poi.txtMore = "Today's lowest price:" + arrayInfo[5] + "  Today's highest price:" + arrayInfo[4] + "\r\n" + poi.txtMore;
                    poi.txtMore = "Yesterday's closing price:" + arrayInfo[2] + "  Opening price today:" + arrayInfo[1] + "\r\n" + poi.txtMore;
                    poi.txtMore = "Current price:" + arrayInfo[3] + "\r\n" + poi.txtMore;
                }
                poi.UpdateUI();

            }
        }
    }

}

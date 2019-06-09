using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

namespace Mashiro
{
    /// <summary>
    /// ARCore错误提示
    /// </summary>
    public class ARCoreErrorController : MonoBehaviour
    {
        /// <summary>
        /// 异常显示文本
        /// </summary>
        private Text text;
        /// <summary>
        /// 异常显示画布
        /// </summary>
        private GameObject canvas;

        void Start()
        {
            canvas = GameObject.Find("/CanvasError");
            text = canvas.transform.Find("Panel/Text").GetComponent<Text>();
            canvas.SetActive(false);
        }

        void Update()
        {
            //处于追踪状态不会进入睡眠模式
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = 15;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            //显示异常后不继续操作
            if (canvas.activeSelf)
            {
                return;
            }
            //如果有异常则显示，并于1秒后返回菜单
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                canvas.SetActive(true);
                text.text = "Camera permission is needed.";
            }
            else if (Session.Status.IsError())
            {
                canvas.SetActive(true);
                text.text = "ARCore encountered a problem connecting.";
            }
        }
    }
}


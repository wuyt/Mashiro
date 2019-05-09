using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mashiro
{
    public class MainController : MonoBehaviour
    {
        /// <summary>
        /// 位置模型
        /// </summary>
        public Transform place;
        /// <summary>
        /// 计时滚动条
        /// </summary>
        public Slider slider;
        /// <summary>
        /// 模型字典
        /// </summary>
        private Dictionary<int, GameObject> dictPrefab
                = new Dictionary<int, GameObject>();
        /// <summary>
        /// 识别图片列表
        /// </summary>
        private List<AugmentedImage> listAugmentedImage
            = new List<AugmentedImage>();

        /// <summary>
        /// 
        /// </summary>
        private enum Status
        {
            finding,
            waiting,
            outside,
            inside
        }

        private Status status;


        private float waitTime;

        public GameObject canvasDoor;

        public GameObject[] BCanvas;

        public GameObject canvasWindow;

        public Text txtWindow;

        private void Start()
        {
            status = Status.finding;
            slider.gameObject.SetActive(false);
            canvasDoor.SetActive(false);

            foreach (var i in BCanvas)
            {
                i.SetActive(false);
            }

            canvasWindow.SetActive(false);
        }

        void Update()
        {
            switch (status)
            {
                case Status.finding:
                    FindImage();
                    break;
                case Status.waiting:
                    Waiting();
                    break;
                case Status.outside:
                    break;
                case Status.inside:
                    break;
            }

            //检查是否处于追踪状态
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            //更新识别图片列表
            Session.GetTrackables<AugmentedImage>(
                listAugmentedImage, TrackableQueryFilter.Updated);

            //遍历识别图片列表
            foreach (AugmentedImage image in listAugmentedImage)
            {
                //从字典中获取模型
                dictPrefab.TryGetValue(image.DatabaseIndex, out GameObject outPrefab);

                if (image.TrackingState == TrackingState.Tracking
                    && outPrefab == null)   //识别图片被发现
                {
                    //在识别图片中心添加追踪锚点
                    Anchor anchor = image.CreateAnchor(image.CenterPose);

                    //将位置模型移动对应
                    place.position = anchor.transform.position;
                    dictPrefab.Add(image.DatabaseIndex, place.gameObject);
                    place.parent = anchor.transform;
                    place.localPosition = Vector3.zero;
                    place.localRotation = Quaternion.Euler(90f,0f,0f);

                }
                else if (image.TrackingState == TrackingState.Stopped
                    && outPrefab != null)   //识别图片消失
                {
                    //从字典中移除对应内容
                    dictPrefab.Remove(image.DatabaseIndex);
                    //删除模型
                    //GameObject.Destroy(outPrefab);
                }
            }
        }

        private void FindImage()
        {
            //检查是否处于追踪状态
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            //更新识别图片列表
            Session.GetTrackables<AugmentedImage>(
                listAugmentedImage, TrackableQueryFilter.Updated);

            //遍历识别图片列表
            foreach (AugmentedImage image in listAugmentedImage)
            {
                //从字典中获取模型
                dictPrefab.TryGetValue(image.DatabaseIndex, out GameObject outPrefab);

                if (image.TrackingState == TrackingState.Tracking
                    && outPrefab == null)   //识别图片被发现
                {
                    //在识别图片中心添加追踪锚点
                    Anchor anchor = image.CreateAnchor(image.CenterPose);

                    //将位置模型移动对应
                    place.position = anchor.transform.position;
                    dictPrefab.Add(image.DatabaseIndex, place.gameObject);
                    place.parent = anchor.transform;
                    place.localPosition = Vector3.zero;
                    place.localRotation = Quaternion.Euler(90f, 0f, 0f);

                    slider.gameObject.SetActive(true);
                    slider.value = 0f;
                    waitTime = Time.time;
                    status = Status.waiting;
                }
            }
        }

        private void Waiting()
        {
            slider.value = (Time.time - waitTime) / 5;

            if (slider.value >= 1)
            {
                status = Status.outside;
                OnOutside();
            }

            //检查是否处于追踪状态
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            //更新识别图片列表
            Session.GetTrackables<AugmentedImage>(
                listAugmentedImage, TrackableQueryFilter.Updated);

            //遍历识别图片列表
            foreach (AugmentedImage image in listAugmentedImage)
            {
                //从字典中获取模型
                dictPrefab.TryGetValue(image.DatabaseIndex, out GameObject outPrefab);

                if (image.TrackingState == TrackingState.Stopped
                    && outPrefab != null)   //识别图片消失
                {
                    //从字典中移除对应内容
                    dictPrefab.Remove(image.DatabaseIndex);

                    place.parent = null;
                    place.position = new Vector3(0f, 5f, 0f);
                    slider.gameObject.SetActive(false);
                    status = Status.finding;
                }
            }
        }

        private void OnOutside()
        {
            slider.gameObject.SetActive(false);
            canvasDoor.SetActive(true);
            place.parent = null;
        }

        private void OnCameraTriggerEnter(Collider other)
        {
            if (other.name == "Door")
            {
                canvasDoor.SetActive(false);
                status = Status.inside;
            }

            if(other.name == "Box1")
            {
                foreach(var i in BCanvas)
                {
                    i.SetActive(true);
                }
            }

            if (other.name == "Windows")
            {
                canvasWindow.SetActive(true);
            }
        }
    }
}

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
        public Transform room;
        /// <summary>
        /// 模型字典
        /// </summary>
        private Dictionary<int, Transform> dictRoom
                = new Dictionary<int, Transform>();
        /// <summary>
        /// 识别图片列表
        /// </summary>
        private List<AugmentedImage> listAugmentedImage
            = new List<AugmentedImage>();
        /// <summary>
        /// 查找时画布
        /// </summary>
        public GameObject findingCanvas;
        /// <summary>
        /// 等待时候画布
        /// </summary>
        public GameObject watingCanvas;

        /// <summary>
        /// 状态枚举
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// 发现定位
            /// </summary>
            finding,
            /// <summary>
            /// 等待
            /// </summary>
            waiting,
            /// <summary>
            /// 追踪
            /// </summary>
            tracking
        }
        /// <summary>
        /// 状态
        /// </summary>
        private Status status;

        /// <summary>
        /// 等待时间
        /// </summary>
        private float waitTime;

        /// <summary>
        /// ARCore的摄像头
        /// </summary>
        public Transform arCoreCamera;

        private void Start()
        {
            status = Status.finding;
            StartFinding();
        }

        void Update()
        {
            switch (status)
            {
                case Status.finding:
                    Finding();
                    break;
                case Status.waiting:
                    Waiting();
                    break;
                case Status.tracking:
                    break;
            }
        }

        /// <summary>
        /// 查找过程
        /// </summary>
        private void Finding()
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
            foreach (var image in listAugmentedImage)
            {
                dictRoom.TryGetValue(image.DatabaseIndex, out Transform outTransform);
                if (image.TrackingState == TrackingState.Tracking && outTransform == null)
                {
                    //图片被识别

                    //查看图片识别处是否有识别平面
                    TrackableHit hit;
                    TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                        TrackableHitFlags.FeaturePointWithSurfaceNormal;

                    if (Frame.Raycast(arCoreCamera.transform.position,
                        (image.CenterPose.position - arCoreCamera.transform.position).normalized,
                        out hit, 10f, raycastFilter))
                    {
                        if ((hit.Trackable is DetectedPlane) &&
                            Vector3.Dot(arCoreCamera.transform.position - hit.Pose.position,
                                hit.Pose.rotation * Vector3.up) < 0)
                        {
                            Debug.Log("Hit at back of the current DetectedPlane");
                        }
                        else
                        {
                            //有识别平面，进入等待状态
                            status = Status.waiting;
                            StartWaiting();
                        }
                    }
                }
                else if (image.TrackingState == TrackingState.Stopped && outTransform != null)
                {
                    dictRoom.Remove(image.DatabaseIndex);
                }
            }
        }

        /// <summary>
        /// 等待过程，目的是提高稳定程度，避免因为识别过快导致的偏移
        /// </summary>
        private void Waiting()
        {
            if ((Time.time - waitTime) > 2f)
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
                foreach (var image in listAugmentedImage)
                {
                    dictRoom.TryGetValue(image.DatabaseIndex, out Transform outTransform);
                    if (image.TrackingState == TrackingState.Tracking && outTransform == null)
                    {
                        //图片被识别

                        //建立锚点
                        Anchor anchor = image.CreateAnchor(image.CenterPose);

                        //根据锚点设置空间
                        room.gameObject.SetActive(true);
                        room.parent = anchor.transform;
                        room.localPosition = room.GetComponent<PositioningOffset>().position;
                        room.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        room.parent = null;

                        dictRoom.Add(image.DatabaseIndex, room);

                        //进入追踪状态
                        status = Status.tracking;
                        StartTracking();
                    }
                    else if (image.TrackingState == TrackingState.Stopped && outTransform != null)
                    {
                        dictRoom.Remove(image.DatabaseIndex);
                        status = Status.finding;
                        StartFinding();
                    }
                }
            }
        }

        /// <summary>
        /// 开始等待
        /// </summary>
        private void StartWaiting()
        {
            waitTime = Time.time;
            findingCanvas.SetActive(false);
            watingCanvas.SetActive(true);
            dictRoom.Clear();
        }

        /// <summary>
        /// 开始查找
        /// </summary>
        private void StartFinding()
        {
            watingCanvas.SetActive(false);
            findingCanvas.SetActive(true);
            room.gameObject.SetActive(false);
            dictRoom.Clear();
        }

        /// <summary>
        /// 开始追踪
        /// </summary>
        private void StartTracking()
        {
            watingCanvas.SetActive(false);
        }
    }
}

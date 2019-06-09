using UnityEngine;
using UnityEngine.Video;

namespace Mashiro
{
    /// <summary>
    /// 视频播放
    /// </summary>
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoController : MonoBehaviour
    {
        private VideoPlayer videoPlay;

        private void Awake()
        {
            videoPlay = GetComponent<VideoPlayer>();
        }

        private void OnEnable()
        {
            videoPlay.Play();
        }

        private void OnDisable()
        {
            videoPlay.Stop();
        }
    }

}

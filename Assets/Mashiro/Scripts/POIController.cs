using UnityEngine;
using UnityEngine.UI;

namespace Mashiro
{
    /// <summary>
    /// 内容点显示控制
    /// </summary>
    public class POIController : MonoBehaviour
    {
        /// <summary>
        /// 标题文本框
        /// </summary>
        private Text title;
        /// <summary>
        /// 内容文本框
        /// </summary>
        private Text content;
        /// <summary>
        /// 标题
        /// </summary>
        public string txtTitle;
        /// <summary>
        /// 内容
        /// </summary>
        public string txtContent;
        /// <summary>
        /// 更多内容
        /// </summary>
        public string txtMore;

        void Awake()
        {
            transform.Find("Canvas").GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
            title = transform.Find("Canvas/Background/Title/TitleLabel").GetComponent<Text>();
            content = transform.Find("Canvas/Background/Content/ContentLabel").GetComponent<Text>();

            UpdateUI();
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        public void UpdateUI()
        {
            title.text = txtTitle;
            content.text = txtContent;
        }
    }
}
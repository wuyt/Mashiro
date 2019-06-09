using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Mashiro
{
    /// <summary>
    /// 详细信息显示
    /// </summary>
    public class InfoController : MonoBehaviour
    {
        /// <summary>
        /// 是否显示UI
        /// </summary>
        private bool isShow;
        /// <summary>
        /// 背景
        /// </summary>
        public GameObject panel;
        /// <summary>
        /// 标题文本框
        /// </summary>
        private Text title;
        /// <summary>
        /// 内容文本框
        /// </summary>
        private Text content;
        /// <summary>
        /// 点击对象
        /// </summary>
        private Transform label;

        // Start is called before the first frame update
        void Start()
        {
            isShow = false;
            panel.SetActive(false);
            title = transform.Find("Panel/Background/Title/TitleLabel").GetComponent<Text>();
            content = transform.Find("Panel/Background/Content/ContentLabel").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isShow || Input.touchCount != 1)
            {
                return;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                int mask = 1 << 9;

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
                {
                    label = hit.transform;
                    ShowUI();
                }
            }
        }
        /// <summary>
        /// 显示UI
        /// </summary>
        private void ShowUI()
        {
            panel.SetActive(true);
            isShow = true;
            panel.transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).OnComplete(ShowUIComplete);
        }
        /// <summary>
        /// 显示动作完成
        /// </summary>
        private void ShowUIComplete()
        {
            POIController poi = label.GetComponent<POIController>();

            title.text = poi.txtTitle;
            content.text = poi.txtContent + "\r\n" + poi.txtMore;
        }
        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void HiddenUI()
        {
            panel.transform.DORotate(new Vector3(0f, 90f, 0f), 0.5f).OnComplete(HiddenUIComplete);
        }
        /// <summary>
        /// 隐藏UI动作完成
        /// </summary>
        private void HiddenUIComplete()
        {
            panel.SetActive(false);
            isShow = false;
        }
    }

}

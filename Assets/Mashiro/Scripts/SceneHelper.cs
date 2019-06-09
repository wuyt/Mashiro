using UnityEngine;
using UnityEngine.UI;

namespace Mashiro
{
    /// <summary>
    /// 场景辅助，调整角度位置
    /// </summary>
    public class SceneHelper : MonoBehaviour
    {
        /// <summary>
        /// 当前对象
        /// </summary>
        private Transform currentTransform;
        /// <summary>
        /// 修改类型枚举
        /// </summary>
        private enum ModifyType
        {
            /// <summary>
            /// 定位
            /// </summary>
            position,
            /// <summary>
            /// 角度
            /// </summary>
            rotation
        }
        /// <summary>
        /// 修改轴枚举
        /// </summary>
        private enum ModifyAxis
        {
            x,
            y,
            z
        }
        /// <summary>
        /// 修改类型
        /// </summary>
        private ModifyType modifyType;
        /// <summary>
        /// 修改轴
        /// </summary>
        private ModifyAxis modifyAxis;
        /// <summary>
        /// 修改类型显示文本框
        /// </summary>
        private Text textPR;
        /// <summary>
        /// 修改轴显示文本框
        /// </summary>
        private Text textAxis;
        /// <summary>
        /// 输入框
        /// </summary>
        private InputField input;
        /// <summary>
        /// 对象名称文本框
        /// </summary>
        private Text textName;
        /// <summary>
        /// 对象信息文本框
        /// </summary>
        private Text textInfo;
        /// <summary>
        /// 默认对象
        /// </summary>
        public Transform defaultTF;

        void Start()
        {
            currentTransform = defaultTF;
            textPR = transform.Find("TextPR").GetComponent<Text>();
            textAxis = transform.Find("TextAxis").GetComponent<Text>();
            input = transform.Find("InputField").GetComponent<InputField>();
            textName = transform.Find("TextName").GetComponent<Text>();
            textInfo = transform.Find("TextInfoShow").GetComponent<Text>();

            SetModifyType(true);
            SetModifyAxis(-1);
        }

        void Update()
        {
            if (Input.touchCount != 1)
            {
                return;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                int mask = 1 << 9;

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
                {
                    currentTransform = hit.transform;
                    UpdateUI();
                }
            }
        }

        /// <summary>
        /// 设置修改类型
        /// </summary>
        /// <param name="setType">true：定位；false：角度</param>
        public void SetModifyType(bool setType)
        {
            if (setType)
            {
                modifyType = ModifyType.position;
            }
            else
            {
                modifyType = ModifyType.rotation;
            }

            UpdateUI();
        }

        /// <summary>
        /// 设置修改轴
        /// </summary>
        /// <param name="setAxis">-1：x轴；0：y轴；1：z轴</param>
        public void SetModifyAxis(int setAxis)
        {
            switch (setAxis)
            {
                case -1:
                    modifyAxis = ModifyAxis.x;
                    break;
                case 0:
                    modifyAxis = ModifyAxis.y;
                    break;
                case 1:
                    modifyAxis = ModifyAxis.z;
                    break;
            }

            UpdateUI();
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="operation">true：+；false：-</param>
        public void ModifyTransform(bool operation)
        {
            switch (modifyType)
            {
                case ModifyType.position:
                    if (operation)
                    {
                        ModifyPosition(float.Parse(input.text));
                    }
                    else
                    {
                        ModifyPosition(-float.Parse(input.text));
                    }
                    break;
                case ModifyType.rotation:
                    if (operation)
                    {
                        ModifyRotation(float.Parse(input.text));
                    }
                    else
                    {
                        ModifyRotation(-float.Parse(input.text));
                    }
                    break;
            }
        }
        /// <summary>
        /// 修改位置
        /// </summary>
        /// <param name="num"></param>
        private void ModifyPosition(float num)
        {
            var temp = currentTransform.localPosition;
            switch (modifyAxis)
            {
                case ModifyAxis.x:
                    currentTransform.localPosition = new Vector3(temp.x + num, temp.y, temp.z);
                    break;
                case ModifyAxis.y:
                    currentTransform.localPosition = new Vector3(temp.x, temp.y + num, temp.z);
                    break;
                case ModifyAxis.z:
                    currentTransform.localPosition = new Vector3(temp.x, temp.y, temp.z + num);
                    break;
            }
            UpdateUI();
        }
        /// <summary>
        /// 修改角度
        /// </summary>
        /// <param name="num"></param>
        private void ModifyRotation(float num)
        {
            var temp = currentTransform.localEulerAngles;
            switch (modifyAxis)
            {
                case ModifyAxis.x:
                    currentTransform.localEulerAngles = new Vector3(temp.x + num, temp.y, temp.z);
                    break;
                case ModifyAxis.y:
                    currentTransform.localEulerAngles = new Vector3(temp.x, temp.y + num, temp.z);
                    break;
                case ModifyAxis.z:
                    currentTransform.localEulerAngles = new Vector3(temp.x, temp.y, temp.z + num);
                    break;
            }
            UpdateUI();
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI()
        {
            textName.text = currentTransform.name;
            textInfo.text = "position:" + currentTransform.localPosition.ToString() + "\r\n"
                + "rotation:" + currentTransform.localEulerAngles.ToString();

            switch (modifyType)
            {
                case ModifyType.position:
                    textPR.text = "Position";
                    break;
                case ModifyType.rotation:
                    textPR.text = "Rotation";
                    break;
            }

            switch (modifyAxis)
            {
                case ModifyAxis.x:
                    textAxis.text = "X";
                    break;
                case ModifyAxis.y:
                    textAxis.text = "Y";
                    break;
                case ModifyAxis.z:
                    textAxis.text = "Z";
                    break;
            }
        }
    }

}

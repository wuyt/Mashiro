using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mashiro
{   
    /// <summary>
    /// 该脚本的意义仅仅是在编辑器中显示各物体的信息
    /// </summary>
    public class SizePlaceShow : MonoBehaviour
    {
        /// <summary>
        /// 大小
        /// </summary>
        [Header("大小")]
        public Vector3 Size;
        /// <summary>
        /// 位置
        /// </summary>
        [Header("相对Room在原点的时候的位置")]
        public Vector3 Position;
    }
}


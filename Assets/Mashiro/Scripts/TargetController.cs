using DG.Tweening;
using UnityEngine;

namespace Mashiro
{
    /// <summary>
    /// 目的地显示控制（自身不停旋转）
    /// </summary>
    public class TargetController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.DOLocalRotate(new Vector3(0f, 180f, 45f), 1.5f).SetLoops(-1, LoopType.Restart);
        }

        void OnDisable()
        {
            transform.DOPause();
        }

        void OnEnable()
        {
            transform.DOPlay();
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace Mashiro
{
    /// <summary>
    /// 室内导航控制
    /// </summary>
    public class NavController : MonoBehaviour
    {
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public GameObject btnMenu;
        /// <summary>
        /// 菜单
        /// </summary>
        public GameObject menu;
        /// <summary>
        /// 导航目的地
        /// </summary>
        private Transform target;
        /// <summary>
        /// 导航目的地父节点
        /// </summary>
        public Transform targets;
        /// <summary>
        /// 导航代理
        /// </summary>
        public NavMeshAgent agent;
        /// <summary>
        /// 导航路径
        /// </summary>
        private NavMeshPath path;
        /// <summary>
        /// 导航（动态更新用）
        /// </summary>
        public NavMeshSurface surface;
        /// <summary>
        /// 导航线
        /// </summary>
        public LineRenderer line;
        /// <summary>
        /// 玩家
        /// </summary>
        public Transform player;

        // Start is called before the first frame update
        void Start()
        {
            HiddenMenu();
            HiddenTargets();
            path = new NavMeshPath();
            agent.enabled = false;
        }
        /// <summary>
        /// 显示菜单
        /// </summary>
        public void ShowMenu()
        {
            btnMenu.SetActive(false);
            menu.SetActive(true);
        }
        /// <summary>
        /// 隐藏菜单
        /// </summary>
        public void HiddenMenu()
        {
            btnMenu.SetActive(true);
            menu.SetActive(false);
        }
        /// <summary>
        /// 导航到目的地
        /// </summary>
        /// <param name="tf">目的地</param>
        public void NavTarget(Transform tf)
        {
            CancelInvoke("Nav");
            HiddenTargets();

            target = tf;
            target.gameObject.SetActive(true);
            surface.BuildNavMesh();

            InvokeRepeating("Nav", 0, 0.5f);

            HiddenMenu();
        }
        /// <summary>
        /// 绘制导航线
        /// </summary>
        public void Nav()
        {
            agent.transform.position = player.position;
            agent.enabled = true;
            agent.CalculatePath(target.position, path);

            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);

            agent.enabled = false;
        }
        /// <summary>
        /// 隐藏目的地
        /// </summary>
        private void HiddenTargets()
        {
            for (int i = 0; i < targets.childCount; i++)
            {
                targets.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}

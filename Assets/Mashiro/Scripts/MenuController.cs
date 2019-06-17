using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mashiro
{
    /// <summary>
    /// 菜单控制
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        /// <summary>
        /// 退出应用
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }
    }
}


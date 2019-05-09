using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mashiro
{
    public class CameraSend : MonoBehaviour
    {
        private GameObject gameMaster;

        private void Start()
        {
            gameMaster = GameObject.Find("/GameMaster");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "Door")
            {
                GetComponent<SphereCollider>().radius = 2f;
            }
            gameMaster.SendMessage("OnCameraTriggerEnter", other);
        }
    }
}


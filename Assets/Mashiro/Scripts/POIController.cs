using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mashiro
{
    public class POIController : MonoBehaviour
    {
        void Start()
        {
            transform.Find("Canvas").GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
        }
    }
}
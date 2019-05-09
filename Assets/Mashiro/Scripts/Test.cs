using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void Start()
    {
        //SphereCollider sc = GetComponent<SphereCollider>();
        //sc.radius = 2;
        Debug.Log("start");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnEnable()
    {
        Debug.Log("enable");
    }
}

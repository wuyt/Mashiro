using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public Transform target;
    public Transform start;
    private Vector3 dr;

    // Start is called before the first frame update
    void Start()
    {
        dr = target.position - transform.position;
        //dr = dr.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits;
       //hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);
        hits = Physics.RaycastAll(transform.position, dr, 100f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
            }
        }
    }
}

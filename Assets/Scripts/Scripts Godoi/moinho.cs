using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moinho : MonoBehaviour
{
    Vector3 posi��o;
    Vector3 prox;
    private void Start()
    {
        posi��o = transform.position;
        prox.x = posi��o.x + 1;
        prox.y = posi��o.y + 1;
        prox.z = posi��o.z + 1;
    }
    void FixedUpdate()
    {
        transform.Rotate(prox.x, transform.rotation.y + 0.00005f, transform.rotation.z, Space.Self);
    }
}

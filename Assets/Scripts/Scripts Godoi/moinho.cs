using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moinho : MonoBehaviour
{
    Vector3 posição;
    Vector3 prox;
    private void Start()
    {
        posição = transform.position;
        prox.x = posição.x + 1;
        prox.y = posição.y + 1;
        prox.z = posição.z + 1;
    }
    void FixedUpdate()
    {
        transform.Rotate(prox.x, transform.rotation.y + 0.00005f, transform.rotation.z, Space.Self);
    }
}

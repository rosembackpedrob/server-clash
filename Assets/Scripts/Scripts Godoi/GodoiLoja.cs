using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiLoja : MonoBehaviour
{
    public GameObject Loja;
    void Start()
    {
        Loja.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Loja.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Loja.SetActive(false);
        }
    }
}

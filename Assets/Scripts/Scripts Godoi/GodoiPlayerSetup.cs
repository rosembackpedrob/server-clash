using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GodoiPlayerSetup : MonoBehaviour
{
    PhotonView pV;

    private void Awake()
    {
        pV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (pV.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        Debug.Log("Instanciou o controlador do player");
        PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}

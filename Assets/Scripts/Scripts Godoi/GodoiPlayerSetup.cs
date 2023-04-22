using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GodoiPlayerSetup : MonoBehaviour
{
    PhotonView pV;

    GameObject controller;

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
        Transform spawnpoint = GodoiSpawnManager.instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] {pV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}

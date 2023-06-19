using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GodoiPlayerSetup : MonoBehaviourPun
{
    PhotonView pV;

    GameObject controller;

    public int playerId;

    public Team meuTime;
    private void Awake()
    {
        pV = GetComponent<PhotonView>();
    }
    void Start()
    {
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        meuTime = GodoiTeamManager.GetPlayerTeam(playerId);
        if (pV.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        Transform spawnpoint = GodoiSpawnManager.instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { pV.ViewID });
        //photonView.RPC(nameof(PedirLayer), RpcTarget.MasterClient);
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
    /*[PunRPC]
    void PedirLayer(PhotonMessageInfo mensagem)
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Team playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);
        if (playerTeam == Team.TeamA)
        {
            photonView.RPC(nameof(DefinirLayer), mensagem.Sender, true, mensagem.Sender.ActorNumber);
        }
        else if (playerTeam == Team.TeamB)
        {
            photonView.RPC(nameof(DefinirLayer), mensagem.Sender, false, mensagem.Sender.ActorNumber);
        }
    }
    [PunRPC]
    void DefinirLayer(bool Defensor, int actorNumber)
    {
        if (Defensor)
        {
            Transform spawnpoint = GodoiSpawnManager.instance.GetSpawnPoint();
            controller = PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { actorNumber });
            controller.layer = LayerMask.NameToLayer("Defensores");
        }
        else
        {
            Transform spawnpoint = GodoiSpawnManager.instance.GetSpawnPoint();
            controller = PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { actorNumber });
            controller.layer = LayerMask.NameToLayer("Atacantes");
        }
    }*/
}

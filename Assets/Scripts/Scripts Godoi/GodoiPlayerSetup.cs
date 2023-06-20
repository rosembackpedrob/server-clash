using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GodoiPlayerSetup : MonoBehaviourPun
{
    PhotonView pV;

    GameObject controller;

    public int playerId;

    public Team meuTime;

    public int myPlayerKill;
    public int myPlayerDeath;

    public int dinheiro;
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
        controller.transform.parent = gameObject.transform;
        //controller.GetComponent<GodoiPlayerController>()._kill = dados.kill;
        //controller.GetComponent<GodoiPlayerController>()._Death = dados.death;
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

        myPlayerDeath++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", myPlayerDeath);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void GetKill()
    {
        pV.RPC(nameof(RPC_GetKill), pV.Owner);
    }
    [PunRPC]
    void RPC_GetKill()
    {
        myPlayerKill++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", myPlayerKill);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public static GodoiPlayerSetup Find(Player player)
    {
        return FindObjectsOfType<GodoiPlayerSetup>().SingleOrDefault(x => x.pV.Owner == player);
    }
}

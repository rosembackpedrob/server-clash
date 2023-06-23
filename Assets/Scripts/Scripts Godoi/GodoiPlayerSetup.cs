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

    public GameObject controller;

    public Team meuTime;
    public Personagem meuPersonagem;

    public int myPlayerKill;
    public int myPlayerDeath;

    public int dinheiro;

    public PlacarManager placar;

    private void Awake()
    {
        pV = GetComponent<PhotonView>();
    }
    void Start()
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        meuTime = GodoiTeamManager.GetPlayerTeam(playerId);
        meuPersonagem = CharacterManager.PegarPersonagem(playerId);
        if (pV.IsMine)
        {
            CreateController();
        }
        placar = FindObjectOfType<PlacarManager>();
    }
    void CreateController()
    {
        Transform spawnpoint = GodoiSpawnManager.instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { pV.ViewID });
        controller.transform.parent = gameObject.transform;
    }
    public void EscudoMedio()
    {
        controller.GetComponent<GodoiPlayerController>().currentShield = 25;
        controller.GetComponent<GodoiPlayerController>().AttEscudo();
    }
    public void EscudoGrande()
    {
        controller.GetComponent<GodoiPlayerController>().currentShield = 50;
        controller.GetComponent<GodoiPlayerController>().AttEscudo();
    }
    public void Spawn()
    {
        if (pV.IsMine)
        {
            if (controller != null)
            {
                PhotonNetwork.Destroy(controller);
            }
            CreateController();
        }
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        PlacarManager.instance.ComecarContagem();
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

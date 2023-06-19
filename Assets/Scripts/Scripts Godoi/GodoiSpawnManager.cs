using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiSpawnManager : MonoBehaviourPun
{
    public static GodoiSpawnManager instance;

    GodoiSpawnPoint[] spawnPoints;

    [SerializeField] private GodoiSpawnPoint[] SpawnLocalAtacantes;
    [SerializeField] private GodoiSpawnPoint[] SpawnLocalDefensores;

    public int spawnCountAtacantes = 0;
    public int spawnCountDefensores = 5;
    public bool posicaoOcupada;
    Team timeDoPidao;
    LayerMask layerDoPidao;
    private void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<GodoiSpawnPoint>();
    }
    private void Start()
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Team playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);
        timeDoPidao = playerTeam;
        if (timeDoPidao == Team.TeamA)
        {
            spawnPoints = SpawnLocalDefensores;
        }
        else
        {
            spawnPoints = SpawnLocalAtacantes;
        }
    }
    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}

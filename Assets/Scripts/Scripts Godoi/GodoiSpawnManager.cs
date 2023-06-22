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
    public GodoiPlayerController[] todosPlayerController;

    public List <GodoiPlayerController> playerControllersDefensores = new List<GodoiPlayerController>();
    public List <GodoiPlayerController> playerControllersAtacantes = new List<GodoiPlayerController> ();
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
        StartCoroutine(encontrarObjetos());

    }
    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
    IEnumerator encontrarObjetos()
    {
        yield return new WaitForSeconds(1);
        todosPlayerController = GameObject.FindObjectsOfType<GodoiPlayerController>();
        foreach (GodoiPlayerController playerController in todosPlayerController)
        {
            if (playerController.playerTeam == Team.TeamA)
            {
                playerControllersDefensores.Add(playerController);
            }
            else
            {
                playerControllersAtacantes.Add(playerController);
            }
        }
    }
    public void Update()
    {
        for (int i = 0; i < playerControllersDefensores.Count; i++)
        {

        }
        foreach (GodoiPlayerController playerController in playerControllersDefensores)
        {
            if (playerController == null)
            {

            }
        }
    }
    // se o jogador estiver no timeA ele vai para o array de defensores, ao contrario vai para o de atacantes
}

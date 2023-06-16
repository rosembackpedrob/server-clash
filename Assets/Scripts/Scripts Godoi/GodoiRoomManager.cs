using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class GodoiRoomManager : MonoBehaviourPunCallbacks
{
    public static GodoiRoomManager Instance;
    public GodoiObjetoDeletavel del;
    public int jogadoresNoTimeA;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        del = FindObjectOfType<GodoiObjetoDeletavel>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene _cena, LoadSceneMode _loadSceneMode)
    {
        if (_cena.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
    public void Teste()
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Team playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);
        Debug.Log(playerTeam);
    }
    public void Update()
    {
        
    }
}

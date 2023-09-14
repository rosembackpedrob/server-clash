using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using System.IO;

public class GodoiLauncher : MonoBehaviourPunCallbacks
{
    public static GodoiLauncher Instance;

    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;

    [SerializeField] Transform playerListContent;
    [SerializeField] public Transform playerListTimeA;
    [SerializeField] public Transform playerListTimeB;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] public GameObject playerListDefensores;
    [SerializeField] public GameObject playerListAtacantes;

    [SerializeField] GameObject startGameButton;
    [SerializeField] private int MinimoDePlayers;
    public bool TimeA;
    public bool TimeB;
    public int conttimeA;
    public int conttimeB;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log(message: "Connecting to Server");

        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log(message: "Connected to Server");

        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        GodoiMenuManager.Instance.OpenMenu("title");

        Debug.Log(message: "We're in the lobby");

        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameIF.text);
        GodoiMenuManager.Instance.OpenMenu("loading");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        GodoiMenuManager.Instance.OpenMenu("room");

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        Debug.Log("meu actor number é: " + PhotonNetwork.LocalPlayer.ActorNumber);

        if (PhotonNetwork.IsMasterClient)
        {
            PlayerDataManager.dadosDosPlayers.Clear();
        }

        if (photonView.IsMine)
        {
            Debug.Log("pv é meu");
        }
        photonView.RPC(nameof(RequisicaoDeTime), RpcTarget.MasterClient);

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        Debug.Log(PhotonNetwork.CurrentRoom.Players.Count);
    }
    public void JoinTeam(int team)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
            {
                { "Team", team }
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        errorText.text = "Room Creation Failer: " + message;
        GodoiMenuManager.Instance.OpenMenu("error");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        GodoiMenuManager.Instance.OpenMenu("loading");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);

        GodoiMenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        GodoiMenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<GodoiRoomListButton>().SetUp(roomList[i]); 
        }
    }
    [PunRPC]
    public void RequisicaoDeTime(PhotonMessageInfo mensagem)
    {
        if (conttimeA <= conttimeB)
        {
            // Jogador entra no timeA
            conttimeA++;
            photonView.RPC(nameof(DefinirTime), mensagem.Sender, true, mensagem.Sender.ActorNumber);
            Debug.Log("Entrei no time Defensores");
        }
        else
        {
            // Jogador entra no timeB
            conttimeB++;
            photonView.RPC(nameof(DefinirTime), mensagem.Sender, false, mensagem.Sender.ActorNumber);
            Debug.Log("Entrei no Time Atacantes");
        }
    }
    [PunRPC]
    public void DefinirTime(bool EntrarNoTimeA, int ActorNumber)
    {
        if (EntrarNoTimeA)
        {
            Team teamA = Team.TeamA;
            GodoiTeamManager.AssignPlayerToTeam(PhotonNetwork.LocalPlayer.ActorNumber, teamA);
            PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "ListaDefensores"), Vector3.zero, Quaternion.identity, 0, new object[] { ActorNumber });
        }
        else
        {
            Team teamB = Team.TeamB;
            GodoiTeamManager.AssignPlayerToTeam(PhotonNetwork.LocalPlayer.ActorNumber, teamB);
            PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "ListaAtacantes"), Vector3.zero, Quaternion.identity, 0, new object[] { ActorNumber });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
public class GodoiLauncher : MonoBehaviourPunCallbacks
{
    public static GodoiLauncher Instance;

    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;

    [SerializeField] Transform playerListContent;
    [SerializeField] Transform playerListTimeA;
    [SerializeField] Transform playerListTimeB;
    [SerializeField] GameObject playerListPrefab;

    [SerializeField] GameObject startGameButton;

    [SerializeField] GodoiRoomManager RM;
    public bool TimeA;
    public bool TimeB;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log(message: "Connecting to Server");

        PhotonNetwork.ConnectUsingSettings();

        RM = GameObject.FindObjectOfType<GodoiRoomManager>();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log(message: "Connected to Server");

        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    /*public static Dictionary<int, Team> playerTeams = new Dictionary<int, Team>();

    public static void AssignPlayerToTeam(int playerId, Team team)
    {
        if (!playerTeams.ContainsKey(playerId))
        {
            playerTeams.Add(playerId, team);
        }
        else
        {
            playerTeams[playerId] = team;
        }
    }
    public static Team GetPlayerTeam(int playerId)
    {
        if (playerTeams.ContainsKey(playerId))
        {
            return playerTeams[playerId];
        }
        return Team.TeamA; // Time padrão se o jogador não estiver atribuído a nenhum time
    }*/
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        GodoiMenuManager.Instance.OpenMenu("title");

        Debug.Log(message: "We're in the lobby");

        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameIF.text);
        GodoiMenuManager.Instance.OpenMenu("loading");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        GodoiMenuManager.Instance.OpenMenu("room");

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        /*foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }*/
        if (RM.jogadoresNoTimeA <= 0)
        {
            TimeA = true;
            RM.jogadoresNoTimeA++;
        }
        else TimeB = true;
        foreach (Transform child in playerListTimeA)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerListTimeB)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            if (TimeA == true)
            {
                Instantiate(playerListPrefab, playerListTimeA).GetComponent<GodoiListaNomeJogador>().SetUp(players[i]);
            }
            else if (TimeB == true)
            {
                Instantiate(playerListPrefab, playerListTimeB).GetComponent<GodoiListaNomeJogador>().SetUp(players[i]);
            }
            //Instantiate(playerListPrefab, playerListContent).GetComponent<GodoiListaNomeJogador>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
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
    public void EntrarNoTimeA()
    {
        TimeA = true;
        TimeB = false;
    }
    public void EntrarNoTimeB()
    {
        TimeB = true;
        TimeA = false;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        //Instantiate(playerListPrefab, playerListContent).GetComponent<GodoiListaNomeJogador>().SetUp(newPlayer);

        int playerId = newPlayer.ActorNumber;

        if (playerId <= 0)
        {
            Instantiate(playerListPrefab, playerListTimeA).GetComponent<GodoiListaNomeJogador>().SetUp(newPlayer);
        }
        else Instantiate(playerListPrefab, playerListTimeB).GetComponent<GodoiListaNomeJogador>().SetUp(newPlayer);
        //Team team = Team.TeamA; // Defina o time para o jogador com base na lógica do seu jogo. Trocar para usar Botão
        /*if (TimeA == true || jogadoresNoTimeA <= 5)
        {
            Team team = Team.TeamA;
            GodoiTeamManager.AssignPlayerToTeam(playerId, team);
            jogadoresNoTimeA++;
        }
        else// if (TimeB == true || jogadoresNoTimeB <= 4)
        {
            Team team = Team.TeamB;
            GodoiTeamManager.AssignPlayerToTeam(playerId, team);
            jogadoresNoTimeB++;
        }*/
    }
}

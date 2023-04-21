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
    [SerializeField] GameObject playerListPrefab;

    [SerializeField] GameObject startGameButton;

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

        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<GodoiListaNomeJogador>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Instantiate(playerListPrefab, playerListContent).GetComponent<GodoiListaNomeJogador>().SetUp(newPlayer);
    }
}

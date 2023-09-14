using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text KillText;
    public TMP_Text DeathText;

    Player player;

    Team playerTeam;
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);
        UpdateStats();
    }
    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            KillText.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            DeathText.text = deaths.ToString();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }
}

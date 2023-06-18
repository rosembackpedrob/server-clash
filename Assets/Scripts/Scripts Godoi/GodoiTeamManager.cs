using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum Team
{
    TeamA,
    TeamB
}
public class GodoiTeamManager
{
    public static Dictionary<int, Team> playerTeams = new Dictionary<int, Team>();
    public static Dictionary<int, LayerMask> playerLayer = new Dictionary<int, LayerMask>();
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
    }
    public static void AssignPlayerLayer(int playerId, LayerMask teamLayer)
    {
        if (!playerLayer.ContainsKey(playerId))
        {
            playerLayer.Add(playerId, teamLayer);
        }
        else
        {
            playerLayer[playerId] = teamLayer;
        }
    }
    public static LayerMask GetPlayerLayer(int playerId)
    {
        if (playerLayer.ContainsKey(playerId))
        {
            return playerLayer[playerId];
        }
        return LayerMask.NameToLayer("Defensores"); // Layer padrão se o jogador não estiver atribuído a nenhum time
    }
}

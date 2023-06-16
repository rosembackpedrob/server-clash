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
        return Team.TeamA; // Time padr�o se o jogador n�o estiver atribu�do a nenhum time
    }
}
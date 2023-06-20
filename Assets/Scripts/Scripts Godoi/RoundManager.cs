using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosEmPartida
{
    public int actorId;
    public int kill;
    public int death;
}
public class RoundManager
{
    public static Dictionary<int, DadosEmPartida> dadosDosPlayersEmPartida = new Dictionary<int, DadosEmPartida>();

    public static void SetPlayerKDA(int playerID, DadosEmPartida dados)
    {
        if (!dadosDosPlayersEmPartida.ContainsKey(playerID))
        {
            dadosDosPlayersEmPartida.Add(playerID, dados);
        }
        else
        {
            dadosDosPlayersEmPartida[playerID] = dados;
        }
    }
    public static DadosEmPartida GetPlayerKDA(int playerID)
    {
        if (dadosDosPlayersEmPartida.ContainsKey(playerID))
        {
            return dadosDosPlayersEmPartida[playerID];
        }
        return dadosDosPlayersEmPartida[playerID];
    }
}

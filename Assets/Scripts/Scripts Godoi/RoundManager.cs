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
}

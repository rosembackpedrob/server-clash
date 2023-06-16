using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Times
{
    A, B
}
public class PlayerData 
{
    public int actorID;
    public bool timeA;
}
public static class PlayerDataManager
{
    public static Dictionary<int, PlayerData > dadosDosPlayers = new Dictionary<int, PlayerData> ();
}

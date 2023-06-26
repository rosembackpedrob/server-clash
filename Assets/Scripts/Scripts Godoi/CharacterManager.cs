using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Personagem
{
    Cria = 0,
    Sueli = 1,
    Espectro = 2
}
public class CharacterManager
{
    public static Dictionary<int, Personagem> Personagens = new Dictionary<int, Personagem>();
    public static void DefinirPersonagem(int playerId, Personagem personagem)
    {
        if (!Personagens.ContainsKey(playerId))
        {
            Personagens.Add(playerId, personagem);
        }
        else
        {
            Personagens[playerId] = personagem;
        }
    }
    public static Personagem PegarPersonagem(int playerId)
    {
        if (Personagens.ContainsKey(playerId))
        {
            return Personagens[playerId];
        }
        return Personagem.Cria;
    }
}

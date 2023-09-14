using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GodoiListaNomeJogador : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text characterNameText;

    [SerializeField] Image imagemDosPersonagens;
    [SerializeField] Sprite[] spriteDosPersonagens;
    Player player;

    public int personagemAtual = 1;
    public void SetUp(Player _player)
    {
        player = _player;
        playerNameText.text = _player.NickName;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Verifique se as propriedades personalizadas do jogador foram atualizadas
        if (targetPlayer == photonView.Owner)
        {
            // Verifique se a propriedade "personagemAtual" foi alterada
            if (changedProps.ContainsKey("personagemAtual"))
            {
                // Atualize o personagem exibido no jogador local
                personagemAtual = (int)changedProps["personagemAtual"];
                AtualizarPersonagem(personagemAtual);
            }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        Destroy(gameObject);
    }
    public void TrocarPersonagem()
    {
        if (photonView.IsMine)
        {
            personagemAtual++;
            if (personagemAtual > 3)
            {
                personagemAtual = 1;
            }
            Hashtable hash = new Hashtable();
            hash.Add("personagemAtual", personagemAtual);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        Debug.Log(personagemAtual);
    }
    private void Start()
    {
        Hashtable hash = new Hashtable();
        hash.Add("personagemAtual", 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    private void AtualizarPersonagem(int personagem)
    {
        // Faça as alterações necessárias no jogador com base no personagemAtual
        characterNameText.text = GetCharacterName(personagem);
        imagemDosPersonagens.sprite = spriteDosPersonagens[personagem];
    }
    private string GetCharacterName(int personagem)
    {
        // Retorne o nome do personagem com base no ID do personagem
        switch (personagem)
        {
            case 1:
                return "C.R.I.A.";
            case 2:
                return "Infiltrada";
            case 3:
                return "Espectro";
            default:
                return "";
        }
    }
}

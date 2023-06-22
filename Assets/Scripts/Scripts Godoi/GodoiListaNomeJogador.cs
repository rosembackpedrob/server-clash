using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GodoiListaNomeJogador : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text characterNameText;

    [SerializeField] Image imagemDosPersonagens;
    [SerializeField] Sprite[] spriteDosPersonagens;
    Player player;

    public bool baseMesh;
    public bool cria;
    public bool sueli;
    public bool espectro;

    public int personagemAtual = 0;
    public void SetUp(Player _player)
    {
        player = _player;
        playerNameText.text = _player.NickName;
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
        baseMesh = false;
        cria = false;
        sueli = false;
        espectro = false;
        personagemAtual++;
        if (personagemAtual == 4)
        {
            personagemAtual = 1;
            cria = true;
            characterNameText.text = "C.R.I.A.";
        }
        else if (personagemAtual == 0)
        {
            baseMesh = true;
            characterNameText.text = "";
        }
        else if (personagemAtual == 1)
        {
            cria = true;
            characterNameText.text = "C.R.I.A.";
        }
        else if (personagemAtual == 2)
        {
            sueli = true;
            characterNameText.text = "Sueli";
        }
        else if (personagemAtual == 3)
        {
            espectro = true;
            characterNameText.text = "Espectro";
        }
        imagemDosPersonagens.sprite = spriteDosPersonagens[personagemAtual];
    }
}

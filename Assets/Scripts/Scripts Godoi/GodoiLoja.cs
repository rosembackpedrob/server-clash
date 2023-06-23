using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GodoiLoja : MonoBehaviourPunCallbacks
{
    PhotonView pV;
    public GameObject Loja;
    public int FuzilPre�o;
    public int PistolaPre�o;
    public int RiflePre�o;
    public int EspingardaPre�o;
    public int MeioEscudoPre�o;
    public int EscudoCheioPre�o;
    public int Habilidade1Pre�o;
    public int Habilidade2Pre�o;

    public int tempoDeCompra;
    GodoiPlayerSetup playerSetup;

    int escudo;

    [SerializeField] TMP_Text playerDinheiro;
    [SerializeField] TMP_Text[] TextosDePre�o;
    void Start()
    {
        Cursor.visible = false;
        Loja.SetActive(false);
        playerSetup = gameObject.transform.GetParentComponent<GodoiPlayerSetup>();

        playerDinheiro.text = (playerSetup.dinheiro.ToString());
        TextosDePre�o[0].text = (FuzilPre�o.ToString());
        TextosDePre�o[1].text = (PistolaPre�o.ToString());
        TextosDePre�o[2].text = (RiflePre�o.ToString());
        TextosDePre�o[3].text = (EspingardaPre�o.ToString());
        TextosDePre�o[4].text = (MeioEscudoPre�o.ToString());
        TextosDePre�o[5].text = (EscudoCheioPre�o.ToString());
        TextosDePre�o[6].text = (Habilidade1Pre�o.ToString());
        TextosDePre�o[7].text = (Habilidade2Pre�o.ToString());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AbrirLoja();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Loja.SetActive(false);
        }
    }
    public void AbrirLoja()
    {
        if (PlacarManager.instance.tempoDePartidaAtual >= PlacarManager.instance.tempoDePartida - tempoDeCompra)
        {
            Loja.SetActive(!Loja.activeSelf);
            Cursor.visible = (Loja.activeSelf);
        }
    }
    public void OnFuzilClick()
    {
        if (playerSetup.dinheiro >= FuzilPre�o)
        {
            playerSetup.dinheiro -= FuzilPre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnPistolaClick()
    {
        if (playerSetup.dinheiro >= PistolaPre�o)
        {
            playerSetup.dinheiro -= PistolaPre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnRifleClick()
    {
        if (playerSetup.dinheiro >= RiflePre�o)
        {
            playerSetup.dinheiro -= RiflePre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnEspingardaClick()
    {
        if (playerSetup.dinheiro >= EspingardaPre�o)
        {
            playerSetup.dinheiro -= EspingardaPre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnMeioEscudoClick()
    {
        if (escudo < 25)
        {
            if (playerSetup.dinheiro >= MeioEscudoPre�o)
            {
                playerSetup.dinheiro -= MeioEscudoPre�o;
                playerSetup.EscudoMedio();
                escudo = 25;
            }
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnEscudoCheioClick()
    {
        if (escudo < 50)
        {
            if (playerSetup.dinheiro >= EscudoCheioPre�o)
            {
                playerSetup.dinheiro -= EscudoCheioPre�o;
                playerSetup.EscudoGrande();
                escudo = 50;
            }
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnHabilidade1Click()
    {
        if (playerSetup.dinheiro >= Habilidade1Pre�o)
        {
            playerSetup.dinheiro -= Habilidade1Pre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnHabilidade2Click()
    {
        if (playerSetup.dinheiro >= Habilidade2Pre�o)
        {
            playerSetup.dinheiro -= Habilidade2Pre�o;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
}

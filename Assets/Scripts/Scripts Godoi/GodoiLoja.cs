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
    public int FuzilPreço;
    public int PistolaPreço;
    public int RiflePreço;
    public int EspingardaPreço;
    public int MeioEscudoPreço;
    public int EscudoCheioPreço;
    public int Habilidade1Preço;
    public int Habilidade2Preço;

    public int tempoDeCompra;
    GodoiPlayerSetup playerSetup;
    GodoiPlayerController playerController;

    int escudo;

    [SerializeField] TMP_Text playerDinheiro;
    [SerializeField] TMP_Text[] TextosDePreço;

    public bool fuzilComprado;
    public bool EspingardaComprado;
    public bool rifleComprado;
    public bool pistolaComprado;
    public bool facaComprado;
    void Start()
    {
        pistolaComprado = true;
        facaComprado = true;
        fuzilComprado = true;
        Cursor.visible = false;
        Loja.SetActive(false);
        playerSetup = gameObject.transform.GetParentComponent<GodoiPlayerSetup>();
        playerController = gameObject.transform.GetParentComponent<GodoiPlayerController>();
        playerController.AtualizarEquipamento();

        playerDinheiro.text = (playerSetup.dinheiro.ToString());
        TextosDePreço[0].text = (FuzilPreço.ToString());
        TextosDePreço[1].text = (PistolaPreço.ToString());
        TextosDePreço[2].text = (RiflePreço.ToString());
        TextosDePreço[3].text = (EspingardaPreço.ToString());
        TextosDePreço[4].text = (MeioEscudoPreço.ToString());
        TextosDePreço[5].text = (EscudoCheioPreço.ToString());
        TextosDePreço[6].text = (Habilidade1Preço.ToString());
        TextosDePreço[7].text = (Habilidade2Preço.ToString());
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
        if (playerSetup.dinheiro >= FuzilPreço && fuzilComprado == false)
        {
            if (EspingardaComprado)
            {
                playerSetup.dinheiro += EspingardaPreço;
                fuzilComprado = true;
                EspingardaComprado = false;
                playerController.AtualizarEquipamento();
            }
            else if (rifleComprado)
            {
                playerSetup.dinheiro += RiflePreço;
                fuzilComprado = true;
                rifleComprado = false;
                playerController.AtualizarEquipamento();
            }
            playerSetup.dinheiro -= FuzilPreço;
            fuzilComprado = true;
            playerController.AtualizarEquipamento();
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnPistolaClick()
    {
        if (playerSetup.dinheiro >= PistolaPreço && pistolaComprado == false)
        {
            playerSetup.dinheiro -= PistolaPreço;
            pistolaComprado = true;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnRifleClick()
    {
        if (playerSetup.dinheiro >= RiflePreço && rifleComprado == false)
        {
            if (EspingardaComprado)
            {
                playerSetup.dinheiro += EspingardaPreço;
                rifleComprado = true;
                EspingardaComprado = false;
                playerController.AtualizarEquipamento();
            }
            else if (fuzilComprado)
            {
                playerSetup.dinheiro += FuzilPreço;
                rifleComprado = true;
                fuzilComprado = false;
                playerController.AtualizarEquipamento();
            }
            playerSetup.dinheiro -= RiflePreço;
            rifleComprado = true;
            playerController.AtualizarEquipamento();
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnEspingardaClick()
    {
        if (playerSetup.dinheiro >= EspingardaPreço && EspingardaComprado == false)
        {
            if (fuzilComprado)
            {
                playerSetup.dinheiro -= FuzilPreço;
                EspingardaComprado = true;
                fuzilComprado = false;
                playerController.AtualizarEquipamento();
            }
            else if (rifleComprado)
            {
                playerSetup.dinheiro -= RiflePreço;
                EspingardaComprado = true;
                rifleComprado = false;
                playerController.AtualizarEquipamento();
            }
            playerSetup.dinheiro -= EspingardaPreço;
            EspingardaComprado = true;
            playerController.AtualizarEquipamento();
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnMeioEscudoClick()
    {
        if (escudo < 25)
        {
            if (playerSetup.dinheiro >= MeioEscudoPreço)
            {
                playerSetup.dinheiro -= MeioEscudoPreço;
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
            if (playerSetup.dinheiro >= EscudoCheioPreço)
            {
                playerSetup.dinheiro -= EscudoCheioPreço;
                playerSetup.EscudoGrande();
                escudo = 50;
            }
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnHabilidade1Click()
    {
        if (playerSetup.dinheiro >= Habilidade1Preço)
        {
            playerSetup.dinheiro -= Habilidade1Preço;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
    public void OnHabilidade2Click()
    {
        if (playerSetup.dinheiro >= Habilidade2Preço)
        {
            playerSetup.dinheiro -= Habilidade2Preço;
        }
        playerDinheiro.text = (playerSetup.dinheiro.ToString());
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GodoiPlayerSetup playerSetup;
    void Start()
    {
        Loja.SetActive(false);
        playerSetup = gameObject.transform.GetParentComponent<GodoiPlayerSetup>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Loja.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Loja.SetActive(false);
        }
    }
    public void OnFuzilClick()
    {
        if (playerSetup.dinheiro >= FuzilPreço)
        {
            playerSetup.dinheiro -= FuzilPreço;
        }
    }
    public void OnPistolaClick()
    {
        if (playerSetup.dinheiro >= PistolaPreço)
        {
            playerSetup.dinheiro -= PistolaPreço;
        }
    }
    public void OnRifleClick()
    {
        if (playerSetup.dinheiro >= RiflePreço)
        {
            playerSetup.dinheiro -= RiflePreço;
        }
    }
    public void OnEspingardaClick()
    {
        if (playerSetup.dinheiro >= EspingardaPreço)
        {
            playerSetup.dinheiro -= EspingardaPreço;
        }
    }
    public void OnMeioEscudoClick()
    {
        if (playerSetup.dinheiro >= MeioEscudoPreço)
        {
            playerSetup.dinheiro -= MeioEscudoPreço;
        }
    }
    public void OnEscudoCheioClick()
    {
        if (playerSetup.dinheiro >= EscudoCheioPreço)
        {
            playerSetup.dinheiro -= EscudoCheioPreço;
        }
    }
    public void OnHabilidade1Click()
    {
        if (playerSetup.dinheiro >= Habilidade1Preço)
        {
            playerSetup.dinheiro -= Habilidade1Preço;
        }
    }
    public void OnHabilidade2Click()
    {
        if (playerSetup.dinheiro >= Habilidade2Preço)
        {
            playerSetup.dinheiro -= Habilidade2Preço;
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (playerSetup.dinheiro >= FuzilPre�o)
        {
            playerSetup.dinheiro -= FuzilPre�o;
        }
    }
    public void OnPistolaClick()
    {
        if (playerSetup.dinheiro >= PistolaPre�o)
        {
            playerSetup.dinheiro -= PistolaPre�o;
        }
    }
    public void OnRifleClick()
    {
        if (playerSetup.dinheiro >= RiflePre�o)
        {
            playerSetup.dinheiro -= RiflePre�o;
        }
    }
    public void OnEspingardaClick()
    {
        if (playerSetup.dinheiro >= EspingardaPre�o)
        {
            playerSetup.dinheiro -= EspingardaPre�o;
        }
    }
    public void OnMeioEscudoClick()
    {
        if (playerSetup.dinheiro >= MeioEscudoPre�o)
        {
            playerSetup.dinheiro -= MeioEscudoPre�o;
        }
    }
    public void OnEscudoCheioClick()
    {
        if (playerSetup.dinheiro >= EscudoCheioPre�o)
        {
            playerSetup.dinheiro -= EscudoCheioPre�o;
        }
    }
    public void OnHabilidade1Click()
    {
        if (playerSetup.dinheiro >= Habilidade1Pre�o)
        {
            playerSetup.dinheiro -= Habilidade1Pre�o;
        }
    }
    public void OnHabilidade2Click()
    {
        if (playerSetup.dinheiro >= Habilidade2Pre�o)
        {
            playerSetup.dinheiro -= Habilidade2Pre�o;
        }
    }
}

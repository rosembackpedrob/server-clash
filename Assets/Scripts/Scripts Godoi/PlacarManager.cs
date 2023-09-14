using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlacarManager : MonoBehaviour
{
    public static PlacarManager instance;
    public int contagemDefensores;
    public  int contagemAtacantes;

    public int vitoriaDosDefensores;
    public int vitoriaDosAtacantes;

    public float tempoDePartida;

    public float tempoDePartidaAtual;

    PhotonView pV;

    public GodoiPlayerController[] todosPlayerController;

    public GodoiPlayerSetup[] todosPlayerSetup;

    [SerializeField] Image[] pontosVitoriaDefensor;
    [SerializeField] Image[] pontosVitoriaAtacantes;

    [SerializeField] Sprite pontoVotoria;
    [SerializeField] Sprite pontoNormal;

    [SerializeField] TMP_Text[] timer;
    [SerializeField] Image BarraDeTempo;

    [SerializeField] GameObject VitDefensores;
    [SerializeField] GameObject VitAtacantes;

    [SerializeField] GameObject[] prePartida;
    void Start()
    {
        instance = this;
        pV = GetComponent<PhotonView>();
        vitoriaDosAtacantes = 0;
        vitoriaDosDefensores = 0;
        ResetTempo();
    }
    public void ComecarContagem()
    {
        pV.RPC(nameof(FazerContagem), RpcTarget.MasterClient);
    }
    public void Update()
    {
        if (tempoDePartidaAtual > 0)
        {
            tempoDePartidaAtual -= Time.deltaTime;
            UpdateTempoDisplay(tempoDePartidaAtual);
            pV.RPC(nameof(UpdateTempoDisplay), RpcTarget.All, tempoDePartidaAtual);
        }
        else if (tempoDePartidaAtual != 0)
        {
            tempoDePartidaAtual = 0;
            UpdateTempoDisplay(tempoDePartidaAtual);
            pV.RPC(nameof(UpdateTempoDisplay), RpcTarget.All, tempoDePartidaAtual);
            pV.RPC(nameof(FazerContagem), RpcTarget.All);
        }
    }
    [PunRPC]
    public void FazerContagem()
    {
        todosPlayerController = GameObject.FindObjectsOfType<GodoiPlayerController>();
        contagemDefensores = 0;
        contagemAtacantes = 0;
        foreach (GodoiPlayerController playerController in todosPlayerController)
        {
            if (playerController.playerTeam == Team.TeamA)
            {
                contagemDefensores++;
            }
            else
            {
                contagemAtacantes++;
            }
        }
        if (contagemDefensores == 0)
        {
            vitoriaDosAtacantes++;
            pV.RPC(nameof(SincTelaDeVitoria), RpcTarget.All, false);
            AudioManageeer.instance.PlaySoundEffect(AudioManageeer.instance.audiosPlacar[1]);
        }
        else if (contagemAtacantes == 0 || tempoDePartidaAtual <= 0)
        {
            vitoriaDosDefensores++;
            pV.RPC(nameof(SincTelaDeVitoria), RpcTarget.All, true);
            AudioManageeer.instance.PlaySoundEffect(AudioManageeer.instance.audiosPlacar[0]);
        }
        if (vitoriaDosDefensores == 5)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
            AudioManageeer.instance.PlaySoundEffect(AudioManageeer.instance.audiosPlacar[2]);
        }
        if (vitoriaDosAtacantes == 5)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
            AudioManageeer.instance.PlaySoundEffect(AudioManageeer.instance.audiosPlacar[3]);
        }
    }
    [PunRPC]
    void SincTelaDeVitoria(bool Defensores)
    {
        if (Defensores)
        {
            VitDefensores.SetActive(true);
            StartCoroutine(TelaDeVitoria());
        }
        else
        {
            VitAtacantes.SetActive(true);
            StartCoroutine(TelaDeVitoria());
        }
    }
    public IEnumerator TelaDeVitoria()
    {
        yield return new WaitForSeconds(3);
        ResetTempo();
        pV.RPC(nameof(RespawnaTodoMundo), RpcTarget.All, vitoriaDosDefensores, vitoriaDosAtacantes);
        VitAtacantes.SetActive(false);
        VitDefensores.SetActive(false);
    }
    [PunRPC]
    public void RespawnaTodoMundo(int VitDef, int VitAtc)
    {
        todosPlayerSetup = GameObject.FindObjectsOfType<GodoiPlayerSetup>();
        foreach (GodoiPlayerSetup playerSetup in todosPlayerSetup)
        {
            playerSetup.Spawn();
        }
        for (int i = 0; i < VitDef; i++)
        {
            pontosVitoriaDefensor[i].sprite = pontoVotoria;
        }
        for (int i = 0; i < VitAtc; i++)
        {
            pontosVitoriaAtacantes[i].sprite = pontoVotoria;
        }
    }
    public void ResetTempo()
    {
        tempoDePartidaAtual = tempoDePartida;
    }
    [PunRPC]
    public void UpdateTempoDisplay(float tempo)
    {
        float minutos = Mathf.FloorToInt(tempo / 60);
        float segundos = Mathf.FloorToInt(tempo % 60);

        string tempoAtual = string.Format("{00:00}{1:00}", minutos, segundos);

        timer[0].text = tempoAtual[0].ToString();
        timer[1].text = tempoAtual[1].ToString();
        timer[3].text = tempoAtual[2].ToString();
        timer[4].text = tempoAtual[3].ToString();

        BarraDeTempo.fillAmount = tempo / tempoDePartida;
    }
}

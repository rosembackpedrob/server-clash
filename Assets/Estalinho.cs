using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Estalinho : MonoBehaviour
{
    public float timerDuration = 30f;

    private bool isActivated = false;
    private float currentTimer = 0f;

    [SerializeField] AudioClip[] bombaSom;
    private void OnTriggerEnter(Collider other)
    {
        int playerId = other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber;
        Team playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);

        if (!isActivated && playerTeam == Team.TeamB)
        {
            StartTimer();
        }
        else if (isActivated && playerTeam == Team.TeamA)
        {
            PauseTimer();
        }
    }

    private void PauseTimer()
    {
        isActivated = false;
        currentTimer = 0f;
        AudioManageeer.instance.PlaySoundEffect(bombaSom[0]);
    }

    private void Update()
    {
        if (isActivated)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                FinishTimer(true);
            }

            Debug.Log("Timer: " + currentTimer.ToString("F1") + " seconds");
        }
    }

    private void StartTimer()
    {
        isActivated = true;
        currentTimer = timerDuration;
        AudioManageeer.instance.PlaySoundEffect(bombaSom[1]);
    }

    private void FinishTimer(bool atacante)
    {
        PlacarManager placarManager = PlacarManager.instance;
        if (atacante)
        {
            placarManager.vitoriaDosAtacantes++;
        }
        else
        {
            placarManager.vitoriaDosDefensores++;
        }
        currentTimer = 10f;
        placarManager.StartCoroutine(placarManager.TelaDeVitoria());
    }
}
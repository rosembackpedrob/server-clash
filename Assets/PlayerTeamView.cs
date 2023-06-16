using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamView : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public bool TeamA;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        Debug.Log(instantiationData.Length);
        
        PlayerData data = new PlayerData();
        data.actorID = (int)instantiationData[0];
        Debug.Log(data.actorID = (int)instantiationData[0]);

        if (TeamA == true)
        {
            data.timeA = TeamA;
            transform.parent = GodoiLauncher.Instance.playerListTimeA.transform;
        }
        else
        {
            data.timeA = TeamA;
            transform.parent = GodoiLauncher.Instance.playerListTimeB.transform;
        }
        PlayerDataManager.dadosDosPlayers.Add(data.actorID, data);
        
    }
}

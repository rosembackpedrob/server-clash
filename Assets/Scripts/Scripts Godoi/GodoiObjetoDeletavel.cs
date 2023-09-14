using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiObjetoDeletavel : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        // Verifica se o objeto deve ser exclu�do
        if (!pv.IsMine)
        {
            // Desabilita o objeto se n�o for do jogador local
            gameObject.SetActive(false);
        }
    }

    public void DeleteObject()
    {
        // Verifica se o objeto pertence ao jogador local
        if (pv.IsMine)
        {
            // Destroi o objeto localmente
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            // Solicita ao jogador que possui o objeto para destru�-lo
            pv.RPC("DestroyObject", pv.Owner);
        }
    }

    [PunRPC]
    private void DestroyObject()
    {
        // Destroi o objeto nos jogadores que o possuem
        PhotonNetwork.Destroy(gameObject);
    }
}

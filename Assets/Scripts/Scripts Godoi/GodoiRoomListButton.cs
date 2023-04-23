using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class GodoiRoomListButton : MonoBehaviour
{
    [SerializeField] TMP_Text nomeSalaText;

    public RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        nomeSalaText.text = info.Name;
    }
    public void OnClick()
    {
        GodoiLauncher.Instance.JoinRoom(info);
    }
}

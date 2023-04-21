using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class GodoiRoomManager : MonoBehaviourPunCallbacks
{
    public static GodoiRoomManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene _cena, LoadSceneMode _loadSceneMode)
    {
        if (_cena.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("GodoiPhotonResources", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
}

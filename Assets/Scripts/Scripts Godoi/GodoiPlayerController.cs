using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GodoiPlayerController : MonoBehaviourPunCallbacks, GodoiIDameagable
{
    [SerializeField] Image healthBarImage;
    [SerializeField] Image ShieldBarImage;
    [SerializeField] GameObject ui;

    [SerializeField] GameObject cameraHolder;

    [SerializeField] public GameObject[] personagens3D;
    [SerializeField] public GameObject[] Modelo3D;
    [SerializeField] public GameObject[] Modelo3DAzul;

    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;

    [SerializeField] GameObject[] maoPersonagens;
    [SerializeField] GodoiItem[] itens;
    [SerializeField] GodoiItem pistola;
    [SerializeField] GodoiItem fuzil;
    [SerializeField] GodoiItem espingarda;
    [SerializeField] GodoiItem rifle;
    [SerializeField] GodoiItem faca;
    [SerializeField] Animator[] anim;

    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView pV;

    public int _kill;
    public int _Death;
    const float maxHealth = 150f;
    public float currentHealth = maxHealth;
    const float maxShield = 50f;
    public float currentShield = 0;

    GodoiPlayerSetup playerManager;
    public Team playerTeam;
    public Personagem playerPersonagem;

    public GodoiLoja loja;

    [SerializeField] TMP_Text dinheiroTexto;

    public bool Cria123;
    public bool Suellen;
    public bool expectopatrono;

    int personagemAtual;

    public GodoiPlayerController[] listaDeControllers;

    [SerializeField] AudioClip[] audios;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)pV.InstantiationData[0]).GetComponent<GodoiPlayerSetup>();
    }
    private void Start()
    {
        if (pV.IsMine)
        {
            int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
            playerTeam = GodoiTeamManager.GetPlayerTeam(playerId);
            photonView.RPC(nameof(EspalhaMeuTime), RpcTarget.All, playerTeam);
            loja = gameObject.transform.GetComponentInChildren<GodoiLoja>();
            playerPersonagem = CharacterManager.PegarPersonagem(playerId);

            personagemAtual = (int)PhotonNetwork.LocalPlayer.CustomProperties["personagemAtual"];
            photonView.RPC(nameof(TrocarModelo), RpcTarget.AllBufferedViaServer, personagemAtual - 1);

            Modelo3D[personagemAtual - 1].SetActive(false);
            LockRoom();
            listaDeControllers = GameObject.FindObjectsOfType<GodoiPlayerController>();
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }
    private void LockRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = false;    // Set the room to not be joinable
            roomOptions.IsVisible = false; // Set the room to not be visible in the lobby

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions.CustomRoomProperties); // Apply the custom room properties

            // Update the room properties for other players
            PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[] { "IsOpen", "IsVisible" });

            Debug.Log("Room locked");

            Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers + " " + PhotonNetwork.CurrentRoom.IsVisible + " " + PhotonNetwork.CurrentRoom.IsOpen);
        }
    }
    [PunRPC]
    public void TrocarModelo(int personagemAtual)
    {
        switch (personagemAtual)
        {
            case 0:
                personagens3D[0].SetActive(true);
                break;
            case 1:
                personagens3D[1].SetActive(true);
                break;
            case 2:
                personagens3D[2].SetActive(true);
                break;
        }
    }
    void SetlayerDefensores()
    {
        int layer = 6;
        gameObject.layer = layer;
    }
    void SetLayerAtacantes()
    {
        int layer = 7;
        gameObject.layer = layer;
    }
    public void AtualizarEquipamento()
    {
        itens = new GodoiItem[3];
        Debug.Log("O numero do meu personagem é: " + (personagemAtual - 1));
        if (loja.facaComprado) itens[2] = faca;
        if (loja.pistolaComprado) itens[0] = pistola;
        if (loja.EspingardaComprado)
        {
            itens[1] = espingarda;
        }
        if (loja.rifleComprado)
        {
            itens[1] = rifle; 
        }
        if (loja.fuzilComprado)
        {
            itens[1] = fuzil;
        }
        /*if (personagemAtual - 1 == 0)
        {
            itens[0].transform.parent = maoPersonagens[0].transform;
            itens[1].transform.parent = maoPersonagens[0].transform;
            itens[2].transform.parent = maoPersonagens[0].transform;
        }
        else if (personagemAtual - 1 == 1)
        {
            itens[0].transform.parent = maoPersonagens[1].transform;
            itens[1].transform.parent = maoPersonagens[1].transform;
            itens[2].transform.parent = maoPersonagens[1].transform;
        }
        else if (personagemAtual - 1 == 2)
        {
            itens[0].transform.parent = maoPersonagens[2].transform;
            itens[1].transform.parent = maoPersonagens[2].transform;
            itens[2].transform.parent = maoPersonagens[2].transform;
        }*/
        EquipItem(0);
    }
    private void Update()
    {
        if (!pV.IsMine)
        {
            return;
        }

        dinheiroTexto.text = playerManager.dinheiro.ToString();

        if (loja.Loja.activeSelf == true)
        {
            return;
        }
        MouseLook();
        Movimento();
        Pulo();
        for (int i = 0; i < itens.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= itens.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(itens.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(itens[itemIndex]);
            if (itens[itemIndex] == fuzil)
            {
                AudioManager.instance.Play("FuzilTiro");
                //AudioManageeer.instance.PlaySoundEffect(audios[0]);
            }
            else if (itens[itemIndex] == pistola)
            {
                AudioManager.instance.Play("FuzilTiro");
                //AudioManageeer.instance.PlaySoundEffect(audios[1]);
            }
            else if (itens[itemIndex] == faca)
            {
                AudioManager.instance.Play("FuzilTiro");
                //AudioManageeer.instance.PlaySoundEffect(audios[2]);
            }
            else if (itens[itemIndex] == espingarda)
            {
                AudioManager.instance.Play("FuzilTiro");
                //AudioManageeer.instance.PlaySoundEffect(audios[3]);
            }
            else if (itens[itemIndex] == rifle)
            {
                AudioManager.instance.Play("FuzilTiro");
                //AudioManageeer.instance.PlaySoundEffect(audios[4]);
            }
            itens[itemIndex].Use();
        }
        if (transform.position.y < -10f)
        {
            Die();
        }
    }
    void MouseLook()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -50f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        Cursor.lockState = CursorLockMode.Confined;
    }
    void Movimento()
    {
        /*if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            anim[personagemAtual - 1].SetTrigger("Correndo");
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") != 0)
        {
            anim[personagemAtual - 1].SetTrigger("Correndo");
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            anim[personagemAtual - 1].SetTrigger("Andando");
        } 
        else
        {
            anim[personagemAtual - 1].SetTrigger("Parado");
        }*/
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }
    void Pulo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
        {
            return;
        }
        itemIndex = _index;

        itens[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            Debug.Log(itens[previousItemIndex]);
            itens[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (pV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        if (itens[0].tag == "Pistola")
        {
            anim[personagemAtual - 1].SetBool("Rifle", false);
        }
        if (itens[itemIndex] == fuzil)
        {
            anim[personagemAtual - 1].SetBool("Rifle", true);
        }
        if (itens[2].tag == "Pistola")
        {
            anim[personagemAtual - 1].SetBool("Rifle", false);
        }
        if (itens[1].tag == "Fuzil")
        {
            anim[personagemAtual - 1].SetBool("Rifle", true);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        try
        {
            if (changedProps.ContainsKey("itemIndex") && !pV.IsMine && targetPlayer == pV.Owner)
            {
                EquipItem((int)changedProps["itemIndex"]);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            //throw;
        }
    }
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }
    private void FixedUpdate()
    {
        if (!pV.IsMine)
        {
            return;
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    public void TakeDamage(float damage)
    {
        pV.RPC(nameof(RPC_TakeDamage), pV.Owner, damage);
    }
    public void AttEscudo()
    {
        ShieldBarImage.fillAmount = currentShield / maxShield;
    }
    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        if (currentShield > 0)
        {
            currentHealth -= damage;
            ShieldBarImage.fillAmount = currentShield / maxShield;
        }
        else
        {
            currentHealth -= damage;

            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
        if (currentHealth <= 0)
        {
            Die();
            GodoiPlayerSetup.Find(info.Sender).GetKill();
        }
    }
    void Die()
    {
        playerManager.Die();
    }
    [PunRPC]
    void EspalhaMeuTime(Team time)
    {
        playerTeam = time;
    }
}

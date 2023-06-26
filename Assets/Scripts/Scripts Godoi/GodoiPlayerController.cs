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

    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;

    [SerializeField] GodoiItem[] itens;
    [SerializeField] GodoiItem pistola;
    [SerializeField] GodoiItem fuzil;
    [SerializeField] GodoiItem espingarda;
    [SerializeField] GodoiItem rifle;
    [SerializeField] GodoiItem faca;

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
    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    const float maxShield = 50f;
    public float currentShield = 0;

    GodoiPlayerSetup playerManager;
    public Team playerTeam;
    public Personagem playerPersonagem;

    public GodoiLoja loja;

    [SerializeField] GodoiSingleShotGun[] Armas;

    [SerializeField] TMP_Text dinheiroTexto;

    public bool Cria123;
    public bool Suellen;
    public bool expectopatrono;

    int personagemAtual;
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
            photonView.RPC(nameof(TrocarModelo), RpcTarget.All, personagemAtual - 1);

            Modelo3D[personagemAtual - 1].SetActive(false);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
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
            itens[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (pV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

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

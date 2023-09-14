using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GodoiSingleShotGun : GodoiGun
{
    [SerializeField] Camera cam;

    PhotonView pv;

    LayerMask meuLayer;

    [SerializeField]GameObject myPlayerController;

    public GodoiPlayerSetup myPlayerSetup;

    public int municao;
    public int municaoReserva;
    public float distanciaFaca;
    [SerializeField] TMP_Text MunicaoNaArmaTexto;
    [SerializeField] TMP_Text Barra;
    [SerializeField] TMP_Text MunicaoReservaTexto;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        myPlayerSetup = gameObject.transform.GetParentComponent<GodoiPlayerSetup>();
    }
    public override void Use()
    {
        if (gameObject.name.Contains("Faca"))
        {
            AtacaComAFaca();
        }
        else if (municao > 0)
        {
            municao -= 1;
            Shoot();
        }
        else if (gameObject.name == "Fuzil" && municaoReserva >= 30)
        {
            municaoReserva -= 30;
            municao = 30;
        }
        else if (gameObject.name == "Pistola" && municaoReserva >= 15)
        {
            municaoReserva -= 15;
            municao = 15;
        }
        else if (gameObject.name == "Espingarda" && municaoReserva >= 4)
        {
            municaoReserva -= 4;
            municao = 4;
        }
        else if (gameObject.name == "Rifle" && municaoReserva >= 2)
        {
            municaoReserva -= 2;
            municao = 2;
        }
        AttHudMunicao();
    }
    public void AtacaComAFaca()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit, distanciaFaca))
        {
            if (hit.collider.gameObject.GetComponent<GodoiPlayerController>() == null)
            {
                Debug.Log("Objeto do Cenário");
                pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hit.point, hit.normal);
            }
            else if (hit.collider.gameObject.GetComponent<GodoiPlayerController>().playerTeam != myPlayerController.GetComponent<GodoiPlayerController>().playerTeam)
            {
                Debug.Log("Tiro no inimigo");
                hit.collider.gameObject.GetComponent<GodoiIDameagable>()?.TakeDamage(((GodoiGunInfo)itemInfo).damage);
                pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hit.point, hit.normal);
                if (hit.collider.gameObject.GetComponent<GodoiPlayerController>().currentHealth <= 0)
                {
                    myPlayerSetup.myPlayerKill++;
                }
            }
        }
    }
    public void AttHudMunicao()
    {
        if (gameObject.name.Contains("Faca"))
        {
            MunicaoNaArmaTexto.text = "";
            Barra.text = "";
            MunicaoReservaTexto.text = "";
        }
        else
        {
            MunicaoNaArmaTexto.text = (municao.ToString());
            Barra.text = "/";
            MunicaoReservaTexto.text = (municaoReserva.ToString());
        }
    }
    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<GodoiPlayerController>() == null)
            {
                Debug.Log("Objeto do Cenário");
                pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hit.point, hit.normal);
            }
            else if (hit.collider.gameObject.GetComponent<GodoiPlayerController>().playerTeam != myPlayerController.GetComponent<GodoiPlayerController>().playerTeam)
            {
                Debug.Log("Tiro no inimigo");
                hit.collider.gameObject.GetComponent<GodoiIDameagable>()?.TakeDamage(((GodoiGunInfo)itemInfo).damage);
                pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hit.point, hit.normal);
                if (hit.collider.gameObject.GetComponent<GodoiPlayerController>().currentHealth <= 0)
                {
                    myPlayerSetup.myPlayerKill++;
                }
            }
        }
    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitnormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObject = Instantiate(bulletImpactPrefab, hitPosition + hitnormal * 0.001f, Quaternion.LookRotation(hitnormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObject, 10f);
            bulletImpactObject.transform.SetParent(colliders[0].transform);
        }
    }
}
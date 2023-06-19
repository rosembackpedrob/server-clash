using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiSingleShotGun : GodoiGun
{
    [SerializeField] Camera cam;

    PhotonView pv;

    LayerMask meuLayer;

    [SerializeField]GameObject myPlayerController;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
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

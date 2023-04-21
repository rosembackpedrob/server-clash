using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiPlayerGroundCheck : MonoBehaviour
{
    GodoiPlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<GodoiPlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(false);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGroundedState(true);
    }
}
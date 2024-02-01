using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    Vector3 deflectionDirection = Vector3.zero;
    GameObject playerMesh;

    private void Start()
    {
        playerMesh = GameObject.Find("PlayerMesh");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();

            deflectionDirection = playerMesh.transform.forward;

            other.gameObject.transform.forward = deflectionDirection;

            projectile.Deflected();
        }
    }
}

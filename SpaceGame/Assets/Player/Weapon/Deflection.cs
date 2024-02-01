using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    Vector3 deflectionDirection = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();

            deflectionDirection = transform.parent.forward;

            print(other.transform.up);

            other.gameObject.transform.Rotate(Quaternion.FromToRotation(other.gameObject.transform.forward, deflectionDirection).eulerAngles);

            projectile.speed *= 1.05f;
        }
    }
}

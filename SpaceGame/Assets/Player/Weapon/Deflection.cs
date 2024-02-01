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

            other.gameObject.transform.Rotate(Quaternion.FromToRotation(other.gameObject.transform.forward, deflectionDirection).eulerAngles);

            projectile.Deflected();
        }
    }
}

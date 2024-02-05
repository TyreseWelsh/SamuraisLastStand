using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    Vector3 deflectionDirection = Vector3.zero;
    GameObject playerMesh;
    ScoringSystem scoringSystem;

    [SerializeField] GameObject newProjectilePosition;

    private void Start()
    {
        playerMesh = GameObject.Find("PlayerMesh");
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();

            deflectionDirection = playerMesh.transform.forward;

            other.gameObject.transform.forward = deflectionDirection;
            other.gameObject.transform.position = newProjectilePosition.transform.position;

            scoringSystem.IncreaseTempScore(50);
            projectile.Deflected();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    Vector3 deflectionDirection = Vector3.zero;
    GameObject playerMesh;
    Player_BaseMovement movementScript;
    ScoringSystem scoringSystem;

    [SerializeField] GameObject newProjectilePosition;

    private void Start()
    {
        playerMesh = GameObject.Find("PlayerMesh");
        movementScript = GameObject.Find("Player").GetComponent<Player_BaseMovement>();
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();

            deflectionDirection = movementScript.lookDirection;

            other.gameObject.transform.forward = deflectionDirection;
            other.gameObject.transform.position = newProjectilePosition.transform.position;

            scoringSystem.IncreaseTempScore(50);
            projectile.Deflected();
        }
    }
}

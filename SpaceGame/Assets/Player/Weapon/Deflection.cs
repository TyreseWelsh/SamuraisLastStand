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

    [SerializeField] AudioSource deflectSrc;
    [SerializeField] AudioClip deflectSound1, deflectSound2, deflectSound3;

    private void Start()
    {
        movementScript = GameObject.Find("Player").GetComponent<Player_BaseMovement>();
        playerMesh = movementScript.mesh;
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();

            deflectionDirection = movementScript.lookDirection;

            other.gameObject.transform.forward = newProjectilePosition.transform.forward;
            other.gameObject.transform.position = newProjectilePosition.transform.position;
            other.gameObject.transform.rotation = newProjectilePosition.transform.rotation;

            scoringSystem.IncreaseTempScore(50 + 50 * projectile.currentSpeedStage, projectile.currentSpeedStage,  projectile.stageColours);
            projectile.Deflected();

            PlayDeflectSound();
        }
    }

    private void PlayDeflectSound()
    {
        int randNum = Random.Range(1, 4);
        switch (randNum)
        {
            case 1:
                deflectSrc.clip = deflectSound1;
                break;
            case 2:
                deflectSrc.clip = deflectSound2;
                break;
            case 3:
                deflectSrc.clip = deflectSound3;
                break;
            default:
                deflectSrc.clip = deflectSound1;
                break;
        }

        deflectSrc.volume = 0.35f;
        deflectSrc.Play();
    }
}

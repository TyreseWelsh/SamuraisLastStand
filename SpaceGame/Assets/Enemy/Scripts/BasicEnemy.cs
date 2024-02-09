using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    Rigidbody rb;
    GravityBody gravityBody;
    Animator animator;
    GameObject spawner;
    GameObject scoreManager;
    ScoringSystem scoringSystem;


    bool alive = true;

    public static Transform playerTarget;
    CinemachineVirtualCamera playerVCam;
    float attackRange = 12;
    float attackTimer = 0.0f;
    float attackRate = 2.5f;
    bool attacking = false;

    float moveSpeed = 7.0f;

    [SerializeField] Transform projectileStart;
    [SerializeField] GameObject projectile;

    Vector3 lookDirection = Vector3.zero;

    public GameObject shield;

    [SerializeField] AudioSource shieldAudioSrc;
    [SerializeField] AudioClip shieldReflect;
    [SerializeField] AudioClip shieldShatter1, shieldShatter2;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        animator = GetComponent<Animator>();
        playerTarget = GameObject.Find("Player")?.transform;
        spawner = GameObject.Find("EnemySpawner");
        scoreManager = GameObject.Find("ScoreManager");
        scoringSystem = scoreManager?.GetComponent<ScoringSystem>();

        playerVCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (alive && !GameManager.isPaused)
        {
            if (playerTarget != null)
            {
                animator.SetBool("HasTarget", true);
            }
            else
            {
                animator.SetBool("HasTarget", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive && playerTarget != null && gravityBody.attractor != null && !GameManager.isPaused)
        {
            bool inRange = Vector3.Distance(transform.position, playerTarget.position) <= attackRange;
            animator.SetBool("Attacking", attacking);
            if (inRange || attacking)
            {
                StartFireProjectile();
            }
            else
            {
                MoveTowardsTarget();
            }

            LookAtTarget(playerTarget);
        }
        else
        {
            StopCoroutine(FireProjectile());
        }
    }

    private void LookAtTarget(Transform target)
    {
        lookDirection = Vector3.ProjectOnPlane(target.position - transform.position, gravityBody.gravityUp);
        transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
    }

    private void MoveTowardsTarget()
    {
        if(!attacking)
        {
            Debug.DrawRay(transform.position, transform.forward * moveSpeed);
            rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void StartFireProjectile()
    {
        attackTimer += Time.deltaTime;
        attacking = true;

        if (attackTimer >= attackRate)
        {
            animator.SetTrigger("Attack");
            attackTimer = 0.0f;


            StartCoroutine(FireProjectile());
        }
    }

    IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(0.07f);

        GameObject newProjectile = Instantiate(projectile, projectileStart.transform.position, Quaternion.identity);
        newProjectile.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);

        yield return new WaitForSeconds(1.0f);
        attacking = false;
    }

    public void Damage(GameObject damageSource)
    {
        EnemyProjectile projectileScript = damageSource.GetComponent<EnemyProjectile>();
        EnemyShield shieldScript = shield.GetComponent<EnemyShield>();

        if (projectileScript != null)
        {
            if (projectileScript.currentSpeedStage >= shieldScript.currentStage)
            {
                Death(damageSource);
                Destroy(damageSource);
            }
            else
            {
                ReflectProjectile(damageSource);
            }
        }

    }

    private void Death(GameObject damageSource)
    {
        alive = false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerVCam.gameObject.transform.position);
        if(distanceToPlayer <= 45)
        {
            float newShakeIntensity = (distanceToPlayer / 45 * 2) + (damageSource.GetComponent<EnemyProjectile>().currentSpeedStage / 5);
            print(newShakeIntensity);
            playerVCam.gameObject.GetComponent<CameraShake>().ShakeCamera(newShakeIntensity);
        }

        PlayShieldShatterSound();
        shield.SetActive(false);
        LookAtTarget(damageSource.transform);

        LayerMask ignoreLayers = LayerMask.GetMask("Character", "EnemyProjectile");
        GetComponent<CapsuleCollider>().excludeLayers = ignoreLayers;
        animator.SetTrigger("Death");
        scoringSystem?.BankScore();
        spawner.GetComponent<EnemySpawner>().numCurrentEnemies--;

        StartCoroutine(WaitForDeath());
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }

    private void ReflectProjectile(GameObject projectile)
    {
        shieldAudioSrc.PlayOneShot(shieldReflect);
        projectile.gameObject.transform.forward = -projectile.gameObject.transform.forward;
    }

    private void PlayShieldShatterSound()
    {
        int randNum = Random.Range(1, 3);
        switch (randNum)
        {
            case 1:
                shieldAudioSrc.PlayOneShot(shieldShatter1);
                break;
            case 2:
                shieldAudioSrc.PlayOneShot(shieldShatter2);
                break;
            default:
                shieldAudioSrc.PlayOneShot(shieldShatter1);
                break;
        }
    }
}
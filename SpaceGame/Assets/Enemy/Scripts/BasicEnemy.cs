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

    Transform target;
    float attackRange = 12;
    float attackTimer = 0.0f;
    float attackRate = 2.5f;
    bool canattack = true;

    float moveSpeed = 10.0f;

    [SerializeField] Transform projectileStart;
    [SerializeField] GameObject projectile;

    Vector3 lookDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player")?.transform;
        spawner = GameObject.Find("EnemySpawner");
        scoreManager = GameObject.Find("ScoreManager");
        scoringSystem = scoreManager?.GetComponent<ScoringSystem>();
    }

    private void FixedUpdate()
    {
        if (alive && target != null && gravityBody.attractor != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= attackRange;
            animator.SetBool("InRange", inRange);

            if (inRange)
            {
                StartFireProjectile();
            }
            else
            {
                MoveTowardsTarget();
            }

            LookAtTarget();
        }
        else
        {
            StopCoroutine(FireProjectile());
        }
    }

    private void LookAtTarget()
    {
        lookDirection = Vector3.ProjectOnPlane(target.position - transform.position, gravityBody.gravityUp);
        transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
    }

    private void MoveTowardsTarget()
    {
        Debug.DrawRay(transform.position, transform.forward * moveSpeed);
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }

    private void StartFireProjectile()
    {
        attackTimer += Time.deltaTime;

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
    }

    public void Damage()
    {
        Death();
    }

    private void Death()
    {
        alive = false;
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

}
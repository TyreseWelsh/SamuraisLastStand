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
    GameObject spawner;
    GameObject scoreManager;
    ScoringSystem scoringSystem;

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
        target = GameObject.Find("Player")?.transform;
        spawner = GameObject.Find("EnemySpawner");
        scoreManager = GameObject.Find("ScoreManager");
        scoringSystem = scoreManager?.GetComponent<ScoringSystem>();
    }

    private void FixedUpdate()
    {
        if (target != null && gravityBody.attractor != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= attackRange;

            if (inRange)
            {
                FireProjectile();
            }
            else
            {
                MoveTowardsTarget();
            }

            LookAtTarget();
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

    private void FireProjectile()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackRate)
        {
            print(projectileStart.transform.position);
            GameObject newProjectile = Instantiate(projectile, projectileStart.transform.position, Quaternion.identity);
            newProjectile.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
            attackTimer = 0.0f;
        }
    }

    public void Damage()
    {
        scoringSystem?.BankScore();
        spawner.GetComponent<EnemySpawner>().numCurrentEnemies--;
        Destroy(gameObject);
    }
}
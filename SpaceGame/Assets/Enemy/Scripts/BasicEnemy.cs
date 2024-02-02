using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour, IDamageable
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    GravityBody gravityBody;

    Transform target;
    float shootingDistance;
    float fireTimer = 0.0f;
    float fireRate = 4.0f;
    bool canFire = true;
    [SerializeField] Transform projectileStart;
    [SerializeField] GameObject projectile;
    List<Coroutine> fireCoroutines = new List<Coroutine>();

    float pathUpdateDelay = 0.2f;
    float pathUpdateDeadline = 0.0f;

    Vector3 lookDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gravityBody = GetComponent<GravityBody>();
        target = GameObject.Find("Player")?.transform;

        shootingDistance = navMeshAgent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange)
            {
                LookAtTarget();
                //fireCoroutines.Add(StartCoroutine(WaitToFire()));
                FireProjectile();
            }
            else
            {
                UpdatePath();
                //foreach (Coroutine coroutine in fireCoroutines)
                //{
                //    StopCoroutine(coroutine);
                //}
            }
        }
    }

    private void LookAtTarget()
    {
        lookDirection = Vector3.ProjectOnPlane(target.position - transform.position, gravityBody.attractor.gravityUp);
        transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.attractor.gravityUp);
    }

    private void UpdatePath()
    {
        if(Time.time > pathUpdateDeadline)
        {
            //print("Updating path...");
            pathUpdateDeadline = Time.time + pathUpdateDelay;
            navMeshAgent.SetDestination(target.position);
        }
    }

    private void FireProjectile()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            print(projectileStart.transform.position);
            Instantiate(projectile, projectileStart.transform.position, Quaternion.LookRotation(lookDirection, gravityBody.attractor.gravityUp));
            fireTimer = 0.0f;
        }
    }

    IEnumerator WaitToFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;

        if(canFire)
        {
            //Instantiate(projectile, transform.position, Quaternion.LookRotation(lookDirection, gravityBody.attractor.gravityUp));
            canFire = false;
        }

        StartCoroutine(WaitToFire());
    }

    public void Damage()
    {
        //Destroy(gameObject);
    }
}
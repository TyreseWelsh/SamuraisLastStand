using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_BaseMovement : MonoBehaviour, IDamageable
{
    Rigidbody rb;
    public GameObject mesh;
    Animator animator;
    PlayerInput input;
    public GravityBody gravityBody;
    Camera playerCamera;
    CinemachineVirtualCamera virtualCamera;

    public bool alive = true;
    public int health = 5;
    bool hit = false;
    float speed = 8.0f;

    float horizontalInput;
    float verticalInput;
    public Vector3 movementDirection;
    public Vector3 lookDirection;
    bool canDash = true;

    [SerializeField] GameObject weaponObj;
    [SerializeField] GameObject weaponSpawn;

    GameManager gameManager;
    ScoringSystem scoringSystem;
    PlayerUIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        animator = GetComponentInChildren<Animator>();

        gravityBody = GetComponent<GravityBody>();
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
        uiManager = GameObject.Find("PlayerUIManager")?.GetComponent<PlayerUIManager>();
    }

    private void Start()
    {
        GameManager.isPaused = true;
        animator.SetTrigger("DrawSword");
        StartCoroutine(WaitToDisableStartPause());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!GameManager.isPaused)
        {
            movementDirection = new Vector3(horizontalInput, 0, verticalInput);

            if (alive && gravityBody.attractor != null)
            {
                if (!hit)
                {
                    rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);
                    animator.SetFloat("Speed", movementDirection.magnitude * 100);                                                  // Multiply by 100 just for animator blend tree visual clarity

                    // Rotation to mouse code thanks to: https://forum.unity.com/threads/rotating-an-object-on-its-y-axis-while-it-is-relative-to-a-specific-normal.512838/
                    Vector3 mousePos;
                    mousePos = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));

                    lookDirection = Vector3.ProjectOnPlane(mousePos - mesh.transform.position, gravityBody.gravityUp);
                    mesh.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);

                    float lookDirToMovementDirAngle = Vector3.SignedAngle(transform.InverseTransformDirection(mesh.transform.forward), movementDirection, gravityBody.gravityUp);
                    animator.SetFloat("Direction", lookDirToMovementDirAngle);
                }
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    IEnumerator WaitToDisableStartPause()
    {
        yield return new WaitForSeconds(1.2f);
        GameManager.isPaused = false;
    }


    void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
        verticalInput = value.Get<Vector2>().y;
    }

    void OnDash(InputValue value)
    {
        if (alive && canDash && !GameManager.isPaused)
        {
            canDash = false;

            rb.AddRelativeForce(movementDirection * 600);
            StartCoroutine(WaitToResetDash());
        }
    }

    IEnumerator WaitToResetDash()
    {
        yield return new WaitForSeconds(0.4f);
        canDash = true;
    }

    void OnPause(InputValue value)
    {
        gameManager.TogglePause();
    }

    public void Damage(GameObject damageSource)
    {
        Destroy(damageSource);

        scoringSystem.ResetTempScore();
        health--;

        if(health <= 0)
        {
            PlayerDeath(damageSource);   
        }
        else
        {
            PlayerHit(damageSource);
        }
    }

    private void LookAtDamageSource(Transform damageSourceTransform)
    {
        Vector3 damageDirection = Vector3.ProjectOnPlane(damageSourceTransform.position - mesh.transform.position, gravityBody.gravityUp);

        mesh.transform.rotation = Quaternion.LookRotation(damageDirection, gravityBody.gravityUp);
    }

    private void PlayerHit(GameObject damageSource)
    {
        LookAtDamageSource(damageSource.transform);
        virtualCamera.gameObject.GetComponent<CameraShake>().ShakeCamera(2f);
        uiManager.DamageHealthText();

        hit = true;
        animator.SetBool("Hit", hit);

        StartCoroutine(WaitToEndHit());
    }

    IEnumerator WaitToEndHit()
    {
        yield return new WaitForSeconds(0.1f);

        hit = false;
        animator.SetBool("Hit", hit);
    }

    private void PlayerDeath(GameObject damageSource)
    {
        alive = false;
        health = 0;
        LookAtDamageSource(damageSource.transform);
        //Time.timeScale = 0.7f;
        GetComponentInChildren<Animator>().SetTrigger("Death");

        BasicEnemy.playerTarget = null;
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().canSpawn = false;
        DisablePlayerColliders();

        EnemyProjectile[] projectiles = FindObjectsOfType<EnemyProjectile>();
        foreach(EnemyProjectile projectile in projectiles)
        {
            projectile.Dissipate();
        }
    }

    private void DisablePlayerColliders()
    {
        LayerMask excludedLayers = LayerMask.GetMask("Character", "EnemyProjectile");
        GetComponent<CapsuleCollider>().excludeLayers = excludedLayers;
        GetComponentInChildren<SphereCollider>().enabled = false;
    }
}
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
    GameObject mesh;
    Animator animator;
    PlayerInput input;
    public GravityBody gravityBody;
    Camera playerCamera;
    CinemachineVirtualCamera virtualCamera;

    public bool alive = true;
    int health = 5;
    [SerializeField] TextMeshProUGUI healthText;
    bool hit = false;
    float speed = 8.0f;

    float horizontalInput;
    float verticalInput;
    public Vector3 movementDirection;
    public Vector3 lookDirection;
    bool canDash = true;

    [SerializeField] GameObject weaponObj;
    [SerializeField] GameObject weaponSpawn;

    ScoringSystem scoringSystem;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        animator = GetComponentInChildren<Animator>();

        gravityBody = GetComponent<GravityBody>();
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "HEALTH: " + health.ToString();
    }

    private void FixedUpdate()
    {
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        if(alive && gravityBody.attractor != null)
        {
            if(!hit)
            {
                rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);

                // Rotation to mouse code thanks to: https://forum.unity.com/threads/rotating-an-object-on-its-y-axis-while-it-is-relative-to-a-specific-normal.512838/
                Vector3 mousePos;
                mousePos = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));

                lookDirection = Vector3.ProjectOnPlane(mousePos - mesh.transform.position, gravityBody.gravityUp);
                Debug.DrawRay(transform.position, lookDirection * 50);
                mesh.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
        verticalInput = value.Get<Vector2>().y;
    }

    void OnDash(InputValue value)
    {
        if (alive && canDash)
        {
            canDash = false;

            rb.AddRelativeForce(movementDirection * 200);
            StartCoroutine(WaitToResetDash());
        }
    }

    IEnumerator WaitToResetDash()
    {
        yield return new WaitForSeconds(0.4f);
        canDash = true;
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
        virtualCamera.gameObject.GetComponent<CameraShake>().ShakeCamera(1f);

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
        Time.timeScale = 0.7f;
        GetComponentInChildren<Animator>().SetTrigger("Death");

        BasicEnemy.playerTarget = null;
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().canSpawn = false;
        DisablePlayerColliders();

        print("Player Death");
    }

    private void DisablePlayerColliders()
    {
        LayerMask excludedLayers = LayerMask.GetMask("Character", "EnemyProjectile");
        GetComponent<CapsuleCollider>().excludeLayers = excludedLayers;
        GetComponentInChildren<SphereCollider>().enabled = false;
    }
}
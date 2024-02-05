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
    PlayerInput input;
    public GravityBody gravityBody;
    Camera playerCamera;

    public bool alive = true;
    int health = 5;
    [SerializeField] TextMeshProUGUI healthText;
    float speed = 8.0f;
    [SerializeField] float turnSpeed = 1000;
    public Vector3 movementDirection;
    public Vector3 lookDirection;
    bool canDash = true;

    [SerializeField] GameObject weaponObj;
    [SerializeField] GameObject weaponSpawn;

    ScoringSystem scoringSystem;

    float horizontalInput;
    float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        gravityBody = GetComponent<GravityBody>();
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
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
            rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);

            // Rotation to mouse code thanks to: https://forum.unity.com/threads/rotating-an-object-on-its-y-axis-while-it-is-relative-to-a-specific-normal.512838/
            Vector3 mousePos;
            mousePos = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));

            lookDirection = Vector3.ProjectOnPlane(mousePos - mesh.transform.position, gravityBody.gravityUp);

            mesh.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
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

    public void Damage(Transform damageSourceTransform)
    {
        scoringSystem.ResetTempScore();
        health--;

        if(health <= 0)
        {
            PlayerDeath(damageSourceTransform);   
        }
    }

    private void LookAtDamageSource(Transform damageSourceTransform)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(damageSourceTransform.position - mesh.transform.position, gravityBody.gravityUp);

        mesh.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
    }

    private void PlayerDeath(Transform damageSourceTransform)
    {
        alive = false;
        health = 0;
        LookAtDamageSource(damageSourceTransform);
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
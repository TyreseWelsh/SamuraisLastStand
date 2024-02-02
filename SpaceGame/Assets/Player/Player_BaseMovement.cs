using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_BaseMovement : MonoBehaviour, IDamageable
{
    Rigidbody rb;
    GameObject mesh;
    PlayerInput input;
    GravityBody gravityBody;

    Camera playerCamera;

    float speed = 8;
    [SerializeField] float turnSpeed = 1000;
    Vector3 movementDirection;
    bool canDash = true;

    [SerializeField] GameObject weaponObj;
    [SerializeField] GameObject weaponSpawn;
    GameObject spawnedWeapon;
    bool canReflect = true;

    float horizontalInput;
    float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        gravityBody = GetComponent<GravityBody>();
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Debug.DrawRay(transform.position, mesh.transform.forward * 2.25f, Color.blue);
        }

    }

    private void FixedUpdate()
    {
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        if(gravityBody.attractor != null)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);

            // Rotation to mouse code thanks to: https://forum.unity.com/threads/rotating-an-object-on-its-y-axis-while-it-is-relative-to-a-specific-normal.512838/
            Vector3 mousePos;
            mousePos = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));

            Vector3 lookDirection = Vector3.ProjectOnPlane(mousePos - mesh.transform.position, gravityBody.gravityUp);

            mesh.transform.rotation = Quaternion.LookRotation(lookDirection, gravityBody.gravityUp);
        }
    }

    void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
        verticalInput = value.Get<Vector2>().y;
    }

    void OnDash(InputValue value)
    {
        //rb.AddRelativeForce(Vector3.up * jumpHeight);
        if (canDash)
        {
            canDash = false;

            rb.AddRelativeForce(movementDirection * 1500);
            StartCoroutine(WaitToResetDash());
        }
    }

    IEnumerator WaitToResetDash()
    {
        yield return new WaitForSeconds(0.4f);
        canDash = true;
    }

    void OnAttack(InputValue value)
    {
        if(canReflect)
        {
            spawnedWeapon = Instantiate(weaponObj, weaponSpawn.transform, false);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            canReflect = false;

            StartCoroutine(WaitToDestroyWeapon());
        }
    }

    IEnumerator WaitToDestroyWeapon()
    {
        yield return new WaitForSeconds(0.40f);

        Destroy(spawnedWeapon);
        canReflect = true;
    }

    public void Damage()
    {
        //Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
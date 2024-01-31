using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_BaseMovement : MonoBehaviour
{
    Rigidbody rb;
    GameObject mesh;
    PlayerInput input;
    GravityBody gravityBody;

    [SerializeField] float speed = 5;
    [SerializeField] float turnSpeed = 500;
    Vector3 movementDirection;
    [SerializeField] float jumpHeight = 1400;

    //[SerializeField] GameObject currentPlanet;
    float horizontalInput;
    float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        gravityBody = GetComponent<GravityBody>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnMove(InputValue value)
    {
        horizontalInput = value.Get<Vector2>().x;
        verticalInput = value.Get<Vector2>().y;
    }

    void OnJump(InputValue value)
    {
        rb.AddRelativeForce(Vector3.up * jumpHeight);
    }

    private void FixedUpdate()
    {
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        //movementDirection.Normalize();

        if (transform.TransformDirection(movementDirection) != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(transform.TransformDirection(movementDirection), gravityBody.attractor.gravityUp);

            mesh.transform.rotation = Quaternion.RotateTowards(mesh.transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
        

        rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);
    }
}
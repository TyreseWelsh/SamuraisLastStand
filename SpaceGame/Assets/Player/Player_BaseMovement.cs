using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_BaseMovement : MonoBehaviour
{
    Rigidbody rb;
    GameObject mesh;

    [SerializeField] float speed = 5;
    [SerializeField] float turnSpeed = 500;
    Vector3 movementDirection;

    [SerializeField] GameObject currentPlanet;
    GravityAttractor currentAttractor;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GameObject.Find("PlayerMesh");
        currentAttractor = currentPlanet.GetComponent<GravityAttractor>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        if (transform.TransformDirection(movementDirection) != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(transform.TransformDirection(movementDirection), currentAttractor.gravityUp);

            mesh.transform.rotation = Quaternion.RotateTowards(mesh.transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
        

        rb.MovePosition(rb.position + transform.TransformDirection(movementDirection) * speed * Time.deltaTime);
    }
}
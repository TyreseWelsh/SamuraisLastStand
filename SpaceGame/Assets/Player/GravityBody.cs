using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    Transform bodyTransform;
    Rigidbody rb;

    public GravityAttractor attractor;

    // Start is called before the first frame update
    void Start()
    {
        bodyTransform = transform;
        rb = GetComponent<Rigidbody>();

        //attractor = attractingBody.GetComponent<GravityAttractor>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        attractor?.AttractBody(gameObject);
    }
}

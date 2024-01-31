using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GravityAttractor : MonoBehaviour
{
    [SerializeField] float gravity = 1.62f;

    public Vector3 gravityUp;

    public void AttractBody(GameObject body)
    {
        gravityUp = body.transform.position - transform.position;
        Vector3 bodyUp = body.transform.up;

        Rigidbody rb = body.GetComponent<Rigidbody>();
        rb.AddForce(gravityUp * gravity);
        
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.transform.rotation;
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, 60);
    }
}

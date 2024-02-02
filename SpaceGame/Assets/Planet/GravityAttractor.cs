using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -2f;

    //public Vector3 gravityUp;

    public void AttractBody(GameObject body)
    {
        GravityBody bodyScript = body.GetComponent<GravityBody>();

        if (bodyScript != null ) 
        {
            bodyScript.gravityUp = body.transform.position - transform.position;
            Vector3 bodyUp = body.transform.up;

            Rigidbody rb = body.GetComponent<Rigidbody>();
            rb.AddForce(bodyScript.gravityUp * gravity);

            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, bodyScript.gravityUp) * body.transform.rotation;
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, 60);

            Debug.DrawRay(transform.position, body.transform.position, Color.yellow);
        }
    }
}
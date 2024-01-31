using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GravityBody>() != null)
        {
            print("Changing planet");

            other.gameObject.GetComponent<GravityBody>().attractor = gameObject.GetComponent<GravityAttractor>();
        }
    }
}

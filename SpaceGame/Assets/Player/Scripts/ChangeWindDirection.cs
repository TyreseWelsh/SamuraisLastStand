using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWindDirection : MonoBehaviour
{
    Player_BaseMovement playerMovement;
    GameObject globalWind;

    Quaternion baseRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<Player_BaseMovement>();
        globalWind = GameObject.Find("GlobalWind");
        baseRotation = globalWind.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.movementDirection != Vector3.zero)
        {
            Vector3 windDirection = Vector3.ProjectOnPlane((transform.position + transform.TransformDirection(playerMovement.movementDirection)) - transform.position, playerMovement.gravityBody.gravityUp);

            globalWind.transform.rotation = Quaternion.LookRotation(-windDirection, playerMovement.gravityBody.gravityUp);
        }
        else
        {
            globalWind.transform.rotation = baseRotation;
        }
    }
}

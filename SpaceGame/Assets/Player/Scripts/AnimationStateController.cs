using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    Player_BaseMovement movementScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movementScript = GetComponent<Player_BaseMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VelZ", movementScript.movementDirection.z, 0.12f, Time.deltaTime);
        animator.SetFloat("VelX", movementScript.movementDirection.x, 0.12f, Time.deltaTime);
    }
}

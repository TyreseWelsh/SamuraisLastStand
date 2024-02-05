using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    Player_BaseMovement movementScript;

    bool canDeflect = true;
    [SerializeField] GameObject deflectCollider;

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

    void OnAttack(InputValue value)
    {
        print("DEFLECT");
        if(canDeflect)
        {
            animator.SetTrigger("Attack");
            animator.SetLayerWeight(1, 1);
            deflectCollider.SetActive(true);
            canDeflect = false;

            StartCoroutine(WaitToDisableDeflect());
        }
    }

    IEnumerator WaitToDisableDeflect()
    {
        yield return new WaitForSeconds(0.75f);

        animator.SetLayerWeight(1, 0);
        deflectCollider.SetActive(false);
        canDeflect = true; 
    }
}

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

    float attackTimer = 0;
    float inwardAttackTime = 0.8f;

    float targetLayerWeight = 0;
    float currentLayerWeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movementScript = GetComponent<Player_BaseMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.alive)
        {
            animator.SetFloat("VelZ", movementScript.movementDirection.z, 0.12f, Time.deltaTime);
            animator.SetFloat("VelX", movementScript.movementDirection.x, 0.12f, Time.deltaTime);

            if (currentLayerWeight > targetLayerWeight)
            {
                attackTimer += 0.2f;

                if (attackTimer >= inwardAttackTime)
                {
                    currentLayerWeight -= Time.deltaTime;

                    animator.SetLayerWeight(1, currentLayerWeight);
                }
            }
            else if (currentLayerWeight <= targetLayerWeight)
            {
                animator.SetLayerWeight(1, 0);

                DisableDeflect();
                attackTimer = 0;
            }
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    void OnAttack(InputValue value)
    {
        if(canDeflect && movementScript.alive)
        {
            animator.SetTrigger("Attack");
            animator.SetLayerWeight(1, 1);
            currentLayerWeight = 1.0f;
            deflectCollider.SetActive(true);
            canDeflect = false;
        }
    }

    IEnumerator WaitToDisableDeflect()
    {
        yield return new WaitForSeconds(0.5f);



        if(currentLayerWeight <= targetLayerWeight)
        {
            deflectCollider.SetActive(false);
            canDeflect = true;
            currentLayerWeight = 0;
        }
    }

    void DisableDeflect()
    {
        deflectCollider.SetActive(false);
        canDeflect = true;
        currentLayerWeight = 0;
    }
}

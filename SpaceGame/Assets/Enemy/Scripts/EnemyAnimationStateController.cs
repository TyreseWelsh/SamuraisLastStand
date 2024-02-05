using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationStateController : MonoBehaviour
{
    Animator animator;
    BasicEnemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<BasicEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

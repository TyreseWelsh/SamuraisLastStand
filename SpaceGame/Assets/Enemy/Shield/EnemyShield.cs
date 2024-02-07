using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamageable
{
    BasicEnemy owningEnemyScript;

    [ColorUsage(true, true)]
    public Color[] stageColours;
    public Renderer renderer;
    Material shieldMaterial;

    public int currentStage = 1;


    void Awake()
    {
        owningEnemyScript = GetComponentInParent<BasicEnemy>();
        shieldMaterial = renderer.material;
        SetToStage1();
    }

    public void SetStage(int stage)
    {
        switch(stage)
        {
            case 1:
                SetToStage1(); 
                break;
            case 2:
                SetToStage2();
                break;
            case 3:
                SetToStage3();
                break;
            case 4:
                SetToStage4();
                break;
            case 5:
                SetToStage5();
                break;
        }
    }

    public void SetToStage1()
    {
        currentStage = 1;
        shieldMaterial.SetColor("_Colour", stageColours[0]);
    }

    public void SetToStage2()
    {
        currentStage = 2;
        shieldMaterial.SetColor("_Colour", stageColours[1]);
    }
    public void SetToStage3()
    {
        currentStage = 3;
        shieldMaterial.SetColor("_Colour", stageColours[2]);
    }

    public void SetToStage4()
    {
        currentStage = 4;
        shieldMaterial.SetColor("_Colour", stageColours[3]);
    }
    public void SetToStage5()
    {
        currentStage = 5;
        shieldMaterial.SetColor("_Colour", stageColours[4]);
    }

    public void Damage(GameObject damageSource)
    {
        owningEnemyScript.Damage(damageSource);
    }
}

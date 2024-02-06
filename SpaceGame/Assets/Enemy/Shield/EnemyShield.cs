using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamageable
{
    //public enum ShieldStage
    //{
    //    Stage1,
    //    Stage2,
    //    Stage3,
    //    Stage4,
    //    Stage5,
    //}
    BasicEnemy owningEnemyScript;

    [ColorUsage(true, true)]
    public Color[] stageColours;
    public Renderer renderer;
    Material shieldMaterial;

    public int currentStage = 1;


    // Start is called before the first frame update
    void Start()
    {
        owningEnemyScript = GetComponentInParent<BasicEnemy>();
        shieldMaterial = renderer.material;
        SetToStage1();
    }

    // Update is called once per frame
    void Update()
    {
        
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

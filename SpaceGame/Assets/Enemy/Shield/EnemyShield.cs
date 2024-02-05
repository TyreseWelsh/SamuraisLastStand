using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    public enum ShieldStage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }

    public Color[] stageColours;
    public Renderer renderer;
    Material shieldMaterial;

    public ShieldStage currentStage = ShieldStage.Stage1;

    // Start is called before the first frame update
    void Start()
    {
        shieldMaterial = renderer.material;
        SetToStage1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetToStage1()
    {
        currentStage = ShieldStage.Stage1;
        shieldMaterial.SetColor("_Colour", stageColours[0]);
    }

    public void SetToStage2()
    {
        currentStage = ShieldStage.Stage2;
        shieldMaterial.SetColor("_Colour", stageColours[1]);
    }
    public void SetToStage3()
    {
        currentStage = ShieldStage.Stage3;
        shieldMaterial.SetColor("_Colour", stageColours[2]);
    }

    public void SetToStage4()
    {
        currentStage = ShieldStage.Stage4;
        shieldMaterial.SetColor("_Colour", stageColours[3]);
    }
    public void SetToStage5()
    {
        currentStage = ShieldStage.Stage5;
        shieldMaterial.SetColor("_Colour", stageColours[4]);
    }
}

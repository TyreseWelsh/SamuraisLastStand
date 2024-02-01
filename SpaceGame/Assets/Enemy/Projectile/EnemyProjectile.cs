using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    enum SpeedState
    {
        Stage0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }

    [SerializeField] ParticleSystem mainParticleSystem;
    [SerializeField] Color[] stageColours;

    Rigidbody rb;
    GravityBody gravityBody;
    SpeedState currentSpeedState = SpeedState.Stage0;
    Color currentProjectileColour;

    public float speed = 18;
    const float MAX_SPEED =  52.0f;
    float damage = 5;

    int timesDeflected = 0;
    [SerializeField] GameObject deflectionParticles;

    ParticleSystem.MainModule particleSystemMain;

    private void Awake()
    {
        particleSystemMain = mainParticleSystem.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        var main = mainParticleSystem.main;
        gravityBody = GetComponent<GravityBody>();
        SetToStage0();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(gravityBody.attractor != null)
        {
            Vector3 currentDistance = transform.position - gravityBody.attractor.transform.position;

            if (currentDistance.magnitude > 12)
            {
                Vector3.MoveTowards(transform.position, gravityBody.attractor.transform.position, 20);
            }
        }

        rb.MovePosition(rb.position + gameObject.transform.forward * speed * Time.deltaTime);
    }

    // To test speeds after so many deflections, can do (speed increase^how many times deflected) * base speed          16 * 1.1^15 = 66 which could be the max
    public void Deflected()
    {
        timesDeflected++;

        speed *= 1.16f;
        
        if (speed >= MAX_SPEED)
        {
            speed = MAX_SPEED;
        }


        print("Hits= " + timesDeflected + " Current Speed= " + speed);


        if(timesDeflected == 1)
        {
            SetToStage1();
        }
        else if(timesDeflected >= 2 && timesDeflected < 4)
        {
            SetToStage2();
        }
        else if(timesDeflected >= 4 && timesDeflected < 6)
        {
            SetToStage3();
        }
        else if (timesDeflected >= 6 && timesDeflected < 8)
        {
            SetToStage4();
        }
        else if (timesDeflected >= 8)
        {
            SetToStage5();
        }

        GameObject spawnedDeflectionParticles = Instantiate(deflectionParticles, transform.position, Quaternion.identity);
        spawnedDeflectionParticles?.GetComponent<ChangeDeflectionColour>().SetColour(particleSystemMain.startColor);

        GameObject.Destroy(spawnedDeflectionParticles, 1.0f);
    }

    IEnumerator ToDestroyDeflectionParticles()
    {
        yield return new WaitForSeconds(1);
        
    }

    private void SetToStage0()
    {
        currentSpeedState = SpeedState.Stage0;
        particleSystemMain.startColor = stageColours[0];
        damage = 5;
    }

    private void SetToStage1()
    {
        currentSpeedState = SpeedState.Stage1;
        particleSystemMain.startColor = stageColours[1];
        damage = 15;
    }

    private void SetToStage2()
    {
        currentSpeedState = SpeedState.Stage2;
        particleSystemMain.startColor = stageColours[2];
        damage = 25;
    }
    private void SetToStage3()
    {
        currentSpeedState = SpeedState.Stage3;
        particleSystemMain.startColor = stageColours[3];
        damage = 35;
    }

    private void SetToStage4()
    {
        currentSpeedState = SpeedState.Stage4;
        particleSystemMain.startColor = stageColours[4];
        damage = 45;
    }
    private void SetToStage5()
    {
        currentSpeedState = SpeedState.Stage5;
        particleSystemMain.startColor = stageColours[5];
        damage = 60;
    }
}

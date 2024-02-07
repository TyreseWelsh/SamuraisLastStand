using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //enum SpeedState
    //{
    //    Stage0,
    //    Stage1,
    //    Stage2,
    //    Stage3,
    //    Stage4,
    //    Stage5,
    //}

    [SerializeField] ParticleSystem mainParticleSystem;

    public int currentSpeedStage = 0;
    [ColorUsage(true, true)]
    [SerializeField] Color[] stageColours;
    Color currentProjectileColour;

    Rigidbody rb;
    GravityBody gravityBody;
    // currentSpeedState = SpeedState.Stage0;

    public float speed = 20;
    const float MAX_SPEED =  52.0f;
    float damage = 5;

    int timesDeflected = 0;
    [SerializeField] GameObject deflectionParticles;

    ParticleSystem.MainModule particleSystemMain;

    const int DESTRUCTION_TIME = 10;
    float destructionTimer;

    [SerializeField] AudioSource mainSrc;
    [SerializeField] AudioClip inAirSound;

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

        mainSrc.clip = inAirSound;
        mainSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        destructionTimer += Time.deltaTime;
        if(destructionTimer >= DESTRUCTION_TIME )
        {
            Destroy(gameObject);
        }
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
        destructionTimer = 0.0f;

        speed *= 1.15f;
        
        if (speed >= MAX_SPEED)
        {
            speed = MAX_SPEED;
        }


        print("Hits= " + timesDeflected + " Current Speed= " + speed);


        if(timesDeflected == 2 || timesDeflected == 3)
        {
            SetToStage1();
        }
        else if(timesDeflected >= 4 && timesDeflected < 6)
        {
            SetToStage2();
        }
        else if(timesDeflected >= 6 && timesDeflected < 8)
        {
            SetToStage3();
        }
        else if (timesDeflected >= 8 && timesDeflected < 10)
        {
            SetToStage4();
        }
        else if (timesDeflected >= 10)
        {
            SetToStage5();
        }

        GameObject spawnedDeflectionParticles = Instantiate(deflectionParticles, transform.position, Quaternion.identity);
        spawnedDeflectionParticles?.GetComponent<ChangeDeflectionColour>().SetColour(particleSystemMain.startColor);

        GameObject.Destroy(spawnedDeflectionParticles, 1.0f);
    }

    private void SetToStage0()
    {
        currentSpeedStage = 0;
        particleSystemMain.startColor = stageColours[0];
        damage = 5;
    }

    private void SetToStage1()
    {
        currentSpeedStage = 1;
        particleSystemMain.startColor = stageColours[1];
        damage = 15;
    }

    private void SetToStage2()
    {
        currentSpeedStage = 2;
        particleSystemMain.startColor = stageColours[2];
        damage = 25;
    }
    private void SetToStage3()
    {
        currentSpeedStage = 3;
        particleSystemMain.startColor = stageColours[3];
        damage = 35;
    }

    private void SetToStage4()
    {
        currentSpeedStage = 4;
        particleSystemMain.startColor = stageColours[4];
        damage = 45;
    }
    private void SetToStage5()
    {
        currentSpeedStage = 5;
        particleSystemMain.startColor = stageColours[5];
        damage = 60;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<IDamageable>() != null)          // If colliding object is player and implements IDamageable, deal damage
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(this.gameObject);
        }
        else if (collision.gameObject.GetComponent<IDamageable>() != null)         // If colliding object implements IDamageable and the current speed stage is not 0, deal damage
        {                                                                                                                   // Done after player check because if colliding object is Player, it will stop their before this
            collision.gameObject.GetComponent<IDamageable>().Damage(this.gameObject);
        }
    }
}
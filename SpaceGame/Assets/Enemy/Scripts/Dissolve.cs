using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer[] renderers;
    float dissolveTimer;
    //const float DISSOLVE_TIME = 1.5f;
    bool dissolve = false;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(dissolve)
        {
            dissolveTimer += Time.deltaTime;

            foreach(Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_DissolveProgress", dissolveTimer * 0.75f);
            }
        }
    }

    public void StartDissolve()
    {
        dissolve = true;
    }
}

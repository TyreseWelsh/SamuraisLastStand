using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDeflectionColour : MonoBehaviour
{
    [SerializeField] ParticleSystem[] involvedParticles;

   public void SetColour(ParticleSystem.MinMaxGradient newColour)
    {
        foreach (var particle in involvedParticles)
        {
            var main = particle.main;

            main.startColor = newColour;
        }
    }
}

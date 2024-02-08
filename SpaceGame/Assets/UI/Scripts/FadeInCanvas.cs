using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInCanvas : MonoBehaviour
{
    [SerializeField] GameObject fadeFill;
    Image fillImage;
    Color newFillColour;

    float fadeTimer = 0;
    const float FADE_TIME = 1f;

    private void Awake()
    {
        fillImage = fadeFill.GetComponent<Image>();
        newFillColour = fillImage.color;
    }

    private void Update()
    {
        if(fadeTimer <= FADE_TIME)
        {
            fadeTimer += Time.deltaTime / 2;
            newFillColour.a = 1 - fadeTimer;
            fillImage.color = newFillColour;
        }
    }
}

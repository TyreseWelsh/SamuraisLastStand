using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] Player_BaseMovement playerScript;

    [SerializeField] TextMeshProUGUI playerHealthText;
    float damagedHealthTimer;
    float DAMAGED_HEALTH_TIME;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript != null)
        {
            playerHealthText.text = "HP: " + playerScript.health.ToString();
        }
    }

    public void DamageHealthText()
    {
        StopCoroutine(WaitToReturnDamagedHealth());

        playerHealthText.color = Color.red;
        StartCoroutine(WaitToReturnDamagedHealth());
    }

    IEnumerator WaitToReturnDamagedHealth()
    {
        yield return new WaitForSeconds(0.3f);
        playerHealthText.color = Color.white;
    }
}

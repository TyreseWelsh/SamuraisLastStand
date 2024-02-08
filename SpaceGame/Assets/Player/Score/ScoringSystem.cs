using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public  int tempScore = 0;
    public  int bankedScore = 0;

    private  float loseScoreTimer = 0;
    const int LOSE_SCORE_TIME = 8;

    [SerializeField] TextMeshProUGUI tempScoreText;
    [SerializeField] TextMeshProUGUI bankedScoreText;

    [ColorUsage(true, true)]
    [SerializeField] Color[] tempScoreTextIncreaseColours;

    private void Update()
    {
        loseScoreTimer += Time.deltaTime;
        if(loseScoreTimer >= LOSE_SCORE_TIME)
        {
            ResetTempScore();
        }

        tempScoreText.text = ("+ " + tempScore);
        bankedScoreText.text = ("SCORE: " + bankedScore);
    }

    public void IncreaseTempScore(int score, int projectileSpeedStage, Color[] projectileStageColours)
    {
        StopCoroutine(WaitToReturnTempScoreText());

        tempScore += score;
        loseScoreTimer = 0;

        tempScoreText.fontSize += 7 * projectileSpeedStage;
        tempScoreText.color = tempScoreTextIncreaseColours[projectileSpeedStage];
        // MIGHT ALSO PLAY SOUND with slightly higher pitch depending on speed stage


        StartCoroutine(WaitToReturnTempScoreText());
    }

    IEnumerator WaitToReturnTempScoreText()
    {
        yield return new WaitForSeconds(0.4f);
        tempScoreText.fontSize = 95;
        tempScoreText.color = Color.white;
    }

    public void ResetTempScore()
    {
        // Play text animation where score rapidly decreases to 0
        tempScore = 0;
        //print("Scoring: Reset Temp Score...");
    }

    public void BankScore()
    {
        bankedScore += tempScore;
        tempScore = 0;
        loseScoreTimer = 0;
        //print("Scoring: Banked Score...");
    }
}
